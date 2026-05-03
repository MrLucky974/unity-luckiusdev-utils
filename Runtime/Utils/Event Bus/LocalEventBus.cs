using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuckiusDev.Utils.EventBus
{
    /// <summary>
    /// A MonoBehaviour component that provides a scoped event bus local to a GameObject and its hierarchy.
    /// Add this component to a GameObject to create an isolated event scope.
    /// Use <see cref="LocalEventBus.For(GameObject)"/> or <see cref="LocalEventBus.For(Component)"/>
    /// to locate the nearest bus by walking up the transform hierarchy.
    /// </summary>

    [DisallowMultipleComponent]
    public sealed class LocalEventBus : MonoBehaviour
    {
        private readonly Dictionary<Type, object> bindingSets = new();
        
        private void OnDestroy()
        {
            ClearAll();
        }
        
        /// <summary>
        /// Walks up the transform hierarchy of <paramref name="go"/> looking for the nearest
        /// <see cref="LocalEventBus"/> component, including on <paramref name="go"/> itself.
        /// Returns <c>null</c> if none is found.
        /// </summary>
        public static LocalEventBus For(GameObject go)
        {
            if (go == null) return null;
 
            Transform current = go.transform;
            while (current != null)
            {
                if (current.TryGetComponent(out LocalEventBus bus))
                    return bus;
 
                current = current.parent;
            }
 
            return null;
        }
 
        /// <summary>
        /// Walks up the transform hierarchy of <paramref name="component"/>'s GameObject looking
        /// for the nearest <see cref="LocalEventBus"/> component.
        /// Returns <c>null</c> if none is found.
        /// </summary>
        public static LocalEventBus For(Component component)
        {
            if (component == null) return null;
            return For(component.gameObject);
        }

        /// <summary>
        /// Registers an <see cref="EventBinding{T}"/> on this local bus.
        /// </summary>
        public void Register<T>(EventBinding<T> binding) where T : IEvent
        {
            GetOrCreateBindings<T>().Add(binding);
        }
 
        /// <summary>
        /// Deregisters an <see cref="EventBinding{T}"/> from this local bus.
        /// </summary>
        public void Deregister<T>(EventBinding<T> binding) where T : IEvent
        {
            if (TryGetBindings<T>(out var set))
                set.Remove(binding);
        }
        
        /// <summary>
        /// Raises an event on this local bus, invoking all registered bindings for <typeparamref name="T"/>.
        /// </summary>
        public void Raise<T>(T @event) where T : IEvent
        {
            if (!TryGetBindings<T>(out var set)) return;
 
            foreach (var binding in set)
            {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }
        
        /// <summary>
        /// Clears all bindings for a specific event type on this local bus.
        /// </summary>
        public void Clear<T>() where T : IEvent
        {
            if (TryGetBindings<T>(out var set))
            {
                Debug.Log($"[LocalEventBus] Clearing {typeof(T).Name} bindings on {gameObject.name}");
                set.Clear();
            }
        }
 
        /// <summary>
        /// Clears all bindings for every event type on this local bus.
        /// </summary>
        public void ClearAll()
        {
            Debug.Log($"[LocalEventBus] Clearing all bindings on {gameObject.name}", gameObject);
            bindingSets.Clear();
        }
        
        private HashSet<IEventBinding<T>> GetOrCreateBindings<T>() where T : IEvent
        {
            var key = typeof(T);
            if (!bindingSets.TryGetValue(key, out var raw))
            {
                raw = new HashSet<IEventBinding<T>>();
                bindingSets[key] = raw;
            }
 
            return (HashSet<IEventBinding<T>>)raw;
        }
 
        private bool TryGetBindings<T>(out HashSet<IEventBinding<T>> set) where T : IEvent
        {
            if (bindingSets.TryGetValue(typeof(T), out var raw))
            {
                set = (HashSet<IEventBinding<T>>)raw;
                return true;
            }
 
            set = null;
            return false;
        }
    }
}