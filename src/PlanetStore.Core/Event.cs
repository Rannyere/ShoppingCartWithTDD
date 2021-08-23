using System;
using MediatR;
using PlanetStore.Core.Messages;

namespace PlanetStore.Core
{
    public abstract class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
