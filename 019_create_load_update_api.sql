/*
    FleetFlow
    Load update database API.

    Actualiza la información comercial de una carga,
    protege contra modificaciones concurrentes y evita
    cambios incompatibles con un viaje en operación.
*/

USE FleetFlowDb;
GO

CREATE OR ALTER PROCEDURE operations.Load_Update
    @LoadId bigint,
    @LoadNumber varchar(30),
    @CustomerId bigint,
    @Description nvarchar(300),
    @Commodity nvarchar(100) = NULL,
    @WeightLbs decimal(12,2),
    @Pieces int = NULL,
    @RevenueAmount decimal(14,2) = NULL,
    @SpecialInstructions nvarchar(1000) = NULL,
    @ExpectedRowVersion binary(8)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    /*
        Validaciones básicas independientes
        del estado actual de la carga.
    */
    IF @LoadId <= 0
    BEGIN
        THROW 51029,
            'A valid LoadId is required.',
            1;
    END;

    IF NULLIF(
        LTRIM(RTRIM(@LoadNumber)),
        '') IS NULL
    BEGIN
        THROW 51031,
            'Load number is required.',
            1;
    END;

    IF @CustomerId <= 0
    BEGIN
        THROW 51032,
            'A valid customer is required.',
            1;
    END;

    IF NULLIF(
        LTRIM(RTRIM(@Description)),
        '') IS NULL
    BEGIN
        THROW 51033,
            'Description is required.',
            1;
    END;

    IF @WeightLbs <= 0
    BEGIN
        THROW 51034,
            'Weight must be greater than zero.',
            1;
    END;

    IF @Pieces IS NOT NULL
       AND @Pieces <= 0
    BEGIN
        THROW 51035,
            'Pieces must be greater than zero.',
            1;
    END;

    IF @RevenueAmount IS NOT NULL
       AND @RevenueAmount < 0
    BEGIN
        THROW 51036,
            'Revenue cannot be negative.',
            1;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        /*
            UPDLOCK mantiene estable la carga mientras
            se validan las reglas y se ejecuta el UPDATE.
        */
        DECLARE @CurrentLoadNumber varchar(30);
        DECLARE @CurrentCustomerId bigint;
        DECLARE @CurrentLoadStatusCode varchar(30);
        DECLARE @CurrentRowVersion binary(8);

        SELECT
            @CurrentLoadNumber =
                load.LoadNumber,

            @CurrentCustomerId =
                load.CustomerId,

            @CurrentLoadStatusCode =
                loadStatus.Code,

            @CurrentRowVersion =
                load.RowVersion
        FROM dbo.Loads AS load WITH
            (UPDLOCK, HOLDLOCK)
        INNER JOIN dbo.LoadStatuses AS loadStatus
            ON loadStatus.LoadStatusId =
                load.LoadStatusId
        WHERE load.LoadId = @LoadId;

        /*
            La carga pudo eliminarse después de que
            el usuario abrió el formulario.
        */
        IF @CurrentRowVersion IS NULL
        BEGIN
            THROW 51030,
                'The load no longer exists.',
                1;
        END;

        /*
            La versión debe coincidir con la que recibió
            originalmente el formulario.
        */
        IF @CurrentRowVersion <>
           @ExpectedRowVersion
        BEGIN
            THROW 51030,
                'The load was changed by another user.',
                1;
        END;

        /*
            La edición general solamente se permite
            mientras la carga sea nueva o planificada.

            Los estados operacionales se modificarán
            posteriormente mediante operaciones específicas
            del flujo de despacho.
        */
        IF @CurrentLoadStatusCode NOT IN
           ('NEW', 'PLANNED')
        BEGIN
            THROW 51037,
                'Only new or planned loads can be edited.',
                1;
        END;

        /*
            Una carga puede tener como máximo un viaje
            debido a UQ_Trips_LoadId.
        */
        DECLARE @TripId bigint;
        DECLARE @TripStatusCode varchar(40);

        SELECT
            @TripId =
                trip.TripId,

            @TripStatusCode =
                tripStatus.Code
        FROM dbo.Trips AS trip
        INNER JOIN dbo.TripStatuses AS tripStatus
            ON tripStatus.TripStatusId =
                trip.TripStatusId
        WHERE trip.LoadId = @LoadId;

        /*
            Si el viaje ya comenzó, la carga deja de ser
            editable mediante la operación comercial general.
        */
        IF @TripId IS NOT NULL
           AND @TripStatusCode NOT IN
               ('PLANNED', 'OFFERED', 'ASSIGNED')
        BEGIN
            THROW 51038,
                'The load cannot be edited after its trip has started.',
                1;
        END;

        /*
            Cuando ya existe un viaje, conservamos la identidad
            comercial de la carga para evitar inconsistencias.
        */
        IF @TripId IS NOT NULL
           AND
           (
               LTRIM(RTRIM(@LoadNumber)) <>
                   @CurrentLoadNumber

               OR @CustomerId <>
                   @CurrentCustomerId
           )
        BEGIN
            THROW 51039,
                'Load number and customer cannot be changed after a trip is assigned.',
                1;
        END;

        /*
            Se permite conservar un cliente que posteriormente
            fue desactivado, pero no cambiar la carga hacia otro
            cliente inexistente o inactivo.
        */
        IF @CustomerId <> @CurrentCustomerId
           AND NOT EXISTS
           (
               SELECT 1
               FROM dbo.Customers
               WHERE CustomerId = @CustomerId
                 AND IsActive = 1
           )
        BEGIN
            THROW 51040,
                'The selected customer does not exist or is inactive.',
                1;
        END;

        /*
            El estado no forma parte del UPDATE.
            Esta operación modifica únicamente información
            comercial y descriptiva de la carga.
        */
        UPDATE dbo.Loads
        SET
            LoadNumber =
                LTRIM(RTRIM(@LoadNumber)),

            CustomerId =
                @CustomerId,

            Description =
                LTRIM(RTRIM(@Description)),

            Commodity =
                NULLIF(
                    LTRIM(RTRIM(@Commodity)),
                    ''),

            WeightLbs =
                @WeightLbs,

            Pieces =
                @Pieces,

            RevenueAmount =
                @RevenueAmount,

            SpecialInstructions =
                NULLIF(
                    LTRIM(
                        RTRIM(
                            @SpecialInstructions)),
                    ''),

            UpdatedAtUtc =
                SYSUTCDATETIME()
        WHERE LoadId = @LoadId
          AND RowVersion =
              @ExpectedRowVersion;

        /*
            Esta segunda comprobación mantiene la protección
            incluso si la implementación cambia posteriormente.
        */
        IF @@ROWCOUNT = 0
        BEGIN
            THROW 51030,
                'The load was changed by another user or no longer exists.',
                1;
        END;

        /*
            SQL Server genera automáticamente una nueva
            RowVersion después del UPDATE.
        */
        SELECT
            LoadId,
            RowVersion
        FROM dbo.Loads
        WHERE LoadId = @LoadId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        THROW;
    END CATCH;
END;
GO

/*
    Confirma que el procedimiento existe.
    Esta validación no modifica información.
*/
SELECT
    SCHEMA_NAME(schema_id) AS SchemaName,
    name AS ProcedureName
FROM sys.procedures
WHERE schema_id =
      SCHEMA_ID('operations')
  AND name =
      'Load_Update';
GO