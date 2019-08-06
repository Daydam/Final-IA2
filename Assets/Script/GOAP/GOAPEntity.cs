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
                    new Dictionary<string, Func<object, bool>>()
                    {
                        { "HasHammer", (a) => (bool)a },
                        { "Energy", (a) => (float)a >= 65 },
                        { "Wood", (a) => (int)a >= 10 },
                        { "HouseBuilt", (a) => true }
                    }
                ).SetEffects
                (
                    new Dictionary<string, Func<object, object>>()
                    {
                        { "Energy", (a) => Mathf.Max((float)a - 65f, 0f) },
                        { "Wood", (a) => Mathf.Max((int)a - 10, 0) },
                        { "HouseBuilt", (a) => true }
                    }
                ),
            new GOAPAction("GatherWood").SetPreconditions
                (
                    new Dictionary<string, Func<object, bool>>()
                    {
                        { "HasAxe", (a) => (bool)a },
                        { "Energy", (a) => (float)a >= 35 }
                    }
                ).SetEffects
                (
                    new Dictionary<string, Func<object, object>>()
                    {
                        { "Energy", (a) => Mathf.Max((float)a - 35f, 0f) },
                        { "Wood", (a) => Mathf.Min((int)a + 10, 100) }
                    }
                ),
            new GOAPAction("PickAxe").SetPreconditions
                (
                    new Dictionary<string, Func<object, bool>>()
                    {
                        { "HasAxe", (a) => !(bool)a }
                    }
                ).SetEffects
                (
                    new Dictionary<string, Func<object, object>>()
                    {
                        { "HasAxe", (a) => true }
                    }
                ),
            new GOAPAction("Sleep").SetPreconditions
                (
                    new Dictionary<string, Func<object, bool>>()
                    {
                        { "HasBed", (a) => (bool)a },
                        { "Energy", (a) => (float)a <= 75 }
                    }
                ).SetEffects
                (
                    new Dictionary<string, Func<object, object>>()
                    {
                        { "Energy", (a) => Mathf.Min((float)a + 50f, 100f) }
                    }
                ),
            new GOAPAction("PickHammer").SetPreconditions
                (
                    new Dictionary<string, Func<object, bool>>()
                    {
                        { "HasHammer", (a) => !(bool)a }
                    }
                ).SetEffects
                (
                    new Dictionary<string, Func<object, object>>()
                    {
                        { "HasHammer", (a) => true }
                    }
                ),
            new GOAPAction("BuildBed").SetPreconditions
                (
                    new Dictionary<string, Func<object, bool>>()
                    {
                        { "HasBed", (a) => !(bool)a },
                        { "Energy", (a) => (float)a >= 40 }
                    }
                ).SetEffects
                (
                    new Dictionary<string, Func<object, object>>()
                    {
                        { "Energy", (a) => Mathf.Max((float)a - 40f, 0f) },
                        { "HasBed", (a) => true }
                    }
                ),
        };

        current = new GOAPState().UpdateValues
            (
                new Dictionary<string, object>()
                {
                    //If a value isn't here, this breaks. We need to create a getter for these values.
                    { "Energy", 100f },
                    { "Wood", 0 },
                    { "HasHammer", false },
                    { "HouseBuilt", false },
                    { "HasAxe", false },
                    { "HasBed", false }
                }
            );

        GOAPState to = new GOAPState().UpdateValues
            (
                new Dictionary<string, object>()
                {
                    { "Energy", 100f },
                    { "HouseBuilt", true }
                }
            );

        GOAP.RunGOAP(this, current, to, actions);
    }
}
