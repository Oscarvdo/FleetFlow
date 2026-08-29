using FleetFlow.Application.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Application.Abstractions.Security
{
    public interface IAuthenticationService
    {
        Task<LoginResult> AuthenticateAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default);
    }
}
