using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text; 


namespace FleetFlow.Infrastructure.Abstractions.Persistence
{
    public interface IDbConnectionFactory
    {
        DbConnection CreateConnection();
    }
}
