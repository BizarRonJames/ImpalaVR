using Normal.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedPlayer : RealtimeComponent<NetworkedPlayerModel>
{
    public static NetworkedPlayer instance;

    [Header("Player Info")]
    public bool isHost = false;
    public int playerIndex = -1;
    public int id = 99;
    public string first_name;
    public string second_name;
    public int user_type_id;
    public string user_type;
    public int voice_channel;

    [Header("Player Limbs")]
    [SerializeField] GameObject head;
    [SerializeField] GameObject l_hand;
    [SerializeField] GameObject r_hand;
    //[SerializeField] SkinnedMeshRenderer[] skinnedToDisable;
    //[SerializeField] MeshRenderer[] meshesToDisable;
    [SerializeField] GameObject[] objectsToDisableOnOwnership;

    [Header("VR IK References")]
    public Transform refBody;
    public Transform refHead;
    public Transform refHandL;
    public Transform refHandR;

    [Header("UI Pointers")]
    public GameObject[] uiPointers;

    [Header("PPE")]
    public GameObject glove_l;
    public GameObject glove_l_o;
    public GameObject glove_r;
    public GameObject glove_r_o;

    public GameObject[] helmet_on;
    public GameObject helmet_off;

    public GameObject safety_goggles;
    public GameObject HeadLamp;

    bool _isSelf = false;

    public bool IsSelf
    {
        get{ return _isSelf; }
    }

    protected override void OnRealtimeModelReplaced(NetworkedPlayerModel previousModel, NetworkedPlayerModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.isHostDidChange -= HostStatusChanged;
            previousModel.idDidChange -= IDChanged;
            previousModel.first_nameDidChange -= FirstNameChanged;
            previousModel.second_nameDidChange -= SecondNameChanged;
            previousModel.user_type_idDidChange -= UserTypeIDChanged;
            previousModel.user_typeDidChange -= UserTypeDidChange;
            previousModel.player_indexDidChange -= IndexChanged;
            previousModel.voiceChannelDidChange -= ChangeVoiceChannel;
            previousModel.ppeDidChange -= PPEDidChange;
            currentModel.headlampSwitchDidChange -= HeadLampDidChange;
        }
        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                SetValues();
            }

            SetValues();
            currentModel.isHostDidChange += HostStatusChanged;
            currentModel.idDidChange += IDChanged;
            currentModel.first_nameDidChange += FirstNameChanged;
            currentModel.second_nameDidChange += SecondNameChanged;
            currentModel.user_type_idDidChange += UserTypeIDChanged;
            currentModel.user_typeDidChange += UserTypeDidChange;
            currentModel.player_indexDidChange += IndexChanged;
            currentModel.voiceChannelDidChange += ChangeVoiceChannel;
            currentModel.ppeDidChange += PPEDidChange;
            currentModel.headlampSwitchDidChange += HeadLampDidChange;
        }
    }

    private void HeadLampDidChange(NetworkedPlayerModel model, bool value)
    {
        HeadLamp.SetActive(value);
       // print("NETWORKEDHEADLAMP");
    }

    public void localHeadLampChange( bool value)
    {
        model.headlampSwitch = value;
    }

    private void PPEDidChange(NetworkedPlayerModel model, string value)
    {
        ParsePPE(value);
    }

    private void ChangeVoiceChannel(NetworkedPlayerModel model, int value)
    {
        Set_ChangeVoiceChannel(value);
    }

    void Set_ChangeVoiceChannel(int i)
    {
        //Debug.Log("Changing Audio: " + i);
        voice_channel = i;
        if (this != NetworkedPlayer.instance)
            return;
        //Debug.Log("You own this model");
        NetworkedPlayer[] players = FindObjectsOfType<NetworkedPlayer>();
        foreach (NetworkedPlayer p in players)
        {
            if (p != this)
            {
                AudioSource aud = p.head.GetComponentInParent<AudioSource>();
                if (aud)
                {
                    if (p.voice_channel != voice_channel)
                        aud.enabled = false;
                    else
                        aud.enabled = true;
                }
            }
        }
    }

    private void IndexChanged(NetworkedPlayerModel model, int value)
    {
        Debug.Log("Setting Index Value: " + value);
        playerIndex = value;

        StartCoroutine(SetPlayerIndex_I(value));
    }

    private void UserTypeDidChange(NetworkedPlayerModel model, string value)
    {
        user_type = value;
    }

    private void UserTypeIDChanged(NetworkedPlayerModel model, int value)
    {
        user_type_id = value;
    }

    private void SecondNameChanged(NetworkedPlayerModel model, string value)
    {
        second_name = value;
    }

    private void FirstNameChanged(NetworkedPlayerModel model, string value)
    {
        first_name = value;
        gameObject.name = "Network_Player:_" + value;
    }

    private void IDChanged(NetworkedPlayerModel model, int value)
    {
        if(CourseDetails.instance.playerIndex<0)
            id = value;
    }

    private void HostStatusChanged(NetworkedPlayerModel model, bool value)
    {
        isHost = value;
    }

    private void Start()
    {
        foreach (GameObject go in objectsToDisableOnOwnership)
            go.SetActive(false);

        if(PlayerIndex.instance)
            PlayerIndex.instance.AddPlayer(this);
        StartCoroutine(InitializeNetwork());
        
    }

    IEnumerator InitializeNetwork()
    {
        yield return null;
        if (GetComponent<RealtimeAvatar>().isOwnedLocallyInHierarchy)
        {
            _isSelf = true;
            instance = this;

            ToggleMeshesColliders(head, false);
            ToggleMeshesColliders(l_hand, false);
            ToggleMeshesColliders(r_hand, false);
            
            model.isHost = Player.instance.isHost;
            model.id = Player.instance.id;
            model.created_at = Player.instance.created_at;
            model.first_name = Player.instance.first_name;
            model.second_name = Player.instance.second_name;
            model.email_address = Player.instance.email_address;
            model.id_number = Player.instance.id_number;
            model.user_type_id = Player.instance.user_type_id;
            model.user_type = Player.instance.user_type;
            model.company_id = Player.instance.company_id;
            model.company_name = Player.instance.company_name;

            Set_ChangeVoiceChannel(0);

            if (LobbySpots.instance && LobbySpots.instance.gameObject.activeSelf && LobbySpots.instance.shouldJoinLobby && SessionDetails.instance.player_index < 0 && !CourseDetails.instance.GetCourseStarted)
                StartCoroutine(WaitToFindSpot());
            else if (SessionDetails.instance.player_index >= 0 && CourseDetails.instance.GetCourseStarted)
            {

                int i = SessionDetails.instance.player_index;
                Debug.Log("Reconnecting to Room " + i);

            }
                
            foreach (GameObject go in uiPointers)
                go.SetActive(false);            
        }
        else
        {
            foreach (GameObject go in objectsToDisableOnOwnership)
                go.SetActive(true);
        }
    }

    public void AssignToScry(int i)
    {
        if(i>=0 && i< Scry.instance.faces.Length)
            Scry.instance.faces[i].AssignPlayer(refBody, refHead, refHandL, refHandR);
    }

    void ToggleMeshesColliders(GameObject go, bool flag)
    {
        foreach (Collider col in go.GetComponentsInChildren<Collider>())
            col.enabled = flag;
        foreach (MeshRenderer ren in go.GetComponentsInChildren<MeshRenderer>())
            ren.enabled = flag;
        foreach (SkinnedMeshRenderer skinned in go.GetComponentsInChildren<SkinnedMeshRenderer>())
            skinned.enabled = flag;
    }

    IEnumerator WaitToFindSpot()
    {        

        NetworkedPlayer[] players = FindObjectsOfType<NetworkedPlayer>();
        bool flag = false;

        while (!flag)
        {
            foreach (NetworkedPlayer p in players)
            {
                Debug.Log("Other Player ID: " + p.playerIndex);
                if (p.playerIndex == 0)
                    flag = true;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1);

        flag = false;

        for (int i = 0; i < LobbySpots.instance.spots.Length; i++)
        {
            flag = false;
            foreach (NetworkedPlayer np in players)
            {
                if (!flag && np!=this)
                {
                    if (np.playerIndex == i)
                    {
                        flag = true;
                        //break;
                    }
                }
            }
            if (!flag)
            {
                //playerIndex = i;                
                model.player_index = i;
                Player.instance.playerIndex = i;
                SessionDetails.instance.player_index = i;

                break;
            }
        }

        if (!flag)
        {
            LobbySpot spot = LobbySpots.instance.GetOpenSpot(playerIndex);
            if (spot)
            {
                spot.TakeSpot(this);
                if(CourseDetails.instance.playerIndex<0)
                    model.player_index = spot.index;
                else
                    model.player_index = CourseDetails.instance.playerIndex;
                PlayerPrefs.SetInt("Index", spot.index);
            }
            else
                Disconnect("No open lobby spots found");
        }
        else
            Disconnect("No open lobby spots found");
    }

    public void SetPlayerIndex(int i)
    {
        Debug.Log("Set player index: " + i);
        if (i > 0)
        {
            //StartCoroutine(SetPlayerIndex_I(i));
            playerIndex = i;

            model.player_index = i;

        }
        else if (i == 0)
        {
            StartCoroutine(SetPlayerIndex_I(i));
            playerIndex = i;
            model.player_index = i;
        }
        else
        {
            Disconnect("Player index out of range");
        }    
    }

    public void SetPlayerIndexS(int i){
        model.player_index = i;
    }

    IEnumerator SetPlayerIndex_I(int i)
    {
        playerIndex = i;
        Player.instance.playerIndex = i;

        SessionDetails.instance.player_index = i;

        yield return new WaitForSeconds(1);        
        LobbySpot spot = LobbySpots.instance.GetOpenSpot(playerIndex);
        if (spot)
        {
            spot.TakeSpot(this);
            //model.player_index = spot.index;
            Debug.Log("Setting Index: " + spot.index);
        }
        else
            Disconnect("No open lobby spots found");
    }



    void Disconnect(string message)
    {
        Debug.LogError("Disconnecting: " + message);
        FindObjectOfType<Realtime>()?.Disconnect();
    }

    private void OnDestroy()
    {
        if(PlayerIndex.instance)
            PlayerIndex.instance.RemovePlayer(this);
    }

    void SetValues()
    {
        isHost = model.isHost;
        id = model.id;
        first_name = model.first_name;
        second_name = model.second_name;
        user_type_id = model.user_type_id;
        user_type = model.user_type;
        playerIndex = model.player_index;

        model.voiceChannel = 0;        
        gameObject.name = "Network_Player:_" + model.first_name;
    }

    public void SetVoiceChannel(int i)
    {
        model.voiceChannel = i
;    }

    [ContextMenu("Go to working area")]
    void TestTeleportToWorkingArea()
    {
        WorkingAreas.instance.areas[NetworkedPlayer.instance.playerIndex].Claim();
    }

    public void EquipPPE(string s)
    {
        model.ppe = s;
    }

    void ParsePPE(string s)
    {
        //Debug.Log("Parsing PPE: " + s);
        if (s.Length >= 5)
        {
            char glasses = s[3];
            if (safety_goggles)
            {
                //Debug.Log("toggling SafetyGlasses: " + (glasses == '1'));
                safety_goggles.SetActive(glasses == '1');
            }
            /*else
                Debug.LogError("PPE: Goggles not set");*/

            char helmet = s[2];
            if (helmet_off)
            {
                //Debug.Log("toggling helmet: " + (helmet == '1'));
                helmet_off.SetActive(helmet != '1');
                foreach (GameObject go in helmet_on)
                    go.SetActive(helmet == '1');
            }
            /*else
                Debug.LogError("PPE: Helmet not set");*/

            char glove = s[4];
            if (NetworkedPlayer.instance!=this && glove_l && glove_r)
            {
                glove_l.SetActive(glove == '1');
                glove_r.SetActive(glove == '1');

                glove_l_o.SetActive(glove != '1');
                glove_r_o.SetActive(glove != '1');
            }
            /*else if (!glove_l || !glove_r)
                Debug.LogError("PPE: Gloves not set");*/
        }
        else
            ParsePPE("00000");
    }
}
