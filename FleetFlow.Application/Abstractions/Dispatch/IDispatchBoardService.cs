using FleetFlow.Application.Dispatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Application.Abstractions.Dispatch
{
    public interface IDispatchBoardService
    {
        Task<IReadOnlyList<DispatchBoardItem>> GetActiveTripsAsync(
            CancellationToken cancellationToken = default);
    }
}
