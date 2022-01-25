using Questing.Data.Model.MileStone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questing.Data.Model.Response
{
    public class GetUserQuestProgressRes
    {
        public decimal QuestPointsEarned { get; set; } = 0;
        public decimal TotalQuestPercentCompleted { get; set; } = 0;
        public List<Milestone> MilestonesCompleted { get; set; } = new List<Milestone>();

        public bool Equals(GetUserQuestProgressRes Actual)
        {
            bool QuestPointMatch = this.QuestPointsEarned == Actual.QuestPointsEarned;
            bool QuestPercentMatch = Math.Round(this.TotalQuestPercentCompleted, 2) == Math.Round(Actual.TotalQuestPercentCompleted, 2);
            bool MileStoneMatch = this.MilestonesCompleted.Where(x => !Actual.MilestonesCompleted.Any(y => y.MilestoneIndex == x.MilestoneIndex && y.ChipsAwarded == x.ChipsAwarded)).Count() == 0;

            return QuestPointMatch && QuestPercentMatch && MileStoneMatch;
        }
    }
}
