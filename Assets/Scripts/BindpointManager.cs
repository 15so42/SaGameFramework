using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindpointManager : MonoBehaviour
{
    private Dictionary<string, BindPoint> bindPoints = new Dictionary<string, BindPoint>();

    public void Init()
    {
        var bindPoints = GetComponentsInChildren<BindPoint>();
        foreach (var bindPoint in bindPoints)
        {
            this.bindPoints.Add(bindPoint.key,bindPoint);
        }
    }

    public BindPoint GetBindPoint(string key)
    {
        if (bindPoints.ContainsKey(key))
        {
            return bindPoints[key];
        }

        return null;
    }
}
