using Microsoft.Extensions.Configuration;
using Questing.Data.IDAL;
using Questing.Data.SqlDatabaseService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questing.Data.DAL
{
    public class QuestPointTransactionDAL : IQuestPointTransactionDAL
    {
        IConfiguration _configuration;
        IDatabaseService _databaseService;
        IQuestDAL _questdal;

        public QuestPointTransactionDAL(IDatabaseService databaseService, IConfiguration configuration, IQuestDAL questDAL)
        {
            _configuration = configuration;
            _databaseService = databaseService;
            _questdal = questDAL;
        }

        public decimal? GetTotalActiveQuestPointByPlayerId(string PlayerId)
        {
            string ConnectionString = _configuration.GetConnectionString("QuestingDatabaseConnection");
            int? Questid = _questdal.GetActiveQuestId();

            if (Questid == null)
            {
                return null;
            }
            else
            {
                string SQL = $"SELECT SUM(quest_point_earned) quest_point_accumulate FROM QuestPointTransaction WHERE player_id = @player_id AND quest_id = @quest_id group by player_id, quest_id ";
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter
                {
                    ParameterName = "@player_id",
                    Value = PlayerId,
                    DbType = DbType.String
                });

                param.Add(new SqlParameter
                {
                    ParameterName = "@quest_id",
                    Value = Questid,
                    DbType = DbType.Int32
                });

                var returnData = _databaseService.ExecuteScalar(ConnectionString, SQL, param, CommandType.Text);

                decimal? quest_point_accumulate = returnData != null ? (decimal) returnData : null;

                return quest_point_accumulate;
            }
        }

        public bool SaveQuestPointTransaction(decimal QuestPointEarned, string PlayerId)
        {
            string ConnectionString = _configuration.GetConnectionString("QuestingDatabaseConnection");
            string SQL = $"EXEC sp_AddQuestPointTransaction @PlayerId, @QuestPointEarned";
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter
            {
                ParameterName = "@QuestPointEarned",
                Value = QuestPointEarned,
                DbType = DbType.Decimal
            });

            param.Add(new SqlParameter
            {
                ParameterName = "@PlayerId",
                Value = PlayerId,
                DbType = DbType.String
            });

            var returnData = _databaseService.ExecuteNonQuery(ConnectionString, SQL, param, CommandType.Text);

            if (returnData > 0)
                return true;
            else
                return false;
        }
    }
}
