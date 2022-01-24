using Questing.Model.MileStone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questing.Model.Response
{
    public class GetUserQuestProgressRes
    {
        public decimal QuestPointsEarned { get; set; } = 0;
        public decimal TotalQuestPercentCompleted { get; set; } = 0;
        public List<Milestone> MilestonesCompleted { get; set; } = new List<Milestone>();
    }
}
