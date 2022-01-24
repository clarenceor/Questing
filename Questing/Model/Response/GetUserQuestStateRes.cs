using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questing.Model.Response
{
    public class GetUserQuestStateRes
    {
        public decimal TotalQuestPercentCompleted { get; set; } = 0;
        public int? LastMilestoneIndexCompleted { get; set; } = null;
    }
}
