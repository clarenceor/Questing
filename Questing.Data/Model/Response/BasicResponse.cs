using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questing.Data.Model.Response
{
    public class BasicResponse<T>
    {
        public int Code { get; set; } = 200;
        public T Content { get; set; }

        public BasicResponse(T data, int ResponseCode)
        {
            Content = data;
            Code = ResponseCode;
        }
    }
}
