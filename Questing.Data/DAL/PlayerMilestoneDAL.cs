using Microsoft.Extensions.Configuration;
using Questing.Data.IDAL;
using Questing.Data.Model.MileStone;
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
    public class PlayerMilestoneDAL : IPlayerMilestoneDAL
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabaseService _databaseService;
        private readonly IQuestDAL _questdal;
        private string ConnectionString;

        public PlayerMilestoneDAL(IDatabaseService databaseService, IConfiguration configuration, IQuestDAL questDAL)
        {
            _configuration = configuration;
            _databaseService = databaseService;
            _questdal = questDAL;
            ConnectionString = _configuration.GetConnectionString("QuestingDatabaseConnection");
        }

        public List<Milestone> GetMilestoneCompletedByPlayerId(string PlayerId, int chipsAwardedMilestoneCompletion)
        {
            int? Questid = _questdal.GetActiveQuestId();
            List<Milestone> Milestones = new List<Milestone>();

            if (Questid != null)
            {
                string SQL = $"SELECT milestone_index FROM PlayerMilestone WHERE player_id = @PlayerId AND quest_id = @Questid";

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

                var returnData = _databaseService.GetDataReader(ConnectionString, SQL, param, CommandType.Text);

                while (returnData.Read())
                {
                    Milestones.Add(new Milestone
                    {
                        ChipsAwarded = chipsAwardedMilestoneCompletion,
                        MilestoneIndex = (int) returnData["milestone_index"]
                    });
                }
            }

            return Milestones;
        }

        public int? GetMilestoneIndexLastCompletedByPlayerId(string PlayerId)
        {
            int? Questid = _questdal.GetActiveQuestId();

            if (Questid == null)
            {
                return null;
            }
            else
            {
                string SQL = $"SELECT MAX(milestone_index) last_index FROM PlayerMilestone WHERE player_id = @PlayerId AND quest_id = @Questid";

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

                int? MilestoneSeq = returnData != null ? (int)returnData : null;

                return MilestoneSeq;
            }
        }
    }
}
