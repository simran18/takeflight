using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogicBranch : LogicBranch
{
    [Header("Logic")]
    public string name;
    public string next;
    public float delayInseconds = 1f;

    public override string Name => name;

    protected override void OnCall()
    {
        StartCoroutine(Log());
    }

    private IEnumerator Log()
    {
        yield return new WaitForSeconds(delayInseconds);
        Debug.Log($"Now in logic branch: {name}");
        Debug.Log($"Next: {next}");
        if (next != null && next != string.Empty)
        {
            MoveToBranch(next);
        }
    }
}
