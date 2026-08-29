using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Application.Authentication
{
    public sealed class LoginResult
    {
        private LoginResult(
            bool succeeded,
            UserSession? session,
            string? errorMessage)
        {
            Succeeded = succeeded;
            Session = session;
            ErrorMessage = errorMessage;
        }

        public bool Succeeded { get; }

        public UserSession? Session { get; }

        public string? ErrorMessage { get; }

        public static LoginResult Success(UserSession session)
        {
            return new LoginResult(
                succeeded: true,
                session: session,
                errorMessage: null);
        }

        public static LoginResult Failure(string errorMessage)
        {
            return new LoginResult(
                succeeded: false,
                session: null,
                errorMessage: errorMessage);
        }
    }
}
