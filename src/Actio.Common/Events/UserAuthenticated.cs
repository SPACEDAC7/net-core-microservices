using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Events
{
    class UserAuthenticated: IEvent
    {

        public string Email { get; }

        public UserAuthenticated()
        {
        }

        public UserAuthenticated(string email)
        {
            Email = email;
        }
    }
}
