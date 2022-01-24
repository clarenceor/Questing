using Microsoft.Extensions.Configuration;
using Questing.Data.IDAL;
using Questing.IServices;
using Questing.Model.MileStone;
using Questing.Model.Response;
using Questing.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questing.Services
{
    public class UserQuestService : IUserQuestService
    {
        private readonly IQuestPointTransactionDAL _questTrans;
        private readonly IConfiguration _configuration;

        public UserQuestService(IQuestPointTransactionDAL questTrans, IConfiguration configuration)
        {
            _questTrans = questTrans;
            _configuration = configuration;
        }

        public GetUserQuestProgressRes GetUserQuestProgress(GetUserQuestProgressReq req)
        {
            decimal rateFromBet = (decimal)_configuration.GetValue(typeof(decimal), "RateFromBet");
            decimal levelBonusRate = (decimal)_configuration.GetValue(typeof(decimal), "LevelBonusRate");
          
            decimal QuestEarned = req.ChipAmountBet * rateFromBet + req.PlayerLevel * levelBonusRate;
            decimal? QuestPointAccumulated = 0m;

            bool savedSuccess = _questTrans.SaveQuestPointTransaction(QuestEarned, req.PlayerId);

            decimal? QuestCompletionPercentage = GetTotalQuestPercentCompleted(req.PlayerId, out QuestPointAccumulated);

            if (QuestPointAccumulated != null)
            {
                GetUserQuestProgressRes ProgressRes = new GetUserQuestProgressRes
                {
                    QuestPointsEarned = QuestEarned,
                    TotalQuestPercentCompleted = QuestCompletionPercentage.Value,
                    MilestonesCompleted = GetMilestoneCompleted(QuestPointAccumulated.Value)
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
                    LastMilestoneIndexCompleted = GetLastMilestoneIndexCompleted(QuestPointAccumulated.Value)
                };

                return StateRes;
            }
            else
            {
                return null;
            }
        }

        private List<Milestone> GetMilestoneCompleted(decimal QuestPointAccumulated)
        {
            int chipsAwardedMilestoneCompletion = (int)_configuration.GetValue(typeof(int), "MilestoneChipsAward");
            decimal rateFromBet = (decimal)_configuration.GetValue(typeof(decimal), "QuestPointCompleteMilestone");
            
            List<Milestone> milestones = new List<Milestone>();
            decimal questPointCompute = QuestPointAccumulated;
            int mileStoneCompletionIndex = 0;
            
            while ((questPointCompute -= rateFromBet) > 0)
            {
                milestones.Add(new Milestone
                {
                    ChipsAwarded = chipsAwardedMilestoneCompletion,
                    MilestoneIndex = mileStoneCompletionIndex++
                });
            }

            return milestones;
        }

        private int? GetLastMilestoneIndexCompleted(decimal QuestPointAccumulated)
        {
            decimal QuestPointMilestoneRequired = (decimal)_configuration.GetValue(typeof(decimal), "QuestPointCompleteMilestone");
            int MilestoneIndex = ((int) Math.Floor(QuestPointAccumulated / QuestPointMilestoneRequired)) - 1;

            return MilestoneIndex < 0 ? null : MilestoneIndex;
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
