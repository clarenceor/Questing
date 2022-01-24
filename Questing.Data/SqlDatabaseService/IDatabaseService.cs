using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questing.Data.SqlDatabaseService
{
    public interface IDatabaseService
    {
        int ExecuteNonQuery(string ConnectionString, string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure);
        object ExecuteScalar(string ConnectionString, string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure);
        DbDataReader GetDataReader(string ConnectionString, string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure);
    }
}
