using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Infrastructures
{
    public class ResponseData
    {
        public Object Data { set; get; }
        public string Messages { set; get; }
        public ResponseData(Object data, string mess = "")
        {
            this.Data = data;
            this.Messages = mess;
        }
    }
}
