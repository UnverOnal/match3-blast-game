using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.AudioService.Scripts
{
    /// <summary>
    ///     Object pooling class for Unity components.
    /// </summary>
    /// <typeparam name="T">Type of component to pool.</typeparam>
    public class ComponentPool<T> where T : Component
    {
        public List<T> Pool { get; } = new();

        private readonly GameObject _gameObject;

        private readonly Func<T, bool> _gettingCondition;
        private readonly Action<T> _returningCondition;
        private readonly Action<T> _creationCondition;

        /// <summary>
        ///     Initializes a new instance of the ComponentPool class.
        /// </summary>
        /// <param name="gameObject">GameObject to attach pooled components to.</param>
        /// <param name="gettingCondition">Condition to check if a component can be retrieved from the pool.</param>
        /// <param name="returningCondition">Action to execute when a component is returned to the pool.</param>
        /// <param name="creationCondition">Action to execute when a new component is created.</param>
        /// <param name="initialSize">Initial size of the pool.</param>
        public ComponentPool(GameObject gameObject, Func<T, bool> gettingCondition = null,
            Action<T> returningCondition = null, Action<T> creationCondition = null, int initialSize = 0)
        {
            _gameObject = gameObject;
            _gettingCondition = gettingCondition;
            _returningCondition = returningCondition;
            _creationCondition = creationCondition;

            for (var i = 0; i < initialSize; i++)
                CreateComponent();
        }

        /// <summary>
        ///     Retrieves a component from the pool.
        /// </summary>
        /// <returns>The retrieved component.</returns>
        public T GetComponent()
        {
            T component;

            if (Pool.Count == 0)
            {
                component = CreateComponent();
                return component;
            }

            foreach (var c in Pool)
                if (_gettingCondition.Invoke(c))
                    return c;

            component = CreateComponent();
            return component;
        }

        /// <summary>
        ///     Returns a component to the pool.
        /// </summary>
        /// <param name="component">The component to return.</param>
        public void ReturnComponent(T component)
        {
            _returningCondition?.Invoke(component);
        }

        private T CreateComponent()
        {
            var component = _gameObject.AddComponent<T>();
            _creationCondition?.Invoke(component);
            Pool.Add(component);
            return component;
        }
    }
}