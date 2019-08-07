using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GOAP
{
    //if object1 or object 2 == null, return cost, otherwise perform calculation
    public static Dictionary<string, Func<object, object, float, float>> heuristics;
    public static Func<object, object, float, float> GetHeuristics(string key)
    {
        if (!heuristics.ContainsKey(key)) heuristics.Add(key, (a, b, c) => { return 0; });
        return heuristics[key];
    }

    //So that heuristics can be set easily
    public void UpdateHeuristics(Dictionary<string, Func<object, object, float, float>> newHeuristics)
    {
        foreach (KeyValuePair<string, Func<object, object, float, float>> kv in newHeuristics)
        {
            if (heuristics.ContainsKey(kv.Key)) heuristics[kv.Key] = kv.Value;
            else heuristics.Add(kv.Key, kv.Value);
        }
    }


    public static IEnumerable<GOAPAction> RunGOAP(GOAPEntity entity, GOAPState from, GOAPState to, IEnumerable<GOAPAction> actions, int watchdog = 5000)
    {
        if (heuristics == null)
            heuristics = new Dictionary<string, Func<object, object, float, float>>()
            {
                { "Energy", (a, b, c) => ((1-(float)a/(float)b))*c },
                { "HouseBuilt", (a,b,c) => a == b? 0 : c }
            };

        int watchdogCount = watchdog;

        var sequence = Algorithms.AStar<GOAPState>(
            from,
            current => current.Satisfies(to),
            (current) =>
                {
                    current.Heuristics(to, );
                },
            current =>
                {
                    var arcs = new List<Arc<GOAPState>>();

                    if (watchdogCount == 0) return arcs;
                    else watchdogCount--;

                    foreach (GOAPAction act in actions)
                    {
                        if (act.Preconditions(current))
                        {
                            var st = new GOAPState();
                            st.generatingAction = act;
                            act.Effects(current, st);
                            st.stepId = current.stepId + 1;
                            arcs.Add(new Arc<GOAPState>().SetArc(st, act.Cost));
                        }
                    }
                    return arcs;
                }
            );

        if (sequence == null) return null;

        foreach (var step in sequence.Skip(1))
        {
            Debug.Log(step.generatingAction.Name);
        }
        Debug.Log("Watchdog: " + watchdogCount);

        return sequence.Skip(1).Select(x => x.generatingAction);
    }
}