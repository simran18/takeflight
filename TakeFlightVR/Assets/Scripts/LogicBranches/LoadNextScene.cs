using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : LogicBranch
{
    public override string Name => "LoadNextScene -> " + nextSceneName;
    [Space]
    public string nextSceneName;
    public string nextLogicBranchAfterLoading;
    public float delayBeforeLoading = 0f;
    public float delayAfterLoading = 0f;

    protected override void Awake()
    {
        base.Awake();
        disableOnLeave = false;
    }

    protected override void OnCall() {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene() {
        yield return new WaitForSeconds(delayBeforeLoading);
        var ret = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
        ret.completed += op =>
        {
            if (op.isDone)
            {
                StartCoroutine(AfterLoadingScene());
            }
        };
    }

    private IEnumerator AfterLoadingScene()
    {
        yield return new WaitForSeconds(delayAfterLoading);
        MoveToBranch(nextLogicBranchAfterLoading);
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
