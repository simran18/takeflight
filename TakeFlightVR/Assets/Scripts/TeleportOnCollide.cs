using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TeleportOnCollide : MonoBehaviour
{
    public delegate void OnTeleportCallback();

    public GameObject player;
    public TeleportPosition teleportTo;

    private OVRPlayerController playerController;
    private Transform playerTransform;
    private OVRScreenFade screenFade;

    public event OnTeleportCallback onTeleport;

    void Awake()
    {
        playerController = player.GetComponent<OVRPlayerController>();
        playerTransform = player.transform;
        screenFade = player.GetComponentInChildren<OVRScreenFade>();
        Assert.IsNotNull(playerController);
        Assert.IsNotNull(screenFade);
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            StartCoroutine(Teleport());
        }
    }

    protected IEnumerator Teleport()
    {
        playerController.Teleported = true;
        var fadeOut = screenFade.Fade(0f, 1f);
        while (fadeOut.MoveNext())
        {
            yield return fadeOut.Current;
        }
        playerTransform.position = teleportTo.position.GetValueOrDefault(playerTransform.position);
        onTeleport?.Invoke();
        yield return null;
        var fadeIn = screenFade.Fade(1f, 0f);
        while (fadeIn.MoveNext())
        {
            yield return fadeIn.Current;
        }
        playerController.Teleported = false;
    }
}
