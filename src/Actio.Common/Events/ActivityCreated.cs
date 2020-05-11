using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Events
{
    class ActivityCreated : IAuthenticatedEvent
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string Password { get; }

        protected ActivityCreated()
        {
        }

        public ActivityCreated(Guid userId, string email, string password)
        {
            UserId = userId;
            Email = email;
            Password = password;
        }
    }
}
