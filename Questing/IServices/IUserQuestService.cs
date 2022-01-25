using Questing.Data.Model.MileStone;
using Questing.Data.Model.Response;
using Questing.Data.Model.Request;
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
