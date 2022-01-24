using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Questing.Data.SqlDatabaseService
{
    public class DatabaseService : IDatabaseService
    {
        private SqlConnection GetConnection(string ConnectionString)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }

        private DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType)
        {
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection)
            {
                CommandType = commandType
            };
            return command;
        }

        public SqlParameter GetParameter(string parameter, object value)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, value ?? DBNull.Value)
            {
                Direction = ParameterDirection.Input
            };
            return parameterObject;
        }

        //public SqlParameter GetParameterOut(string parameter, SqlDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        //{
        //    SqlParameter parameterObject = new SqlParameter(parameter, type);

        //    if (type == SqlDbType.NVarChar || type == SqlDbType.VarChar || type == SqlDbType.NText || type == SqlDbType.Text)
        //        parameterObject.Size = -1;

        //    parameterObject.Direction = parameterDirection;

        //    if (value != null)
        //        parameterObject.Value = value;
        //    else
        //        parameterObject.Value = DBNull.Value;

        //    return parameterObject;
        //}

        public int ExecuteNonQuery(string ConnectionString, string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int returnValue = -1;

            try
            {
                using (SqlConnection connection = GetConnection(ConnectionString))
                {
                    DbCommand cmd = GetCommand(connection, procedureName, commandType);

                    if (parameters != null && parameters.Count > 0)
                        cmd.Parameters.AddRange(parameters.ToArray());

                    returnValue = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //LogException("Failed to ExecuteNonQuery for " + procedureName, ex, parameters);
                throw;
            }

            return returnValue;
        }

        public object ExecuteScalar(string ConnectionString, string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            object returnValue = null;

            try
            {
                using (DbConnection connection = GetConnection(ConnectionString))
                {
                    DbCommand cmd = GetCommand(connection, procedureName, commandType);

                    if (parameters != null && parameters.Count > 0)
                        cmd.Parameters.AddRange(parameters.ToArray());

                    returnValue = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                //LogException("Failed to ExecuteScalar for " + procedureName, ex, parameters);
                throw;
            }

            return returnValue;
        }

        public DbDataReader GetDataReader(string ConnectionString, string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            DbDataReader ds;

            try
            {
                DbConnection connection = GetConnection(ConnectionString);
                {
                    DbCommand cmd = GetCommand(connection, procedureName, commandType);

                    if (parameters != null && parameters.Count > 0)
                        cmd.Parameters.AddRange(parameters.ToArray());

                    ds = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                //LogException("Failed to GetDataReader for " + procedureName, ex, parameters);
                throw;
            }

            return ds;
        }
    }
}
