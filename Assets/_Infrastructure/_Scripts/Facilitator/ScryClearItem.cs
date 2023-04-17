using Autohand;
using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScryClearItem : MonoBehaviour
{
    public GameObject goOriginal;
    public GameObject goImmitation;

    [ContextMenu("Clean Battery")]
    void CleanBattery()
    {
        Transform[] trans = goImmitation.GetComponentsInChildren<Transform>(true);


        foreach (Transform t in trans)
        {
            if (!t)
                continue;

            GameObject g = t.gameObject;
            Debug.Log("Processing: " + g.name);

         



            string gName = g.name.ToLower();
            if (gName.Contains("_demo") || gName.Contains("demo_") || gName.Contains("anim_") || gName.Contains("_anim") || gName.Contains("animation") || gName.Contains("_collider") || gName.Contains("_button") || gName.Contains("animator") || gName.Contains("anim") || gName == "liftinganimation")
            {
                Debug.Log(gName + " Destroyed Object based on " + "name");
                DestroyImmediate(g);
                continue;
            }







            TutorialService ts = g.GetComponent<TutorialService>();
            if (ts)
            {
                DestroyImmediate(ts);
            }



            RealtimeTransform rtt = g.GetComponent<RealtimeTransform>();
            if (rtt)
            {
                g.AddComponent<TransformReflection>();
                DestroyImmediate(rtt);
            }

            RealtimeView rtv = g.GetComponent<RealtimeView>();
            if (rtv)
                DestroyImmediate(rtv);

            HingeJoint hj = g.GetComponent<HingeJoint>();
            if (hj)
                DestroyImmediate(hj);


            PhysicsGadgetButton pgb = g.GetComponent<PhysicsGadgetButton>();
            if (pgb)
                DestroyImmediate(pgb);

            ConfigurableJoint cj = g.GetComponent<ConfigurableJoint>();
            if (cj)
                DestroyImmediate(cj);

            Collider coll = g.GetComponent<Collider>();
            if (coll)
                DestroyImmediate(coll);

            Rigidbody rigid = g.GetComponent<Rigidbody>();
            if (rigid)
                DestroyImmediate(rigid);



            Grabbable grab = g.GetComponent<Grabbable>();
            if (grab)
                DestroyImmediate(grab);

            Animator anim = g.GetComponent<Animator>();
            if (anim)
                DestroyImmediate(anim);
        }
    }


    [ContextMenu("Bind Transforms")]
    void BindTransforms()
    {
        string path_1 = GetGameObjectPath(goOriginal);
        string path_2 = GetGameObjectPath(goImmitation);

        Debug.Log("Path 1: " + path_1);
        Debug.Log("Path 2: " + path_2);


        TransformReflection[] rts = GetComponentsInChildren<TransformReflection>(true);

        foreach (TransformReflection t in rts)
        {
            string target_path = path_1 + GetGameObjectPath(t.gameObject).Substring(path_2.Length);
            GameObject go = GameObject.Find(target_path);
            if (go)
            {
                t.target = go.transform;
                Debug.Log("Got set: " + target_path);
            }
            else
                Debug.LogError("Couldn't find " + target_path);


            //Debug.Log(GameObject.Find(path_a).transform.position);

        }
    }

    string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }
}