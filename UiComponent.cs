using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using PowerUI;
using Mustachio;
using System.Dynamic;

namespace Pocket
{
    public abstract class UiComponent : HtmlElement
    {
        protected dynamic state;

        List<EventHandler> eventHandlers;
        string source;

        internal override void AddedToDOM()
        {
            base.AddedToDOM();

            eventHandlers = new List<EventHandler>();
            state = new ExpandoObject();

            LoadTemplate();
            Render();

            DidMount();

            Render();

            UiManager.Instance.Register(this);
        }

        public void SetState(string key, object value)
        {
            var tmp = this.state as IDictionary<string, object>;
            tmp[key] = value;
            this.state = tmp as ExpandoObject;

            Render();
        }

        public void Tick(float deltaTime)
        {
            OnTick(deltaTime);
        }

        protected void AddEventHandler(string id, string eventName, System.Action<Dom.Event> action)
        {
            eventHandlers.Add(new EventHandler(id, eventName, action));
        }

        void LoadTemplate()
        {
            var name = GetType().Name + ".html";
            var path = "Assets/Resources/UI/Templates/" + name;

            try
            {
                StreamReader reader = new StreamReader(path);
                source = reader.ReadToEnd();
                reader.Close();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        void RegisterEventHandlers()
        {
            foreach (var handler in eventHandlers)
            {
                var element = getElementById(handler.id);
                element.addEventListener(handler.eventName, handler.action);
            }
        }

        void Render()
        {
            var template = Parser.Parse(source);
            var content = template(state);

            innerHTML = content;

            RegisterEventHandlers();
        }

        protected virtual void DidMount() { }
        protected virtual void OnTick(float deltaTime) { }
    }
}