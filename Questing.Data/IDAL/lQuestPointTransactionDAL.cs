using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questing.Data.IDAL
{
    public interface IQuestPointTransactionDAL
    {
        decimal? GetTotalActiveQuestPointByPlayerId(string PlayerId);
        bool SaveQuestPointTransaction(decimal QuestPointEarned, string PlayerId);
    }
}
