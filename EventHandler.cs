using System;

namespace Pocket
{
    public class EventHandler
    {
        public string id;
        public string eventName;
        public Action<Dom.Event> action;

        public EventHandler(string id, string eventName, Action<Dom.Event> action)
        {
            this.id = id;
            this.eventName = eventName;
            this.action = action;
        }
    }
}

