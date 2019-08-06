using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GOAPState
{
    /*TBH, I didn't want to use object, but found no better choice for using generic data as a value*/

    /*I could've done a class-saving structure as seen in Memento, with a generic Memory class for storing different scripts's values,
     * but I felt like this would be better. This way, I can get a really customizable value list, like for example, the distance from an enemy to my character,
     * without having to store the enemy position, my character position, and lots of other stuff that I may not care about.
     * I can store literally anything I want, and I fucking love it.*/
    
    Dictionary<string, object> values;
    public Dictionary<string, object> Values { get { return values; } set { values = value; } }

    /*This will come in handy for some constantly changing states, such as positions and stuff. It turns the GOAPState into something more like a memento,
     * capable of REALLY keeping track of the world's state, managing details like time, distances, and lots of other stuff that would otherwise become ENGORROSO AS FUCK.*/
    Dictionary<string, Func<object>> updates;
    public Dictionary<string, Func<object>> Updates { get { return updates; } set { updates = value; } }

    public GOAPAction generatingAction = null;
    public int stepId = 0;

    public GOAPState()
    {
        values = new Dictionary<string, object>();
        updates = new Dictionary<string, Func<object>>();
    }

    public bool Equals(GOAPState obj)
    {
        var result =
            obj != null
            && obj.values.All(kv => values.Contains(kv));
        return result;
    }

    public GOAPState UpdateValues(Dictionary<string, object> baseState)
    {
        foreach (KeyValuePair<string, object> kv in baseState)
        {
            if (values.ContainsKey(kv.Key)) values[kv.Key] = kv.Value;
            else values.Add(kv.Key, kv.Value);
        }
        return this;
    }

    public GOAPState UpdateValues(Dictionary<string, Func<object, object>> baseState)
    {
        foreach (KeyValuePair<string, Func<object,object>> kv in baseState)
        {
            if (values.ContainsKey(kv.Key)) values[kv.Key] = kv.Value(values[kv.Key]);
            else values.Add(kv.Key, kv.Value(default(object)));
        }
        return this;
    }

    ///This is not being used for now
    public void UpdateData()
    {
        foreach (string key in updates.Keys)
        {
            values[key] = updates[key]();
        }
    }
}