using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FleetFlow.Application.Abstractions.Persistence
{
    public interface IDbConnectionFactory
    {
        DbConnection CreateConnection();
    }
}
