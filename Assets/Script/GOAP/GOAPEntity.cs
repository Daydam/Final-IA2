using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GOAPEntity : MonoBehaviour
{
    public float speed;

    List<GOAPAction> actions;

    GOAPState current;
    public GOAPState Current { get { return current; } set { current = value; } }

    Func<GOAPState, bool> satisfies;
    public Func<GOAPState, bool> Satisfies { get { return satisfies; } }
    Func<GOAPState, float> heuristics;
    public Func<GOAPState, float> Heuristics { get { return heuristics; } }


    IEnumerable<GOAPAction> sequence;
    public IEnumerable<GOAPAction> Sequence { get { return sequence; } set { sequence = value; } }

    private void Start()
    {

        actions = new List<GOAPAction>()
        {
            new GOAPAction("BuildHouse").SetPreconditions
                (
                    (a) =>
                    {
                        return
                            a.HasHammer &&
                            a.Energy >= 65f &&
                            a.Wood >= 10 &&
                            a.PatienceRate >= 2 &&
                            !a.HouseOwned;
                    }
                ).SetEffects
                (
                    (a, b) =>
                    {
                        b.Energy = a.Energy -65f;
                        b.Wood = a.Wood -10;
                        b.HouseOwned = true;
                    }
                ).SetCost(5)
                .SetAct(a => StartCoroutine(ActionRoutines.BuildHouseCoroutine(a))),
            new GOAPAction("GatherWood").SetPreconditions
                (
                    (a) =>
                    {
                        return
                            a.HasAxe &&
                            a.Energy >= 40f;
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.Energy = a.Energy -35f;
                        b.Wood = a.Wood + 10;
                    }
                ).SetCost(3.5f)
                .SetAct(a => StartCoroutine(ActionRoutines.GatherWoodCoroutine(a))),
            new GOAPAction("PickAxe").SetPreconditions
                (
                    (a) =>
                    {
                        return
                            !a.HasAxe &&
                            a.PatienceRate >= 1;
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.HasAxe = true;
                        b.PatienceRate = a.PatienceRate -1;
                    }
                ).SetCost(0.5f)
                .SetAct(a => StartCoroutine(ActionRoutines.GetToolCoroutine(a, ToolType.AXE))),
            new GOAPAction("Sleep").SetPreconditions
                (
                    (a) =>
                    {
                        return
                            a.BedBuilt &&
                            a.Energy <= 75 &&
                            a.PatienceRate >= 2;
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.Energy = a.Energy + 50;
                        b.PatienceRate = a.PatienceRate + 2;
                    }
                ).SetCost(1f)
                .SetAct(a => StartCoroutine(ActionRoutines.SleepCoroutine(a))),
            new GOAPAction("PickHammer").SetPreconditions
                (
                (a) =>
                    {
                        return
                            !a.HasHammer &&
                            a.PatienceRate >= 1;
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.HasHammer = true;
                        b.PatienceRate = a.PatienceRate -1;
                    }
                ).SetCost(0.5f)
                .SetAct(a => StartCoroutine(ActionRoutines.GetToolCoroutine(a, ToolType.HAMMER))),
            new GOAPAction("BuildBed").SetPreconditions
                (
                    (a) =>
                    {
                        return
                            !a.BedBuilt &&
                            a.Energy >= 40f &&
                            a.Wood >= 3 &&
                            a.PatienceRate >= 1;
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.Energy = a.Energy-40f;
                        b.BedBuilt = true;
                        b.PatienceRate = a.PatienceRate -1;
                    }
                ).SetCost(2f)
                .SetAct(a => ActionRoutines.BuildBed(a)),
            new GOAPAction("SummonSatan").SetPreconditions
                (
                    (a) =>
                    {
                        return
                            a.PatienceRate == 0;
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.Energy = 100;
                        b.HouseOwned = true;
                    }
                ).SetCost(10f)
                .SetAct(a => StartCoroutine(ActionRoutines.SummonSatanCoroutine(a))),
            new GOAPAction("KillEnemy").SetPreconditions
                (
                    (a) =>
                    {
                        return
                            a.PatienceRate <= 3 &&
                            a.HasAxe &&
                            a.Energy <= 50 &&
                            a.Energy >= 15;
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.Energy -= 30;
                        b.Keys.Add("House 1");
                        b.PatienceRate += 10;
                    }
                ).SetCost(7.5f)
                .SetAct(a => StartCoroutine(ActionRoutines.KillEnemyCoroutine(a))),
            new GOAPAction("OpenHouse1").SetPreconditions
                (
                    (a) =>
                    {
                        return
                            a.Keys.Contains("House 1");
                    }
                ).SetEffects
                (
                    (a,b) =>
                    {
                        b.HouseOwned = true;
                    }
                ).SetCost(0.5f)
                .SetAct(a => StartCoroutine(ActionRoutines.OpenHouseCoroutine(a))),
        };

        current = new GOAPState(100, 0);

        satisfies = (a) => { return a.Energy >= 90 && a.HouseOwned; };
        heuristics = (a) =>
        {
            float cost = 0;
            cost += Mathf.Max(a.Energy, 0) < 90f ? (90f - Mathf.Max(a.Energy, 0)) * 0.3f : 0f;
            cost += !a.HouseOwned ? 1f : 0f;
            return cost;
        };

        sequence = GOAP.RunGOAPOld(this, current, actions);
        sequence.First().Act(this);
    }
}
