using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
using Normal.Realtime;
using System;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public Material[] emmissionMaterial;
    public static Player instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of Player in this scene");

        
    }
    
    [Header("Player Info")]
    public bool isHost = false;
    public int playerIndex = -1;
    public int id;
    public string created_at;
    public string first_name;
    public string second_name;
    public string email_address;
    public string id_number;
    public int user_type_id = 99;    
    public string user_type;
    public int company_id;
    public string company_name;

    [Header("Body Parts")]
    public GameObject head;
    public GameObject l_Hand;
    public GameObject r_Hand;    

    [Header("Hand References")]
    [SerializeField] private GameObject defaultLHand;
    [SerializeField] private GameObject defaultRHand;
    public Finger l_Thumb;
    public Finger l_Index;
    public Finger l_Middle;
    public Finger l_Ring;
    public Finger l_Pinky;
    public Finger r_Thumb;
    public Finger r_Index;
    public Finger r_Middle;
    public Finger r_Ring;
    public Finger r_Pinky;

    [Header("OVR Parts")]
    public Transform tranQuest_Player;
    public Transform tranTracking_offset;
    public Transform tranOVRCameraRig;
    public Transform tranTrackingSpace;
    public Transform tranCenterEyeTracking;
    public AutoHandPlayer autoHandPlayer;
    public OVRScreenFade fader;

    [Header("Enable Camera After Delay")]
    public Camera cameraToEnable;
    public bool shouldEnableCameraAfterDelay = false;
    public float cameraDelayTime = 2.0f;

    //char[] charPPE = new char[] { '0', '0', '0', '0', '0' };

    private TutorialService tutorialService;

    public bool IsAdmin()
    {
         return user_type_id < 3;
         
    }

    IEnumerator refresh_I;
    bool shouldRefresh = false;

    public void StartRefresher()
    {
        //Debug.Log("Refreshing: " + "1");
        refresh_I = Refresh_I();
        StartCoroutine(refresh_I);
    }

    IEnumerator Refresh_I()
    {
        shouldRefresh = true;
        WWWForm www = new WWWForm();
        www.AddField("user_id", id);
        WaitForSeconds wait = new WaitForSeconds(5);
        while (shouldRefresh)
        {            
            yield return wait;
            yield return Api.instance.RefreshSession_I(www, SessionRefreshed, SessionNotRefreshed);
        }
    }

    private void Start()
    {
        //if (DevelopmentState.instance && !DevelopmentState.instance.IsTesting)
        //    ToggleMovement(false);
        setHandsEmissions(true);
        Realtime realTime = FindObjectOfType<Realtime>();
        if (realTime)
        {
            realTime.didConnectToRoom += SessionConnected;
            realTime.didDisconnectFromRoom += SessionDisconnected;
        }
        else
            Debug.LogError("Realtime Component not found");

        if (shouldEnableCameraAfterDelay)
            StartCoroutine(EnableCameraAfterDelay_I());
        else
            Debug.Log("Not delaying");

        StartCoroutine(WaitToFixHeadFollower_I());
    }

    private void SessionDisconnected(Realtime realtime)
    {
        if(CourseDetails.instance.courseStarted)
            StartRefresher();
    }

    private void SessionConnected(Realtime realtime)
    {
        shouldRefresh = false;
        if (refresh_I != null)
            StopCoroutine(refresh_I);
    }

    private void SessionRefreshed(string info)
    {
        if (shouldRefresh)
        {
            Invites.instance.ParseInvites(info);
        }
    }

    private void SessionNotRefreshed(string info)
    {
        Debug.Log("Session NOT Refreshed: " + info);
    }

    public void ToggleMovement(bool flag)
    {
        AutoHandPlayer ahp = GetComponentInChildren<AutoHandPlayer>();
        if (ahp)
        {
            ahp.useMovement = flag;
        }
    }

    public void Teleport(Transform target)
    {
        /*Player.instance.transform.position = target.position;
        Player.instance.transform.rotation = target.rotation;
        return;*/
        print("PLayer should teleport");
        if (tranQuest_Player && tranTracking_offset && tranCenterEyeTracking)
        {
            Vector3 nPosition;
            RaycastHit hit;
            if (false && Physics.Raycast(target.position, target.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                nPosition = hit.point;
                Debug.Log("Teleport Target Hit: " + hit.collider.gameObject.name + " (" + target.gameObject.name + ")");
            }
            else
                nPosition = target.position;

            //transform.position = nPosition;
            autoHandPlayer.SetPosition(nPosition);
            //Player.instance.transform.position = nPosition;            
            //Player.instance.transform.rotation = transform.rotation;
            tranTracking_offset.rotation = target.rotation;
            return;


            transform.position = nPosition;            
            tranTracking_offset.rotation = target.rotation;

            if (fader)
                fader.FadeIn();
        }
        else
            Debug.LogError("Do not have all required components for teleporting");
        //teleport_Offset.localPosition = Vector3.zero;
        //teleport_Offset.rotation = newPosition.rotation;
    }


    [ContextMenu("Go To working Area As Worker")]
    void Test_GoToWorkingSpace_AsWorker()
    {
        Teleport(WorkingAreas.instance.areas[0].tranStartPoint);
    }

    [ContextMenu("Go To working Area As Admin")]
    void Test_GoToWorkingSpace_AsAdmin()
    {
        Teleport(WorkingAreas.instance.areas[0].tranFacilitatorPoint);
    }

    IEnumerator EnableCameraAfterDelay_I()
    {
        cameraToEnable.enabled = false;
        Debug.Log("About to delay");
        yield return new WaitForSeconds(cameraDelayTime);
        Debug.Log("Finished delay");
        cameraToEnable.enabled = true;
    }

    IEnumerator WaitToFixHeadFollower_I()
    {
        HeadPhysicsFollower follower = GameObject.FindObjectOfType<HeadPhysicsFollower>();        
        while(!follower)
        {
            follower = GameObject.FindObjectOfType<HeadPhysicsFollower>();
            yield return null;
        }
        follower.GetComponent<Collider>().enabled = false;
    }

    public void setHandsEmissions(bool switchHand)
    {
        foreach(Material m in emmissionMaterial)
            if(switchHand==true)
                    m.EnableKeyword("_EMISSION");
            else
                    m.DisableKeyword("_EMISSION");

    }
}
