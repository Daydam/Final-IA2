using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GOAP : MonoBehaviour
{
    //if object1 or object 2 == null, return cost, otherwise perform calculation
    public static Dictionary<string, Func<object, object, float, float>> heuristics;
    public static Func<object, object, float, float> GetHeuristics(string key)
    {
        if (!heuristics.ContainsKey(key)) heuristics.Add(key, (a, b, c) => { return 0; });
        return heuristics[key];
    }


    public static IEnumerable<GOAPAction> RunGOAP(GOAPEntity entity, GOAPState from, GOAPState to, IEnumerable<GOAPAction> actions, int watchdog = 200)
    {
        int watchdogCount = watchdog;

        var sequence = Algorithms.AStar<GOAPState>(
            from, 
            current => current.Equals(to), 
            //Change this! Heuristic should depend on the data type
            (current, goal) =>
                {
                    float totalCost = 0f;
                    foreach (KeyValuePair<string, object> valuePair in goal.Values)
                    {
                        if (heuristics.ContainsKey(valuePair.Key)) totalCost += GetHeuristics(valuePair.Key)(current.Values[valuePair.Key], goal.Values[valuePair.Key], entity.GetPriority(valuePair.Key));
                    }
                    return totalCost;
                },
            current =>
            {
                var arcs = new List<Arc<GOAPState>>();
                if (watchdogCount == 0) return arcs;
                else watchdogCount--;

                foreach (GOAPAction act in actions)
                {
                    if(act.Preconditions())
                }
            }
        return null;
    }
}