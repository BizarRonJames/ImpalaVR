using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformReflection : MonoBehaviour
{
    public bool shouldDebug = false;
    public ScryFace face;
    public Transform target;
    public float scaleModifier;

    private void Start()
    {
        face = GetComponentInParent<ScryFace>();
        if (scaleModifier == 0)
            scaleModifier = face.scaleModifier;
    }

    void Update()
    {
        if(!target ||!target.gameObject.activeSelf)
        {
            if(!GetComponent<VRIK>())
                transform.localScale = Vector3.zero;
            return;
        }

        //transform.position = target.position + face.position_offset;
        //transform.rotation = target.localRotation;
        //transform.localScale = target.localScale * face.scaleModifier;

        /*Vector3 pDiff = target.position - face.parent_Original.position;
        Vector3 pModified = (pDiff * face.scaleModifier) + face.position_offset;
        Vector3 pFinal = face.parent_Immitation.position + pModified;
        transform.position = face.parent_Immitation.position + pModified;
        transform.rotation = target.rotation;
        transform.localScale = target.localScale * face.scaleModifier;*/

        /*Vector3 offset = face.parent_Original.transform.position - target.position;
        Vector3 finalPos = (face.parent_Immitation.position + offset ) * ((float)face.positionScaler*1000);
        finalPos = new Vector3((finalPos.x * face.position_modifier.x)/1000, (finalPos.y * face.position_modifier.y)/1000, (finalPos.z * face.position_modifier.z)/1000);
        transform.position = finalPos;
        transform.localScale = target.localScale * face.scaleModifier;*/

        Vector3 offset = target.position - face.parent_Original.transform.position;
        Vector3 finalPos = (face.transform.position + (offset * face.scaleModifier)) + face.position_offset;
        transform.rotation = target.rotation;
        transform.position = finalPos;

        if (shouldDebug)
        {
            Debug.Log("Offset: " + offset);
            Debug.Log("FinalPos: " + finalPos);
        }
    }
}
