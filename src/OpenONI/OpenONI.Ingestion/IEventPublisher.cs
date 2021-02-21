using System;
using System.Collections.Generic;
using System.Text;

namespace OpenONI.Ingestion
{
    public interface IEventPublisher
    {
        void PublishEvent(object eventArgs);
    }
}
