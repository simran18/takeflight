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

    private Transform momTransform;

    public override string Name => "MomComesInside";

    protected override void Awake()
    {
        momTransform = mom.transform;
        dialog.gameObject.SetActive(false);
        base.Awake();
    }

    protected override void OnCall(CallbackBranches.OnCallEndHandler onCallEnd)
    {
        StartCoroutine(Move(onCallEnd));
    }

    IEnumerator Move(CallbackBranches.OnCallEndHandler onCallEnd)
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
        iter = TextMeshFadeCoroutine.Fade(dialog, 0, 1, 1);
        while (iter.MoveNext())
        {
            yield return iter.Current;
        }
        yield return new WaitForSeconds(2f);
        onCallEnd("MomAsksToGetOnBucket");
        iter = TextMeshFadeCoroutine.Fade(dialog, 0, 1, 1);
        while (iter.MoveNext())
        {
            yield return iter.Current;
        }
    }
}
