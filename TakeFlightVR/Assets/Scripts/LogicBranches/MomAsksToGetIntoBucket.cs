using UnityEngine;
using System.Collections;

public class MomAsksToGetIntoBucket : LogicBranch
{
    [Header("Before Flying")]
    public GameObject bin;
    public AttachGameObject attacher;
    public AlwaysFaceToPlayer momAlwaysFaceToPlayer;
    [Header("After Flying")]
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    [Header("Dialog")]
    public TextMesh dialog;
    public OVRInput.Button dialogButton = OVRInput.Button.One;
    public AudioSource dialogVoice;
    public OVRButtonEvent onButtonDown;

    public override string Name => "MomAsksToGetOnBucket";

    protected override void Awake()
    {
        if (onButtonDown == null) onButtonDown = new OVRButtonEvent();
        onButtonDown.AddListener(OnWearButtonClick);
        base.Awake();
    }

    protected override void OnCall()
    {
        StartCoroutine(OnAction());
    }

    private IEnumerator OnAction()
    {
        dialog.gameObject.SetActive(true);
        //dialogVoice.Play();
        yield return TextMeshFadeCoroutine.Fade(dialog, 0, 1, 1);
        yield return OVRButtonCoroutine.WaitForButtonDown(dialogButton, onButtonDown);
    }

    public void OnWearButtonClick(OVRInput.Button button)
    {
        dialog.gameObject.SetActive(false);
        if (dialogVoice.isPlaying) dialogVoice.Stop();
        StartCoroutine(TextMeshFadeCoroutine.Fade(dialog, 1, 0, .1f));
        OnFlyingBegin();
    }

    public void OnFlyingBegin()
    {
        momAlwaysFaceToPlayer.enabled = false;
        attacher.Attach();
    }

    public void OnFlyingEnd()
    {
        var player = GameObject.FindGameObjectWithTag("Player").transform;
        attacher.Detach();
        momAlwaysFaceToPlayer.enabled = true;
        player.localPosition = playerPosition;
        player.localRotation = playerRotation;
        // onCallEnd("next cut");
    }
}
