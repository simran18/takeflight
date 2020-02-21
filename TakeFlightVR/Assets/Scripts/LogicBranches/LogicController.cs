using UnityEngine;
using System.Collections.Generic;

public class LogicController : Singleton<LogicController>
{
    public delegate void OnCallEndHandler(string nextCall);

    public Callable[] branches;
    public Callable startPoint;

    private Dictionary<string, Callable> branchDict;
    private Callable nextCall;

    public event OnCallEndHandler OnCallEnd;

    public LogicController()
    {
        this.OnCallEnd = HandleOnCallEnd;
        branchDict = new Dictionary<string, Callable>();
    }

    public void AddCallable(Callable c)
    {
        if (!branchDict.ContainsKey(c.Name))
        {
            branchDict[c.Name] = c;
        }
    }

    public void RemoveCallable(Callable c)
    {
        branchDict.Remove(c.Name);
    }

    void Start()
    {
        branches = branches ?? (new Callable[0]);
        foreach (Callable c in branches)
        {
            AddCallable(c);
        }
        nextCall = startPoint;
    }

    private void Update()
    {
        if (nextCall != null)
        {
            Debug.Log($"Get into branch: {nextCall.Name}");
            nextCall.Call(this.OnCallEnd);
            nextCall = null;
        }
    }

    private void HandleOnCallEnd(string nextCall)
    {
        Debug.Assert(branchDict.ContainsKey(nextCall));
        this.nextCall = branchDict[nextCall];
    }
}
