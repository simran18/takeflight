using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Platform.Samples.VrHoops;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class TeleportToSceneOnCollide : MonoBehaviour
{
    public delegate void OnTeleportCallback();

    public string sceneName;
    public Vector3 targetPosition;

    private Transform playerTransform;
    private OVRScreenFade screenFade;

    public event OnTeleportCallback onTeleport;

    void Awake()
    {
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            DontDestroyOnLoad(this);
            StartCoroutine(TeleportToScene());
        }
    }

    protected IEnumerator TeleportToScene()
    {
        var asyncResult = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncResult.completed += (op) =>
        {
            var player = GameObject.FindWithTag("Player");
            Assert.IsNotNull(player);
            playerTransform = player.transform;
            screenFade = player.GetComponentInChildren<OVRScreenFade>();
            Assert.IsNotNull(screenFade);
        };
        var fadeOut = screenFade.Fade(0f, 1f);
        while (screenFade.isActiveAndEnabled && fadeOut.MoveNext())
        {
            yield return fadeOut.Current;
        }
        yield return new WaitUntil(() => asyncResult.isDone);
        playerTransform.position = targetPosition;
        onTeleport?.Invoke();
        yield return null;
        var fadeIn = screenFade.Fade(1f, 0f);
        while (fadeIn.MoveNext())
        {
            yield return fadeIn.Current;
        }
        Destroy(this);
    }
}
