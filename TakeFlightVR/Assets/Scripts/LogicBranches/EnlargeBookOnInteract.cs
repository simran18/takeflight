using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnlargeBookOnInteract : LogicBranch
{
    [Header("Parameters")]
    public GameObject book;
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    public Vector3 targetScale;
    public float movingAndRotatingTime = 4f;
    public float scalingTime = 2f;
    [Header("Dialog Button")]
    public GameObject buttonDialog;
    public OVRInput.Button yesButton = OVRInput.Button.One;
    public float buttonShowingDelay = 1f;
    public AudioSource pageFlipSound;
    public AudioSource bookVoice;
    public OVRButtonEvent dialogButtonEvent;

    private Transform bookTransform;
    private Quaternion targetRotationQuaternion;

    public override string Name => "EnlargeBookOnInteract";

    protected override void Awake()
    {
        if (book == null) book = this.gameObject;
        if (dialogButtonEvent == null) dialogButtonEvent = new OVRButtonEvent();
        dialogButtonEvent.AddListener(OnYesButtonTriggered);
        bookTransform = book.transform;
        targetRotationQuaternion = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);
        base.Awake();
    }

    protected override void OnCall()
    {
        buttonDialog.SetActive(false);
        StartCoroutine(MoveAndEnlarge());
    }

    IEnumerator MoveAndEnlarge()
    {
        yield return new WaitForSeconds(1f);
        var elapsedTime = 0f;
        var startingPosition = bookTransform.localPosition;
        var startingRotation = bookTransform.localRotation;
        var startingScale = bookTransform.localScale;
        pageFlipSound.Play();
        while (elapsedTime < movingAndRotatingTime) {
            var timeRatio = elapsedTime / movingAndRotatingTime;
            bookTransform.localPosition = Vector3.Lerp(startingPosition, targetPosition, timeRatio);
            bookTransform.localRotation = Quaternion.Lerp(startingRotation, targetRotationQuaternion, timeRatio);
            bookTransform.localScale = Vector3.Lerp(startingScale, targetScale, timeRatio);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        bookTransform.localPosition = targetPosition;
        bookTransform.localRotation = targetRotationQuaternion;
        bookTransform.localScale = targetScale;
        pageFlipSound.Stop();
        yield return new WaitForSeconds(buttonShowingDelay);
        buttonDialog.SetActive(true);
        bookVoice.Play();
        yield return OVRButtonCoroutine.WaitForButtonDown(yesButton, dialogButtonEvent);
    }

    public void OnYesButtonTriggered(OVRInput.Button button)
    {
        bookVoice.Stop();
        MoveToBranch("MomComesInside");
    }
}
