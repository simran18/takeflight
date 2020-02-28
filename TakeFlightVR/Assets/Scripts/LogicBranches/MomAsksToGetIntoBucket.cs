using UnityEngine;
using System.Collections;

public class MomAsksToGetIntoBucket : LogicBranch
{
    public GameObject bin;
    [Header("After Then")]
    public TextMesh dialog;
    public OVRInput.Button dialogButton = OVRInput.Button.One;
    [Header("Unity Event")]
    public OVRButtonEvent onButtonDown;

    private AttachGameObject attacher;
    private Transform playerTransform;

    public override string Name => "MomAsksToGetOnBucket";

    protected override void Awake()
    {
        if (onButtonDown == null) onButtonDown = new OVRButtonEvent();
        onButtonDown.AddListener(OnWearButtonClick);
        var player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        attacher = bin.GetComponentInChildren<AttachGameObject>();
        attacher.attachmentTransform = playerTransform;
        base.Awake();
    }

    protected override void OnCall()
    {
        attacher.gameObject.SetActive(false);
        StartCoroutine(OnAction());
    }

    private IEnumerator OnAction()
    {
        dialog.gameObject.SetActive(true);
        yield return TextMeshFadeCoroutine.Fade(dialog, 0, 1, 1);
        OVRButtonCoroutine.WaitForButtonDown(dialogButton, onButtonDown);
    }

    public void OnWearButtonClick(OVRInput.Button button)
    {
        dialog.gameObject.SetActive(false);
        StartCoroutine(TextMeshFadeCoroutine.Fade(dialog, 1, 0, .1f));
        OnFlyingBegin();
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
