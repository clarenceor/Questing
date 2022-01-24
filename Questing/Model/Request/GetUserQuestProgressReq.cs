using Questing.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questing.Request
{
    public class GetUserQuestProgressReq : BasicRequest
    {
        public string PlayerId { get; set; }
        public int PlayerLevel { get; set; }
        public int ChipAmountBet { get; set; }

        public override bool IsValidRequestData()
        {
            if (string.IsNullOrEmpty(PlayerId))
                return false;
            if (ChipAmountBet == 0 && PlayerLevel == 0)
                return false;

            return true;
        }
    }
}
