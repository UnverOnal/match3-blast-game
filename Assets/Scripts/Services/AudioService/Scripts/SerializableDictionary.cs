using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.AudioService.Scripts
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new();

        [SerializeField] private List<TValue> values = new();

        // Indexer to access values in the dictionary using keys
        public TValue this[TKey key]
        {
            get => Dictionary[key];
            set => Dictionary[key] = value;
        }

        // Expose the underlying dictionary for direct access if needed
        public Dictionary<TKey, TValue> Dictionary { get; private set; } = new();

        // Called before the serialization process begins
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            // Store the keys and values of the dictionary in separate lists
            foreach (var kvp in Dictionary)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        // Called after the deserialization process is complete
        public void OnAfterDeserialize()
        {
            Dictionary = new Dictionary<TKey, TValue>();

            // Check if the key and value counts match
            if (keys.Count != values.Count)
            {
                Debug.LogError("Error deserializing SerializableDictionary. Key and value count mismatch.");
                return;
            }

            // Reconstruct the dictionary from the keys and values lists
            for (var i = 0; i < keys.Count; i++) Dictionary[keys[i]] = values[i];
        }
        
        // Method to remove a member from the dictionary
        public void Remove(TKey key)
        {
            if (Dictionary.ContainsKey(key))
            {
                Dictionary.Remove(key);
                // Remove the key and value from the serialization lists as well
                int index = keys.IndexOf(key);
                if (index >= 0 && index < values.Count)
                {
                    keys.RemoveAt(index);
                    values.RemoveAt(index);
                }
            }
        }
    }
}