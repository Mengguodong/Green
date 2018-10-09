using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ResultMessage<T> 
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ResultMessage<T> Get(int code, string msg, T t = default(T))
        {
            ResultMessage<T> resultMessage = new ResultMessage<T>
            {
                Code = code,
                Message = msg ,
                Data = t
            };
            return resultMessage;
        }
    }
}
