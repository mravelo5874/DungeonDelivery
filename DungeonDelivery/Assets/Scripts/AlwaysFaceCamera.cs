using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    void Update()
    {
        var fwd = Camera.main.transform.forward;
        fwd.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(fwd);
    }
}
