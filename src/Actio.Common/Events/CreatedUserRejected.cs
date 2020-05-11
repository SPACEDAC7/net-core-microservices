using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Events
{
    class CreatedUserRejected: IRejectedEvent
    {
        public string Reason { get; }

        public string Code { get; }

        public string Email { get; }

        protected CreatedUserRejected()
        {
        }

        public CreatedUserRejected(string reason, string code, string email)
        {
            Reason = reason;
            Code = code;
            Email = email;
        }
    }
}
