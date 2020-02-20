using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class OnTriggerEvent : MonoBehaviour
{
    public UnityEvent onTriggerEnterEvent;
    public UnityEvent onTriggerExitEvent;

    private void OnTriggerEnter(Collider other) {
        if (onTriggerEnterEvent != null) {
            onTriggerEnterEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (onTriggerExitEvent != null) {
            onTriggerExitEvent.Invoke();
        }
    }
}
