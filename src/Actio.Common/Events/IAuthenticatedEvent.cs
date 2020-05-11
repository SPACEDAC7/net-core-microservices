using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Events
{
    interface IAuthenticatedEvent : IEvent
    {
        Guid UserId { get; }
    }
}
