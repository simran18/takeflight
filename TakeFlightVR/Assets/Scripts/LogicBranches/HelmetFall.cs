using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetFall : LogicBranch
{
    [Header("Helmet Falling")]
    private Animator animationController;
    public AudioSource momHelmetVoice; // need voice! 
    public GameObject mom;
    private Transform momTransform;
    public TextMesh momDialog;
    public float flyingTime;
    public float movingTime;
    // 514.7, 526.36, 542.7
    public Vector3 landPosition;
    // 508.54, 526.36, 771.76
    public Vector3 destinationPosition;

    [Header("Mom Flies Away")]
    public TextMesh buttonText;
    public OVRInput.Button dialogButton = OVRInput.Button.One;
    public float delayAfterMomFly = 3f;

    [Header("Unity Event")]
    public OVRButtonEvent onButtonDown;

    public override string Name => "HelmetFallingMomFlying";

    protected override void Awake()
    {
        if (onButtonDown == null) onButtonDown = new OVRButtonEvent();
        onButtonDown.AddListener(OnButtonFlyClick);
        momTransform = mom.transform;
        buttonText.gameObject.SetActive(false);
        momDialog.gameObject.SetActive(false);
        base.Awake();
    }

    protected override void OnCall()
    {
        StartCoroutine(Animate());
    }

    // Helmet falls down animation
    // mom speech bubble "stay here! While I go pick up your helmet"
    // Mom moves to the ground to the helmet
    IEnumerator Animate()
    {
        animationController.SetBool("helmetFall", true); 
        animationController.SetBool("helmetFall", false);
        yield return TextMeshFadeCoroutine.FadeIn(momDialog);
        momHelmetVoice.Play();
        momDialog.gameObject.SetActive(false);
        StartCoroutine(TextMeshFadeCoroutine.FadeOut(momDialog));
        yield return new WaitForSeconds(1f);
        yield return MoveCoroutine.Move(momTransform, landPosition, flyingTime);
        yield return MoveCoroutine.Move(momTransform, destinationPosition, movingTime);
        yield return TextMeshFadeCoroutine.FadeIn(buttonText);
        yield return OVRButtonCoroutine.WaitForButtonDown(dialogButton, onButtonDown);
    }

    // UI selection triggers animation(child takes off)
    public void OnButtonFlyClick(OVRInput.Button button)
    {
        buttonText.gameObject.SetActive(false);
        StartCoroutine(TextMeshFadeCoroutine.FadeOut(buttonText));
        MoveToBranch("SceneOwenFlying");
    }
}
