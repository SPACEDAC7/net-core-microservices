using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Events
{
    interface IRejectedEvent : IEvent
    {
        string Reason { get; }
        string Code { get; }
    }
}
