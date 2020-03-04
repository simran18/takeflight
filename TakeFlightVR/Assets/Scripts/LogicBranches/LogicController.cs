using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// Modified from https://wiki.unity3d.com/index.php/Singleton
/// Under CC BY-SA 3.0 https://creativecommons.org/licenses/by-sa/3.0/
public class LogicController : MonoBehaviour
{

    public LogicBranch startPoint;
    public Dictionary<string, LogicBranch> branches = new Dictionary<string, LogicBranch>();
    private LogicBranch nextCall;

    private static LogicController m_Instance;
    private static Object m_Lock = new Object();

    public static LogicController Instance
    {
        get {
            if (m_Instance == null)
            {
                lock (m_Lock)
                {
                    m_Instance = FindObjectOfType<LogicController>();
                    if (m_Instance == null)
                    {
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<LogicController>();
                    }
                }
            }
            return m_Instance;
        }
    }

    private void Merge(LogicController other)
    {
        foreach (var b in other.branches.Values)
        {
            AddBranch(b);
        }
    }

    private void Awake() {
        if (m_Instance != this)
        {
            Debug.Log("Merge and destroy the new LogicController.");
            m_Instance.Merge(this);
            Destroy(this);
        } else
        {
            m_Instance = this;
            gameObject.name = typeof(LogicController).ToString() + " (Singleton)";
            SceneManager.sceneUnloaded += s => branches.Clear();
            DontDestroyOnLoad(this);
        }
    }

    public void AddBranch(LogicBranch c)
    {
        if (!branches.ContainsKey(c.Name))
        {
            branches[c.Name] = c;
        }
    }

    public void RemoveBranch(LogicBranch c)
    {
        branches.Remove(c.Name);
    }

    void Start()
    {
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
        this.nextCall = branches[branch];
    }
}
