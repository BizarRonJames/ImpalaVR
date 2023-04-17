using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorScript : MonoBehaviour
{
    [ContextMenu("1. clear light probes")]
    public void clearChildrenLightProbes()
    {
        MeshRenderer[] items = this.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer item in items)
        {
            item.lightProbeUsage = new UnityEngine.Rendering.LightProbeUsage();
        }
    }

    [ContextMenu("1. clear colliders")]
    public void clearColliders()
    {
        Collider[] items = this.gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider item in items)
        {
            DestroyImmediate(item);
        }
    }
}
