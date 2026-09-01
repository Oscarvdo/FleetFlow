using FleetFlow.Application.Loads;
using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Application.Abstractions.Loads
{
    /// <summary>
    /// Define las operaciones que crean o modifican
    /// cargas dentro de FleetFlow.
    /// </summary>
    public interface ILoadCommandService
    {
        /// <summary>
        /// Crea una carga nueva.
        /// </summary>
        Task<CreateLoadResult> CreateAsync(
            CreateLoadRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Actualiza una carga existente utilizando
        /// control de concurrencia mediante RowVersion.
        /// </summary>
        Task<UpdateLoadResult> UpdateAsync(
            UpdateLoadRequest request,
            CancellationToken cancellationToken = default);
    }
}
