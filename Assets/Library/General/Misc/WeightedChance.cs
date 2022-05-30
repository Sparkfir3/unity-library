using System;
using System.Collections.Generic;

/// <summary>
/// Given a list of WeightedChanceEntry<T>, returns a random entry value using each value's weight
/// </summary>
/// <typeparam name="T">Type of object to be returned</typeparam>
public class WeightedChance<T> {

    private float totalWeight;
    private readonly List<WeightedChanceEntry<T>> entries;

    // ------------------------------------------------------------------------------------

    #region Constructors

    public WeightedChance() {
        totalWeight = 0;
        entries = new List<WeightedChanceEntry<T>>();
    }

    public WeightedChance(List<WeightedChanceEntry<T>> entryList) {
        entries = entryList;
        foreach(WeightedChanceEntry<T> entry in entries)
            totalWeight += entry.weight;
        foreach(WeightedChanceEntry<T> entry in entries)
            entry.Percent = entry.weight / totalWeight;
    }

    #endregion

    // ------------------------------------------------------------------------------------

    #region Add & Get Entries

    public void Add(WeightedChanceEntry<T> newEntry) {
        entries.Add(newEntry);
        totalWeight += newEntry.weight;
        foreach(WeightedChanceEntry<T> entry in entries)
            entry.Percent = entry.weight / totalWeight;
    }

    public T GetRandomEntry() {
        Random random = new Random();
        return GetRandomEntry(random);
    }

    public T GetRandomEntry(Random random) {
        if(entries.Count == 0)
            throw new InvalidOperationException("Entry list must contain at least one value");

        float chance = (float)random.NextDouble();
        foreach(WeightedChanceEntry<T> entry in entries) {
            if(chance < entry.Percent)
                return entry.value;
            else
                chance -= entry.Percent;
        }

        throw new IndexOutOfRangeException();
    }

    #endregion

}

public class WeightedChanceEntry<T> {

    public float Percent { get; set; }
    public readonly float weight;
    public readonly T value;

    public WeightedChanceEntry(float w, T val) {
        if(w < 0)
            throw new InvalidOperationException($"Weight cannot be a negative number (weight given was {w})");

        weight = w;
        value = val;
    }

}

/* Example:
 * 
 * WeightedChance<Func<float>> weightedChance = new WeightedChance<Func<float>>(new List<WeightedChanceEntry<Func<float>>> {
 *     new WeightedChanceEntry<Func<float>>(1,    () => { return x; }),
 *     new WeightedChanceEntry<Func<float>>(1,    () => { return x + 2; }),
 *     new WeightedChanceEntry<Func<float>>(2,    () => { return x - 2; }),
 * });
 * return weightedChance.GetRandomEntry().Invoke();
 *  
 */