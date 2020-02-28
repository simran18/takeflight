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
    public OVRInput.Button dialogButton = OVRInput.Button.One;
    [Header("Unity Event")]
    public OVRButtonEvent onButtonDown;

    private Transform momTransform;

    public override string Name => "MomComesInside";

    protected override void Awake()
    {
        if (onButtonDown == null) onButtonDown = new OVRButtonEvent();
        onButtonDown.AddListener(OnButtonLetsGoClick);
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
        yield return MoveCoroutine.Move(momTransform, landPosition, flyingTime);
        yield return MoveCoroutine.Move(momTransform, destinationPosition, movingTime);
        dialog.gameObject.SetActive(true);
        yield return TextMeshFadeCoroutine.Fade(dialog, 0, 1, 1);
        OVRButtonCoroutine.WaitForButtonDown(dialogButton, onButtonDown);
    }

    public void OnButtonLetsGoClick(OVRInput.Button button)
    {
        StartCoroutine(TextMeshFadeCoroutine.Fade(dialog, 1, 0, .1f));
        MoveToBranch("MomAsksToGetOnBucket");
    }
}
