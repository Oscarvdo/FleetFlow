USE [FleetFlowDb]
GO

DECLARE	@return_value Int

EXEC	@return_value = [catalog].[Customer_Search]

SELECT	@return_value as 'Return Value'

GO
