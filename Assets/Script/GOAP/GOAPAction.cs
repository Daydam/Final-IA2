using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPAction
{
    Func<GOAPState, bool> preconditions;
    public Func<GOAPState, bool> Preconditions { get { return preconditions; } }
    Func<GOAPState> effects;
    string actionName;
    public string Name { get { return actionName; } }
    float cost;
    public float Cost { get { return cost; } }

    public GOAPAction (string name)
    {
        this.actionName = name;
        cost = 1f;
        preconditions = a => true;
        effects = () => new GOAPState();
    }

    public GOAPAction SetCost (float cost)
    {
        this.cost = cost;
        return this;
    }

    public GOAPAction SetPreconditions(Func<GOAPState, bool> preconditions)
    {
        this.preconditions = preconditions;
        return this;
    }

    public GOAPAction SetEffects(Func<GOAPState> effects)
    {
        this.effects = effects;
        return this;
    }
}
