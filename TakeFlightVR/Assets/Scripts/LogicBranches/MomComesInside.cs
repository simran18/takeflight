using UnityEngine;
using System.Collections;

public class MomComesInside : LogicBranch
{
    [Header("While Moving Inside")]
    public GameObject mom;
    public float flyingTime;
    public float movingTime;
    // 514.7, 526.36, 542.7
    public Vector3 landPosition;
    // 508.54, 526.36, 771.76
    public Vector3 destinationPosition;
    [Header("After Then")]
    public TextMesh dialog;
    public OVRInput.Button dialogButton = OVRInput.Button.One;
    public AudioSource momVoice;
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
        momVoice.Play();
        yield return TextMeshFadeCoroutine.FadeIn(dialog);
        OVRButtonCoroutine.WaitForButtonDown(dialogButton, onButtonDown);
    }

    public void OnButtonLetsGoClick(OVRInput.Button button)
    {
        StartCoroutine(TextMeshFadeCoroutine.FadeOut(dialog));
        MoveToBranch("MomAsksToGetOnBucket");
    }
}
