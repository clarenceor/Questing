using Microsoft.Extensions.Configuration;
using Questing.Data.IDAL;
using Questing.IServices;
using Questing.Data.Model.MileStone;
using Questing.Data.Model.Response;
using Questing.Data.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questing.Services
{
    public class UserQuestService : IUserQuestService
    {
        private readonly IQuestPointTransactionDAL _questTrans;
        private readonly IPlayerMilestoneDAL _playerMilestone;
        private readonly IConfiguration _configuration;

        public UserQuestService(IQuestPointTransactionDAL questTrans, IPlayerMilestoneDAL playerMilestone, IConfiguration configuration)
        {
            _questTrans = questTrans;
            _configuration = configuration;
            _playerMilestone = playerMilestone;
        }

        public GetUserQuestProgressRes GetUserQuestProgress(GetUserQuestProgressReq req)
        {
            int numOfMilestone = (int)_configuration.GetValue(typeof(int), "NumOfMilestone");
            decimal rateFromBet = (decimal)_configuration.GetValue(typeof(decimal), "RateFromBet");
            decimal levelBonusRate = (decimal)_configuration.GetValue(typeof(decimal), "LevelBonusRate");
            decimal questPointPerMileStone = (decimal)_configuration.GetValue(typeof(decimal), "QuestPointCompleteMilestone");
            
            decimal QuestEarned = req.ChipAmountBet * rateFromBet + req.PlayerLevel * levelBonusRate;
            decimal? QuestPointAccumulated = 0m;

            bool savedSuccess = _questTrans.SaveQuestPointTransaction(QuestEarned, req.PlayerId, questPointPerMileStone, numOfMilestone);

            decimal? QuestCompletionPercentage = GetTotalQuestPercentCompleted(req.PlayerId, out QuestPointAccumulated);

            if (QuestPointAccumulated != null)
            {
                GetUserQuestProgressRes ProgressRes = new GetUserQuestProgressRes
                {
                    QuestPointsEarned = QuestEarned,
                    TotalQuestPercentCompleted = QuestCompletionPercentage.Value,
                    MilestonesCompleted = GetMilestoneCompleted(req.PlayerId)
                };

                return ProgressRes;
            }
            else
            {
                return null;
            }
        }

        public GetUserQuestStateRes GetUserQuestState(string PlayerId)
        {
            decimal? QuestPointAccumulated = 0m;
            decimal? QuestCompletionPercentage = GetTotalQuestPercentCompleted(PlayerId, out QuestPointAccumulated);

            if (QuestPointAccumulated != null)
            {
                GetUserQuestStateRes StateRes = new GetUserQuestStateRes
                {
                    TotalQuestPercentCompleted = QuestCompletionPercentage.Value,
                    LastMilestoneIndexCompleted = GetLastMilestoneIndexCompleted(PlayerId)
                };

                return StateRes;
            }
            else
            {
                return null;
            }
        }

        private List<Milestone> GetMilestoneCompleted(string PlayerId)
        {
            int chipsAwardedMilestoneCompletion = (int)_configuration.GetValue(typeof(int), "MilestoneChipsAward");

            List<Milestone> milestones = _playerMilestone.GetMilestoneCompletedByPlayerId(PlayerId, chipsAwardedMilestoneCompletion);

            return milestones;
        }

        private int? GetLastMilestoneIndexCompleted(string PlayerId)
        {
            int? MilestoneIndex = _playerMilestone.GetMilestoneIndexLastCompletedByPlayerId(PlayerId);

            return MilestoneIndex;
        }

        private decimal? GetTotalQuestPercentCompleted(string PlayerId, out decimal? Accumulated)
        {
            decimal TotalQuestPointRequired = (decimal)_configuration.GetValue(typeof(decimal), "QuestPointCompleteQuest");

            decimal? QuestPointAccumulated = Accumulated = _questTrans.GetTotalActiveQuestPointByPlayerId(PlayerId);
            decimal? QuestCompletionPercent = QuestPointAccumulated * 100 / TotalQuestPointRequired;

            return QuestCompletionPercent > 100m ? 100m : QuestCompletionPercent;
        }
    }
}
