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
        private readonly IConfiguration _configuration;
        private readonly IDatabaseService _databaseService;
        private readonly IQuestDAL _questdal;
        private string ConnectionString;

        public QuestPointTransactionDAL(IDatabaseService databaseService, IConfiguration configuration, IQuestDAL questDAL)
        {
            _configuration = configuration;
            _databaseService = databaseService;
            _questdal = questDAL;
            ConnectionString = _configuration.GetConnectionString("QuestingDatabaseConnection");
        }

        public decimal? GetTotalActiveQuestPointByPlayerId(string PlayerId)
        {
            int? Questid = _questdal.GetActiveQuestId();

            if (Questid == null)
            {
                return null;
            }
            else
            {
                string SQL = $"SELECT SUM(quest_point_earned) quest_point_accumulate FROM QuestPointTransaction WHERE player_id = @PlayerId AND quest_id = @Questid group by player_id, quest_id ";
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter
                {
                    ParameterName = "@PlayerId",
                    Value = PlayerId,
                    DbType = DbType.String
                });

                param.Add(new SqlParameter
                {
                    ParameterName = "@Questid",
                    Value = Questid,
                    DbType = DbType.Int32
                });

                var returnData = _databaseService.ExecuteScalar(ConnectionString, SQL, param, CommandType.Text);

                decimal? quest_point_accumulate = returnData != null ? (decimal) returnData : null;

                return quest_point_accumulate;
            }
        }

        public bool SaveQuestPointTransaction(decimal QuestPointEarned, string PlayerId, decimal QuestPointPerMilestone, int numOfMilestone)
        {
            string ConnectionString = _configuration.GetConnectionString("QuestingDatabaseConnection");
            string SQL = $"EXEC sp_AddQuestPointTransaction @PlayerId, @QuestPointEarned, @QuestPointPerMilestone, @MaxMilestonePerQuest";
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter
            {
                ParameterName = "@PlayerId",
                Value = PlayerId,
                DbType = DbType.String
            });

            param.Add(new SqlParameter
            {
                ParameterName = "@QuestPointEarned",
                Value = QuestPointEarned,
                DbType = DbType.Decimal
            });

            param.Add(new SqlParameter
            {
                ParameterName = "@QuestPointPerMilestone",
                Value = QuestPointPerMilestone,
                DbType = DbType.Decimal
            });

            param.Add(new SqlParameter
            {
                ParameterName = "@MaxMilestonePerQuest",
                Value = numOfMilestone,
                DbType = DbType.Int32
            });

            var returnData = _databaseService.ExecuteNonQuery(ConnectionString, SQL, param, CommandType.Text);

            if (returnData > 0)
                return true;
            else
                return false;
        }
    }
}
