using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScryFace : MonoBehaviour
{

    [Header("Parents")]
    public Transform parent_Original;
    public Transform parent_Immitation;

    [Header("Values")]
    public Vector3 position_offset = Vector3.zero;
    public Vector3 position_modifier = Vector3.one;
    public double positionScaler = 1.0f;
    public float scaleModifier = 1.0f;


    private void Start()
    {
        /*position_offset = parent_Immitation.position - parent_Original.position;
        if(scaleModifier==0)
            scaleModifier = parent_Immitation.localScale.x / parent_Original.localScale.x;*/

        TransformReflection[] trans = GetComponentsInChildren<TransformReflection>(true);
        foreach (TransformReflection t in trans)
            t.transform.parent = transform;
    }

    public TransformReflection refBody;
    public TransformReflection refHead;
    public TransformReflection refHandL;
    public TransformReflection refHandR;

    public void AssignPlayer(Transform body, Transform head, Transform handL, Transform handR)
    {
        refBody.target = body;
        refHead.target = head;
        refHandL.target = handL;
        refHandR.target = handR;

        GetComponentInChildren<VRIK>(true).enabled = true;
    }
}
