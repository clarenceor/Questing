using Questing.Data.Model.MileStone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questing.Data.IDAL
{
    public interface IPlayerMilestoneDAL
    {
        List<Milestone> GetMilestoneCompletedByPlayerId(string PlayerId, int chipsAwardedMilestoneCompletion);

        int? GetMilestoneIndexLastCompletedByPlayerId(string PlayerId);
    }
}
