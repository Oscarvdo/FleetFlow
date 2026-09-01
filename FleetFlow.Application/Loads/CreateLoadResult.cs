using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Application.Loads
{

    /// <summary>
    /// Representa el resultado generado después
    /// de crear correctamente una carga.
    /// </summary>
    public sealed class CreateLoadResult
    {
        /// <summary>
        /// Identificador interno generado por SQL Server.
        /// </summary>
        public long LoadId { get; init; }

        /// <summary>
        /// Valor utilizado posteriormente para detectar
        /// modificaciones concurrentes.
        /// </summary>
        public byte[] RowVersion { get; init; } = [];
    }
}
