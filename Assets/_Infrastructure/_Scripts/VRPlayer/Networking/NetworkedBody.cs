using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedBody : MonoBehaviour
{
    public Transform followPoint;

    void Update()
    {
        Vector3 tp = followPoint.localPosition;
        tp.y = transform.localPosition.y;
        transform.localPosition = tp;
    }
}
