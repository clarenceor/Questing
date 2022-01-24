using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questing.Response
{
    public class BasicResponse<T>
    {
        public int Code { get; set; }
        public T Content { get; set; }

        public BasicResponse(T data, int ResponseCode)
        {
            Content = data;
            Code = ResponseCode;
        }
    }
}
