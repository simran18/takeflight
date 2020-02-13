using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPosition : MonoBehaviour
{
    public Vector3? position;

    void Awake()
    {
        if (!position.HasValue || position.Value == Vector3.zero)
        {
            position = gameObject.transform.position;
        }
    }
}
