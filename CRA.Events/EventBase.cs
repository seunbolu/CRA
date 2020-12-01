using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Events
{
    public abstract class EventBase
    {

        public List<SubscriberBase> Subscribers { get; set; }

        public EventBase()
        {
            Subscribers = new List<SubscriberBase>();
        }

        public void Handle()
        {
            AggregateException aggregateException = null;
            List<Exception> exceptions = new List<Exception>();

            if (Subscribers != null)
            {
                foreach (var subscriber in Subscribers)
                {
                    try
                    {
                        subscriber.Process();
                    }
                    catch (Exception exception)
                    {
                        exceptions.Add(exception);
                    }
                }

                if (exceptions.Count() > 0)
                {
                    aggregateException = new AggregateException(exceptions);
                    throw aggregateException;
                }
            }
        }
    }
}
