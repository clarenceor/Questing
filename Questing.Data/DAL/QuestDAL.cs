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
    public class QuestDAL : IQuestDAL
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabaseService _databaseService;

        public QuestDAL(IDatabaseService databaseService, IConfiguration configuration)
        {
            _configuration = configuration;
            _databaseService = databaseService;
        }

        public int? GetActiveQuestId()
        {
            string ConnectionString = _configuration.GetConnectionString("QuestingDatabaseConnection");
            string SQL = $"SELECT quest_id FROM Quest WHERE is_active = 1 ";

            List<SqlParameter> param = new List<SqlParameter>();

            var returnData = _databaseService.ExecuteScalar(ConnectionString, SQL, param, CommandType.Text);

            int? QuestId = returnData != null ? (int) returnData : null;

            return QuestId;
        }
    }
}
