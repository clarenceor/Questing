using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Questing.IServices;
using Questing.Data.Model.Response;
using Questing.Data.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Questing.Controllers
{
    [ApiController]
    public class QuestingController : ControllerBase
    {
        private readonly ILogger<QuestingController> _logger;
        private readonly IUserQuestService _userQuest;

        public QuestingController(ILogger<QuestingController> logger, IUserQuestService userQuest)
        {
            _logger = logger;
            _userQuest = userQuest;
        }

        [HttpPost]
        [Route("api/progress")]
        public ActionResult<BasicResponse<GetUserQuestProgressRes>> Progress(GetUserQuestProgressReq req)
        {
            try
            {
                if (req.IsValidRequestData())
                {
                    var data = _userQuest.GetUserQuestProgress(req);

                    if (data != null)
                        return new BasicResponse<GetUserQuestProgressRes>(data, (int) HttpStatusCode.OK);
                    else 
                        return new BasicResponse<GetUserQuestProgressRes>(new GetUserQuestProgressRes(), (int) HttpStatusCode.NotFound);
                }
                else
                {
                    return new BasicResponse<GetUserQuestProgressRes>(new GetUserQuestProgressRes(), (int) HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new BasicResponse<GetUserQuestProgressRes>(new GetUserQuestProgressRes(), (int) HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/state")]
        public ActionResult<BasicResponse<GetUserQuestStateRes>> State([FromQuery(Name = "PlayerId")] string PlayerId)
        {
            try
            {
                if (!string.IsNullOrEmpty(PlayerId))
                {
                    var data = _userQuest.GetUserQuestState(PlayerId);

                    if (data != null)
                        return new BasicResponse<GetUserQuestStateRes>(data, (int) HttpStatusCode.OK);
                    else
                        return new BasicResponse<GetUserQuestStateRes>(new GetUserQuestStateRes(), (int) HttpStatusCode.NotFound);
                }
                else
                {
                    return new BasicResponse<GetUserQuestStateRes>(new GetUserQuestStateRes(), (int) HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new BasicResponse<GetUserQuestStateRes>(new GetUserQuestStateRes(), (int) HttpStatusCode.InternalServerError);
            }
        }
    }
}
