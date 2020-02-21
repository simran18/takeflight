using UnityEngine;
using System.Collections;

public class MomAsksToGetIntoBucket : Callable
{
    public GameObject bin;
    [Header("After Then")]
    public TextMesh dialog;
    public GameObject dialogButtonsRoot;

    private AttachGameObject attacher;
    private Transform playerTransform;
    private LogicController.OnCallEndHandler onCallEnd;

    public override string Name => "MomAsksToGetOnBucket";

    protected override void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        attacher = bin.GetComponentInChildren<AttachGameObject>();
        attacher.attachmentTransform = playerTransform;
        base.Awake();
    }

    protected override void OnCall(LogicController.OnCallEndHandler onCallEnd)
    {
        this.onCallEnd = onCallEnd;
        attacher.gameObject.SetActive(false);
        StartCoroutine(OnAction());
    }

    private IEnumerator OnAction()
    {
        dialog.gameObject.SetActive(true);
        dialogButtonsRoot.SetActive(true);
        var iter = TextMeshFadeCoroutine.Fade(dialog, 0, 1, 1);
        while (iter.MoveNext())
        {
            yield return iter.Current;
        }
    }

    public void OnWearButtonClick()
    {
        if (dialogButtonsRoot.activeSelf)
        {
            dialog.gameObject.SetActive(false);
            dialogButtonsRoot.SetActive(false);
            StartCoroutine(TextMeshFadeCoroutine.Fade(dialog, 1, 0, .1f));
            OnFlyingBegin();
        }
    }

    public void OnFlyingBegin()
    {
        attacher.gameObject.SetActive(true);
    }

    public void OnFlyingEnd()
    {
        attacher.gameObject.SetActive(false);
        // onCallEnd("next cut");
    }
}
