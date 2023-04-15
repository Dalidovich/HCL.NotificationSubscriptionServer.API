using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCL.NotificationSubscriptionServer.API.Domain.InnerResponse
{
    public abstract class BaseResponse<T>
    {
        public virtual T Data { get; set; }
        public virtual StatusCode StatusCode { get; set; }
        public virtual string Message { get; set; }
    }
}
