using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPAction
{
    Dictionary<string, Func<object, bool>> preconditions;
    public Dictionary<string, Func<object, bool>> Preconditions { get { return preconditions; } }
    Dictionary<string, Func<object, object>> effects;
    public Dictionary<string, Func<object, object>> Effects { get => effects; }
    string actionName;
    public string Name { get { return actionName; } }
    float cost;
    public float Cost { get { return cost; } }

    public GOAPAction (string name)
    {
        this.actionName = name;
        cost = 1f;
        preconditions = new Dictionary<string, Func<object, bool>>();
        effects = new Dictionary<string, Func<object, object>>();
    }

    public GOAPAction SetCost (float cost)
    {
        this.cost = cost;
        return this;
    }

    public GOAPAction SetPreconditions(Dictionary<string, Func<object, bool>> preconditions)
    {
        foreach (KeyValuePair<string, Func<object, bool>> kv in preconditions)
        {
            if (!this.preconditions.ContainsKey(kv.Key)) this.preconditions.Add(kv.Key, kv.Value);
            else this.preconditions[kv.Key] = kv.Value;
        }
        return this;
    }

    public GOAPAction SetEffects(Dictionary<string, Func<object, object>> effects)
    {
        foreach (KeyValuePair<string, Func<object, object>> kv in effects)
        {
            if (!this.effects.ContainsKey(kv.Key)) this.effects.Add(kv.Key, kv.Value);
            else this.effects[kv.Key] = kv.Value;
        }
        return this;
    }
}
