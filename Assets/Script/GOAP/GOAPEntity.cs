using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GOAPEntity : MonoBehaviour
{
    Dictionary<string, float> priorities;
    public float GetPriority(string key)
    {
        if (!priorities.ContainsKey(key)) priorities.Add(key, 0);
        return priorities[key];
    }

    List<GOAPAction> actions;

    GOAPState current;

    private void Start()
    {
        priorities = new Dictionary<string, float>()
        {
            { "HouseBuilt", 1f },
            { "Energy", 0.5f }
        };

        actions = new List<GOAPAction>()
        {
            new GOAPAction("BuildHouse").SetPreconditions
                (
                //We need to see how to specify if we don't need a state for a boolean... this is gonna be bad.
                    (a) =>
                    {
                        return
                            a.HasHammer &&
                            a.Energy >= 65f &&
                            a.Wood >= 10 &&
                            !a.HouseBuilt;
                    }
                ).SetEffects
                (
                    (a, b) =>
                    {
                        b.Energy = a.Energy -65f;
                        b.Wood = a.Wood -10;
                        b.HouseBuilt = true;
                    }
                ),
            new GOAPAction("GatherWood").SetPreconditions
                (
                    (a) =>
                    {
                        return
                            a.HasAxe &&
                            a.Energy >= 35;
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.Energy = a.Energy -35f;
                        b.Wood = a.Wood +10;
                    }
                ),
            new GOAPAction("PickAxe").SetPreconditions
                (
                    (a) =>
                    {
                        return !a.HasAxe;
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.HasAxe = true;
                    }
                ),
            new GOAPAction("Sleep").SetPreconditions
                (
                    (a) =>
                    {
                        return
                            a.BedBuilt &&
                            a.Energy <= 75;
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.Energy = a.Energy+50f;
                    }
                ),
            new GOAPAction("PickHammer").SetPreconditions
                (
                (a) =>
                    {
                        return !a.HasHammer;
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.HasHammer = true;
                    }
                ),
            new GOAPAction("BuildBed").SetPreconditions
                (
                    (a) =>
                    {
                        return
                            !a.BedBuilt &&
                            a.Energy >= 40f;
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.Energy = a.Energy-40f;
                        b.BedBuilt = true;
                    }
                ),
        };

        current = new GOAPState(100, 0);

        //We need a way to set the boolean for HouseBuilt as required! Maybe turn to into a func as well?
        GOAPState to = new GOAPState(100f);

        GOAP.RunGOAP(this, current, to, actions);
    }
}
