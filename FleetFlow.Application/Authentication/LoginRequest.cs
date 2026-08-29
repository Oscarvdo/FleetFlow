using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Application.Authentication
{
    public sealed record LoginRequest(
     string Username,
     string Password);
}
