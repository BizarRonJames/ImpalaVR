using Autohand;
using Normal.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorkingArea_Battery : MonoBehaviour
{
    
    
    [Header("References")]
    public Information infoMain;
    public ScryFace face;
    public Transform tranStartPoint;
    public Transform tranFacilitatorPoint;
    public GameObject btnExitArea;
    public Transform scryArea;

    [Header("Actions")]
    public UnityEvent actionsOnClaim;
    public UnityEvent actionsOnVisit;
    public UnityEvent actionReconnectI;


    [Header("ObjectsToHide")]
    public GameObject[] objectsToHide;

    Realtime realTime;


    List<MeshRenderer> meshes = new List<MeshRenderer>();
    List<SkinnedMeshRenderer> skinned = new List<SkinnedMeshRenderer>();

    private void Start()
    {
        MeshRenderer[] mRen = GetComponentsInChildren<MeshRenderer>(true);
        foreach(MeshRenderer m in mRen)
            if (m.enabled)
            {
                meshes.Add(m);
                m.enabled = false;
            }

        SkinnedMeshRenderer[] sk = GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach(SkinnedMeshRenderer sm in sk)
            if (sm.enabled)
            {
                skinned.Add(sm);
                sm.enabled = false;
            }

        ToggleObjects(false);
    }

    [ContextMenu("Claim")]
    public void Claim()
    {        
        TeleportTo(NetworkedPlayer.instance?NetworkedPlayer.instance.isHost:false);
        if(infoMain)
            infoMain.gameObject.SetActive(true);
        if (NetworkedPlayer.instance)
        {
            //if (DevelopmentState.instance.IsTesting && false)
            //    NetworkedPlayer.instance.AssignToScry(NetworkedPlayer.instance.playerIndex);
            //else
                NetworkedPlayer.instance.AssignToScry(NetworkedPlayer.instance.playerIndex - 1);
        }

        if(btnExitArea)
            btnExitArea.SetActive(false);

        if (NetworkStatus.instance.IsConnected)
            TakeOwnership();

        if (actionsOnClaim != null)
        {
            Debug.Log("Invoking Actions On Claim");
            actionsOnClaim.Invoke();            
        }

        if (infoMain)
            Information.instance = infoMain;

        ToggleMeshes(true);
    }

    [ContextMenu("Teleport As Admin")]
    void TeleportToAsAdmin()
    {
        TeleportTo(true);
    }

    public void TeleportTo(bool isAdmin)
    {
        if (isAdmin)
        {
            actionsOnVisit.Invoke();
            Player.instance.Teleport(tranFacilitatorPoint);
        }
        else
            Player.instance.Teleport(tranStartPoint);
        ShowArea();

        ToggleMeshes(true);
    }
    
    public void TeleportToAdminArea()
    {
        WorkingAreas.instance.areas[0].TeleportTo(true);
    }

    public void ShowArea()
    {
        ToggleObjects(true);
    }

    public void ExitArea()
    {
        ToggleObjects(false);
        ToggleMeshes(false);
    }

    void ToggleObjects(bool flag)
    {
        foreach (GameObject go in objectsToHide)
            if(go)
                go.SetActive(flag);
    }

    [ContextMenu("Test Teleport")]
    void TestTeleport()
    {
        WorkingAreas.instance.areas[0].Claim();
    }

    public void Reconnect()
    {
        Player.instance.Teleport(tranStartPoint);
        //TakeOwnership();
        if (actionReconnectI!=null)
            actionReconnectI.Invoke();

    }


    void TakeOwnership()
    {
        realTime = FindObjectOfType<Realtime>();
        OVRManager.HMDMounted += HandleHMDMounted;
        RealtimeView[] views = GetComponentsInChildren<RealtimeView>(true);
        RealtimeTransform[] transforms = GetComponentsInChildren<RealtimeTransform>(true);        
        if (NetworkStatus.instance.IsConnected)
        {
            foreach (RealtimeView view in views)
            {
                if (view.gameObject.activeSelf || true)
                {
                    try
                    {
                        if (view.ownerIDSelf == -1)
                        {
                            view.RequestOwnership();
                            view.destroyWhenOwnerLeaves = false;
                            view.destroyWhenLastClientLeaves = false;
                        }
                    }
                    catch(Exception ex)
                    {
                        //Debug.Log("Cant own view: " + view.gameObject.name + "\n" + ex.ToString());
                    }
                }
            }
            //foreach (RealtimeTransform tran in transforms)
            //{
            //    if (tran.gameObject.activeSelf || true)
            //    {
            //        try
            //        {
            //            tran.RequestOwnership();

            //            GrabbableReturnHome grh = tran.gameObject.GetComponent<GrabbableReturnHome>();
            //            if (grh)
            //                grh.GoHome();
            //        }
            //        catch(Exception ex)
            //        {

            //        }
            //    }
            //}
        }
        else
        {
            foreach (RealtimeTransform tran in transforms)
                tran.enabled = false;
            foreach (RealtimeView view in views)
                view.enabled = false;
        }

    }

    private void HandleHMDMounted()
    {
        if (realTime)
        {
            if (!realTime.connected)
            {
                Debug.Log("HMD Mounted Event: Not Connected");
                realTime.Connect(SessionDetails.instance.session_name);
            }
            else
                Debug.Log("HMD Mounted Event: Still Connected");
        }
        else
            Debug.LogError("No Realtime View Found");
    }

    void ToggleMeshes(bool flag)
    {
        foreach (MeshRenderer m in meshes)
            m.enabled = flag;

        foreach (SkinnedMeshRenderer m in skinned)
            m.enabled = flag;
    }

    public void VisitPreview()
    {
        StartCoroutine(VisitPreviewI());
    }

    IEnumerator VisitPreviewI()
    {
        yield return new WaitForSeconds(5);
        TeleportToAsAdmin();
    }

    public List<Grabbable> grabblesToDealWith;

    [ContextMenu("Fix Screws")]
    void FixScrews()
    {
        grabblesToDealWith = new List<Grabbable>();
        Grabbable[] gr = GetComponentsInChildren<Grabbable>();
        foreach (Grabbable g in gr)
            if (g.gameObject.GetComponent<RealtimeTransform>())
            {
                Rigidbody[] rigids = g.gameObject.GetComponentsInChildren<Rigidbody>();
                if (rigids.Length > 1)
                {
                    grabblesToDealWith.Add(g);
                    Debug.Log(rigids.Length + " Too many rigid bodies: " + g.gameObject.name);
                }
            }
            
    }

    [Header("Baking")]
    public Transform[] bakeSpots;
    public Transform[] bakeTransform;
    public Vector3[] bakePoisitions;

    [ContextMenu("Prepare Bake")]
    void PrepareBake()
    {
        bakePoisitions = new Vector3[bakeTransform.Length];
        for(int i=0; i<bakeSpots.Length && i < bakeTransform.Length; i++)
        {
            bakePoisitions[i] = bakeTransform[i].position;
            bakeTransform[i].position = bakeSpots[i].position;
        }
    }

    [ContextMenu("Unroll Bake")]
    void UnrollBake()
    {
        if (bakePoisitions.Length > 0)
        {
            for (int i = 0; i < bakeSpots.Length && i < bakeTransform.Length; i++)
                bakeTransform[i].position = bakePoisitions[i];
        }
    }


}
