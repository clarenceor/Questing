using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questing.Data.Model.Response
{
    public class GetUserQuestStateRes
    {
        public decimal TotalQuestPercentCompleted { get; set; } = 0;
        public int? LastMilestoneIndexCompleted { get; set; } = null;

        public bool Equals(GetUserQuestStateRes Actual)
        {
            bool QuestPercentMatch = this.TotalQuestPercentCompleted == Actual.TotalQuestPercentCompleted;
            bool MileStoneIndexMatch = this.LastMilestoneIndexCompleted == Actual.LastMilestoneIndexCompleted;

            return MileStoneIndexMatch && QuestPercentMatch;
        }
    }
}
