using UnityEngine;
using System.Collections;

public class MomAsksToGetIntoBucket : Callable
{
    public GameObject bin;
    public AttachGameObject attacher;

    private Transform playerTransform;

    public override string Name => "MomAsksToGetOnBucket";

    protected override void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        attacher.attachmentTransform = playerTransform;
        base.Awake();
    }

    protected override void OnCall(LogicController.OnCallEndHandler onCallEnd)
    {
        attacher.gameObject.SetActive(true);
    }

    public void OnFlyingEnd()
    {
        attacher.gameObject.SetActive(false);
    }
}
