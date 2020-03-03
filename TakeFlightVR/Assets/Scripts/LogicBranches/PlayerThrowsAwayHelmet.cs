using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowsAwayHelmet : LogicBranch
{
    [Header("Model References")]
    public GameObject bin;
    public GameObject helmet;
    [Header("Throw Helmet Dialog")]
    public TextMesh dialog;
    public OVRInput.Button dialogButton = OVRInput.Button.One;
    public OVRButtonEvent onButtonDown;
    [Header("Scene Data")]
    public float delayBeforeGetOutOfBucket = 3f;
    public Vector3 positionAfterGetOutOfBucket;
    public Quaternion rotationAfterGetOutOfBucket;
    public OVRScreenFade screenFade;

    public override string Name => "PlayerThrowsAwayHelmet";

    protected override void Awake()
    {
        if (onButtonDown == null) onButtonDown = new OVRButtonEvent();
        onButtonDown.AddListener(OnThrowButtonClick);
        base.Awake();
    }

    protected override void OnCall()
    {
        StartCoroutine(OnAction());
    }

    private IEnumerator OnAction()
    {
        yield return new WaitForSeconds(delayBeforeGetOutOfBucket);
        yield return screenFade.Fade(0f, 1f);
        yield return new WaitForSeconds(2.5f);
        yield return screenFade.Fade(1f, 0f);
        dialog.gameObject.SetActive(true);
        yield return StartCoroutine(TextMeshFadeCoroutine.FadeIn(dialog));
        OVRButtonCoroutine.WaitForButtonDown(dialogButton, onButtonDown);
    }

    public void OnThrowButtonClick(OVRInput.Button button)
    {
        dialog.gameObject.SetActive(false);
        StartCoroutine(TextMeshFadeCoroutine.FadeOut(dialog));
        ThrowHelmet();
    }

    private void ThrowHelmet()
    {
        // TODO
        Debug.Log("Throw Helmet");
    }
}
