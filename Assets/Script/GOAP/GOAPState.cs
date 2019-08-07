using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GOAPState
{
    float energy;
    int wood;
    bool hasAxe;
    bool hasHammer;
    bool bedBuilt;
    bool houseBuilt;
    string equipped;
    public float Energy { get => energy; set => energy = value; }
    public int Wood { get => wood; set => wood = value; }
    public bool HasAxe { get => hasAxe; set => hasAxe = value; }
    public bool HasHammer { get => hasHammer; set => hasHammer = value; }
    public string Equipped { get => equipped; set => equipped = value; }
    public bool BedBuilt { get => bedBuilt; set => bedBuilt = value; }
    public bool HouseBuilt { get => houseBuilt; set => houseBuilt = value; }

    public GOAPAction generatingAction = null;
    public int stepId = 0;


    public GOAPState(float energy = 0, int wood = 0, bool hasAxe = false, bool hasHammer = false, bool bedBuilt = false, bool houseBuilt = false, string equipped = "")
    {
        this.energy = energy;
        this.wood = wood;
        this.hasAxe = hasAxe;
        this.hasHammer = hasHammer;
        this.bedBuilt = bedBuilt;
        this.houseBuilt = houseBuilt;
        this.equipped = equipped;
    }

    public bool Satisfies(GOAPState obj)
    {
        return
            energy >= obj.Energy &&
            wood >= obj.Wood &&
            hasAxe == obj.HasAxe &&
            hasHammer == obj.HasHammer &&
            equipped == obj.Equipped;
    }

    public float Heuristics(GOAPState target, float energyPriority, float woodPriority, float hasAxePriority, float hasHammerPriority, float equippedPriority)
    {
        float cost = 0;
        cost += target.Energy > energy ? (target.Energy - energy * energyPriority) : 0;
        cost += target.Wood > wood ? (target.Wood - wood) * woodPriority : 0;
        cost += target.HasHammer != hasHammer ? hasHammerPriority : 0;
        cost += target.HasAxe != hasAxe ? hasAxePriority : 0;
        cost += target.Equipped != equipped ? equippedPriority : 0;
        Debug.Log("Cost: " + cost);
        return cost;
    }

    //Simply replace values
    public GOAPState UpdateValues(GOAPState baseState)
    {
        energy = baseState.Energy;
        wood = baseState.Wood;
        hasAxe = baseState.HasAxe;
        hasHammer = baseState.HasHammer;
        equipped = baseState.Equipped;
        return this;
    }
}