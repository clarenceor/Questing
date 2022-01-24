using Questing.Model.MileStone;
using Questing.Model.Response;
using Questing.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questing.IServices
{
    public interface IUserQuestService
    {
        GetUserQuestProgressRes GetUserQuestProgress(GetUserQuestProgressReq req);

        GetUserQuestStateRes GetUserQuestState(string PlayerId);
    }
}
