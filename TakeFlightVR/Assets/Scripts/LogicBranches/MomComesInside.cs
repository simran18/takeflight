using UnityEngine;
using System.Collections;

public class MomComesInside : LogicBranch
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

    public override string Name => "MomComesInside";

    protected override void Awake()
    {
        momTransform = mom.transform;
        dialog.gameObject.SetActive(false);
        base.Awake();
    }

    protected override void OnCall()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
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
            dialogButtonsRoot.SetActive(false);
            StartCoroutine(TextMeshFadeCoroutine.Fade(dialog, 1, 0, .1f));
            MoveToBranch("MomAsksToGetOnBucket");
        }
    }
}
