using UnityEngine;
using System.Collections;

public class AttachTestLogicBranch : LogicBranch
{
    public AttachGameObject attacher;
    public GameObject attachTo;
    public GameObject attachment;
    public float delayBeforeAttaching = 2f;

    public override string Name => "AttachTest";

    protected override void OnCall()
    {
        attacher.attacherTransform = attachTo.transform;
        attacher.attachmentTransform = attachment.transform;
        attacher.Attach();
        StartCoroutine(WaitForAnyButtonDown());
    }

    private IEnumerator WaitForAnyButtonDown()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.A));
        attacher.Detach();
    }
}
