using UnityEngine;
using System.Collections.Generic;


public class LogicController : Singleton<LogicController>
{

    public LogicBranch[] branches;
    public LogicBranch startPoint;

    private Dictionary<string, LogicBranch> branchDict;
    private LogicBranch nextCall;

    public LogicController()
    {
        branchDict = new Dictionary<string, LogicBranch>();
    }

    public void AddBranch(LogicBranch c)
    {
        if (!branchDict.ContainsKey(c.Name))
        {
            branchDict[c.Name] = c;
        }
    }

    public void RemoveBranch(LogicBranch c)
    {
        branchDict.Remove(c.Name);
    }

    void Start()
    {
        branches = branches ?? (new LogicBranch[0]);
        foreach (LogicBranch c in branches)
        {
            AddBranch(c);
        }
        nextCall = startPoint;
    }

    private void Update()
    {
        if (nextCall != null)
        {
            Debug.Log($"Get into branch: {nextCall.Name}");
            var c = nextCall;
            nextCall = null;
            c.Call();
        }
    }

    public void MoveToBranch(string branch)
    {
        Debug.Assert(branchDict.ContainsKey(branch));
        this.nextCall = branchDict[branch];
    }
}
