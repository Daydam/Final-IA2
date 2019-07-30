using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPEntity : MonoBehaviour
{
    Dictionary<string, float> priorities;
    public float GetPriority(string key)
    {
        if (!priorities.ContainsKey(key)) priorities.Add(key, 0);
        return priorities[key];
    }
}
