using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sparkfire.Utility
{
    /// <summary>
    /// Given a list of WeightedChanceEntry, returns a random entry value using each value's weight
    /// </summary>
    /// <typeparam name="T">Type of object to be returned</typeparam>
    [Serializable]
    public class WeightedChance<T>
    {
        [Serializable]
        public class WeightedChanceEntry
        {
#if ODIN_INSPECTOR
        [field: Sirenix.OdinInspector.ReadOnly, Sirenix.OdinInspector.TableColumnWidth(-100)]
#endif
            [field: SerializeField]
            public float percent;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.TableColumnWidth(-100)]
#endif
            public float weight;
            public T value;

            public WeightedChanceEntry(float w, T val)
            {
                if(w < 0)
                    throw new InvalidOperationException($"Weight cannot be a negative number (weight given was {w})");

                weight = w;
                value = val;
            }
        }

        // ------------------------------

        #region Variables

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [SerializeField]
        private float totalWeight = 0;
        [SerializeField]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.TableList]
#endif
        private List<WeightedChanceEntry> entries = new();

        private bool hasInitializedPercents;

        #endregion

        // ------------------------------

        #region Initialize

        private void CalculateTotalWeight()
        {
            totalWeight = entries.Sum(x => x.weight);
        }

        private void CalculatePercents()
        {
            foreach(WeightedChanceEntry entry in entries)
                entry.percent = totalWeight > 0f ? entry.weight / totalWeight : 0f;
            hasInitializedPercents = true;
        }

        /// <summary>
        /// Validates and initializes chance percents.
        /// Intended to be called from OnValidate
        /// </summary>
        public void Validate()
        {
            CalculateTotalWeight();
            CalculatePercents();
        }

        #endregion

        // ------------------------------

        #region Info

        public bool ContainsItem(T item)
        {
            return entries.Any(x => x.value.Equals(item));
        }

        public float GetWeight(T item)
        {
            foreach(WeightedChanceEntry entry in entries)
                if(entry.value.Equals(item))
                    return entry.weight;
            return 0;
        }

        public float GetPercent(T item)
        {
            foreach(WeightedChanceEntry entry in entries)
                if(entry.value.Equals(item))
                    return entry.percent;
            return 0;
        }

        #endregion

        // ------------------------------

        #region Add & Update Entries

        public void Add(T item, float weight)
        {
            WeightedChanceEntry newEntry = new WeightedChanceEntry(weight, item);
            entries.Add(newEntry);
            totalWeight += newEntry.weight;
            CalculatePercents();
        }

        /// <summary>
        /// Updates given entry to a new weight
        /// </summary>
        /// <param name="item">Weighted entry value to be updated. Must already exist in the weighted chance</param>
        /// <param name="weight">New weight to be used</param>
        /// <param name="additive">Whether the new weight is applied additively or overwrites the old value</param>
        /// <param name="zeroClamp">If additive, whether or not the new weight should be clamped at zero. 
        ///         If false, will throw an error if the new weight is negative </param>
        /// <exception cref="InvalidOperationException">If new weight would be negative</exception>
        public bool Update(T item, float weight, bool additive = false, bool zeroClamp = true)
        {
            foreach(WeightedChanceEntry entry in entries)
            {
                if(entry.value.Equals(item))
                {
                    if(additive)
                    {
                        if(!zeroClamp && entry.weight + weight < 0)
                            throw new InvalidOperationException($"Weight cannot be a negative number (new weight calculated was {entry.weight + weight})");
                        entry.weight += weight;
                        if(entry.weight < 0)
                            entry.weight = 0;
                    }
                    else
                    {
                        if(weight < 0)
                            throw new InvalidOperationException($"Weight cannot be a negative number (new weight given was {weight})");
                        entry.weight = weight;
                    }

                    Validate();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Updates given entry with new weight, or adds it if it does not exist
        /// </summary>
        /// <param name="item">Weighted entry value to be updated or add.</param>
        /// <param name="weight">New weight to be used</param>
        /// <param name="additive">Whether the new weight is applied additively or overwrites the old value</param>
        /// <param name="zeroClamp">If additive, whether or not the new weight should be clamped at zero. 
        ///         If false, will throw an error if the new weight is negative </param>
        public void UpdateOrAdd(T item, float weight, bool additive = false, bool zeroClamp = true)
        {
            if(ContainsItem(item))
                Update(item, weight, additive, zeroClamp);
            else
                Add(item, weight);
        }

        #endregion

        // ------------------------------

        #region Get Entries

        public T GetRandomEntry()
        {
            System.Random random = new System.Random();
            return GetRandomEntry(random);
        }

        public T GetRandomEntry(System.Random random)
        {
            if(!hasInitializedPercents)
                Validate();

            if(entries.Count == 0)
                throw new InvalidOperationException("Entry list must contain at least one value.");
            if(totalWeight <= 0)
                throw new InvalidOperationException($"Total weight of all entries {totalWeight} must be greater than zero.");

            float chance = (float)random.NextDouble();
            foreach(WeightedChanceEntry entry in entries)
            {
                if(chance <= entry.percent)
                    return entry.value;
                else
                    chance -= entry.percent;
            }
            return entries[^1].value;
        }

        #endregion
    }
}
