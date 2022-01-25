using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questing.Data.Model.Request
{
    public class BasicRequest
    {
        public virtual bool IsValidRequestData()
        {
            return true;
        }
    }
}
