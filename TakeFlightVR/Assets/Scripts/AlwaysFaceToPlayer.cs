using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class AlwaysFaceToPlayer : MonoBehaviour {
    private Transform playerTransform;

    protected void Awake() {
        var player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;
    }

    void Update() {
        this.transform.rotation = Quaternion.LookRotation(this.transform.position - playerTransform.position);
    }
}