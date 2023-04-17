using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlookAt : MonoBehaviour
{
    public Hand hand;
    public Camera head;

    public float maxDistance = 0.75f;
    [Range(0, 1)]
    public float anglePreciseness = 0.75f;
    public bool disableWhileHolding = true;

    [Header("Events")]
    public UnityHandEvent OnShow;
    public UnityHandEvent OnHide;


    bool showing = false;
    bool initialized = false;

    private void Start()
    {
        initialized = (hand && head);
    }

    private void Update()
    {
        if (!initialized)
            return;

        var handPos = hand.transform.position;
        var headPos = head.transform.position;

        float lookness = Vector3.Dot((headPos - handPos).normalized, -hand.palmTransform.forward);
        float distance = Vector3.Distance(headPos, hand.palmTransform.position);
        bool found = lookness >= anglePreciseness && distance < maxDistance && hand.holdingObj == null;

        if (!showing && found)
        {
            OnShow?.Invoke(hand);
            showing = true;
        }
        else if (showing && !found)
        {
            OnHide?.Invoke(hand);
            showing = false;
        }
    }
}
