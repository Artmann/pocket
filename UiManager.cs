using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

namespace Pocket
{
    public class UiManager : MonoBehaviour
    {
        public float ticksPerSecond = 2f;

        public static UiManager Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject();
                    go.name = "Ui Manager";
                    instance = go.AddComponent<UiManager>();
                }

                return instance;
            }
        }

        List<UiComponent> components;
        Dictionary<string, List<Action<ExpandoObject>>> listeners;
        static UiManager instance;
        float timestamp;
        
        void Update()
        {
            if (components == null)
            {
                return;
            }

            var diff = Time.realtimeSinceStartup - timestamp;
            if (diff < (1f / ticksPerSecond))
            {
                return;
            }

            timestamp = Time.realtimeSinceStartup;
            foreach (var component in components)
            {
                component.Tick(Time.deltaTime);
            }
        }

        public void Register(UiComponent component)
        {
            if (components == null)
            {
                components = new List<UiComponent>();
            }
            Debug.Log("Register component");
            components.Add(component);
        }

        public void On(string type, Action<ExpandoObject> callback) {
            if (listeners == null)
            {
                listeners = new Dictionary<string, List<Action<ExpandoObject>>>();
            }

            if (!listeners.ContainsKey(type))
            {
                listeners.Add(type, new List<Action<ExpandoObject>>());
            }

            listeners[type].Add(callback);
        }

        public void Dispatch(string type, ExpandoObject data)
        {
            if (!listeners.ContainsKey(type))
            {
                return;
            }

            foreach (var callback in listeners[type])
            {
                callback.Invoke(data);
            }
        }
    }
}