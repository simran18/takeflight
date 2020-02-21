using UnityEngine;
using System.Collections;

public class MomComesInside : Callable
{
    [Header("While Moving Inside")]
    public GameObject mom;
    public float flyingTime;
    public float movingTime;
    public Vector3 landPosition;
    public Vector3 destinationPosition;
    [Header("After Then")]
    public TextMesh dialog;
    public GameObject dialogButtonsRoot;

    private Transform momTransform;
    private LogicController.OnCallEndHandler onCallEnd;

    public override string Name => "MomComesInside";

    protected override void Awake()
    {
        momTransform = mom.transform;
        dialog.gameObject.SetActive(false);
        base.Awake();
    }

    protected override void OnCall(LogicController.OnCallEndHandler onCallEnd)
    {
        this.onCallEnd = onCallEnd;
        StartCoroutine(Move(onCallEnd));
    }

    IEnumerator Move(LogicController.OnCallEndHandler onCallEnd)
    {
        yield return new WaitForSeconds(1f);
        var iter = MoveCoroutine.Move(momTransform, landPosition, flyingTime);
        while (iter.MoveNext())
        {
            yield return iter.Current;
        }
        iter = MoveCoroutine.Move(momTransform, destinationPosition, movingTime);
        while (iter.MoveNext())
        {
            yield return iter.Current;
        }
        dialog.gameObject.SetActive(true);
        dialogButtonsRoot.SetActive(true);
        iter = TextMeshFadeCoroutine.Fade(dialog, 0, 1, 1);
        while (iter.MoveNext())
        {
            yield return iter.Current;
        }
    }

    public void OnButtonLetsGoClick()
    {
        if (dialogButtonsRoot.activeSelf)
        {
            Debug.Log("OnButtonClick");
            dialogButtonsRoot.SetActive(false);
            StartCoroutine(TextMeshFadeCoroutine.Fade(dialog, 255, 0, .1f));
            onCallEnd("MomAsksToGetOnBucket");
        }
    }
}
