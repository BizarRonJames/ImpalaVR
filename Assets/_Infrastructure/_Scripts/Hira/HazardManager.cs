using Autohand;
using Normal.Realtime;
using Normal.Realtime.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
public class HazardManager : RealtimeComponent<HazardListModel>
{
    [SerializeField]
    private GameObject[] hazards;

    [SerializeField]
    private GameObject[] miniHazards;

    [SerializeField]
    public int viewIndex = 0;

    private int numHazards;
    private const int maxNumPlayers = 4;
    
    [SerializeField]
    public static HazardManager Instance;

    volatile bool initialized = false;
    void Start()
    {


        this.realtime.didConnectToRoom += OnRealtimeConnected;


    }
    void Awake()
    {


        //singleton check
        if (Instance != null && Instance != this)
        {
            print("Warning!: multiple instances of HazardManager");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // Notify us when Realtime successfully connects to the room


    }
    /// <summary>
    /// when the called by the player once they are disconnected to the normcore room
    /// </summary>
    public void OnRealtimeDisconnected(Realtime realtime)
    {

    }


    [ContextMenu("Intialize Data model")]
    public void privateAwake()
    {
        // save a unique Key into cache thatll be used to reconnect in future if the user disconnects
        /// TO-DO !!!
        /// add a check for the key in the User Exist Array 
        PlayerPrefs.SetString("IMAPLA_loginKey", "unique key here");
        hazards = GameObject.FindGameObjectsWithTag("Hazard");
        miniHazards = GameObject.FindGameObjectsWithTag("MiniHazard");
        numHazards = hazards.Length;


        GameObject[] tempHazards = new GameObject[hazards.Length];
        int ind = 0;
        foreach (GameObject mh in miniHazards)
        {
            Hazard h = mh.GetComponent<Hazard>();
            if (h != null && h.enabled == true)
            {
                print(ind);
                //tempHazards[ind] = mh;
                ind++;
               //print(mh);
            }
        }
        miniHazards = tempHazards;
        // initialize the data model
        if (model.hazards.Count <= 0)
        {

            print("Adding local Hazard States to model");
            for (int j = 0; j < maxNumPlayers; j++)
            {
                //model.hazards.modelAdded += HazardAddedEvent;
                for (int i = 0; i < numHazards; i++)
                {
                    print("adding hazards");
                    HazardModel newUserHazard = new HazardModel();

                    newUserHazard.stateDidChange += StateChangeEvent;
                    newUserHazard.classTypeDidChange += ClassChangeEvent;
                    newUserHazard.classType = 0;

                    newUserHazard.state = 0;
                    newUserHazard.index = i;
                    newUserHazard.ownerID = -1;

                    model.hazards.Add(newUserHazard);

                }
            }

        }

        initialized = true;
        print("+------------------------------------------------------------------+");

        SetHazardsLowestPriority();
    }
    public void OnRealtimeConnected(Realtime realtime)
    {
        if (!initialized) privateAwake();
    }

    [ContextMenu("switch view")]
    public void switchView()
    {
        viewIndex++;
        if (viewIndex >= 4) viewIndex = 0;
        refreshView();

    }
    [ContextMenu("PRINT STATE")]
    public void printState()
    {

        print("PRINTING STATE =======================");
        int id = 0;
        int index = 0;
        bool mustPrint = true;
        foreach(HazardModel h in model.hazards)
        {
            if (mustPrint)
            {
                print("User " + id + "'s State");
                mustPrint = false;
            }

            string outMessage = "Hazard-" + index + "'s State: " + h.state + ", Class: " + h.classType;
            index++;

            //reset index for each user
            if (index >= numHazards)
            {
                index = 0;
                id++;
                mustPrint = true;
            }
            print(outMessage);
        }

        print("END PRINTING  ========================");
    }
    /// <summary>
    /// when the called by the player once they are connected to the normcore room
    /// </summary>


    public void setViewerView(int index)
    {
        viewIndex= index;
        refreshView();
    }

    public void resetViewerView()
    {
        viewIndex = 0;
        refreshView();
    }

    /// <summary>
    /// setter function to change the state of a hazard at a specific index. Called when the Network receives an update
    /// </summary>
    /// <param name="hazardState">
    /// the new state we want to set the hazard to 
    /// </param>
    /// <param name="index">
    /// the index we can find the local hazard at
    /// </param>
    private void setLocalHazardStates(int hazardStateIndex)
    {

        int hazardState = model.hazards[hazardStateIndex].state;
        int index = hazardStateIndex % numHazards;
        if (hazardState == 0) // null state
        {
            hazards[index].layer = 0;
            miniHazards[index].layer = 0;
        }
        else if(hazardState == 1) // low priority state
        {

            hazards[index].layer = 21;
            miniHazards[index].layer = 21;
        }
        else if (hazardState == 2) // medium priority state
        {

            hazards[index].layer = 22;
            miniHazards[index].layer = 22;
        }
        else if (hazardState == 3) // high priority state
        {

            hazards[index].layer = 23;
            miniHazards[index].layer = 23;
        }
        else if (hazardState == 4) // highlight state
        {

            hazards[index].layer = 4;
            miniHazards[index].layer = 4;
        }
        else // error 
        {
            throw new Exception("invalid hazard state received from Network");
        }


        int hazardClass = model.hazards[hazardStateIndex].classType;
        Transform[] childs = hazards[index].gameObject.GetComponentsInChildren<Transform>();

        int icount=0;
        foreach (var child in childs)
        {

            if(icount==hazardClass)
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);

            icount++;
        }


    }
    public void refreshView()
    {
        int index = NetworkedPlayer.instance.playerIndex;
        if(viewIndex != -1) index = viewIndex;

        for (int i = 0; i < numHazards; i++)
        {

        
            setLocalHazardStates(i + (viewIndex * numHazards) );

        }
    }

    [ContextMenu("Highlight Object A")]
    public void highlightA()
    {
        HazardHoverEvent(0);
    }


    /// <summary>
    /// sets a hazard to be highlighted and deactivates the last activated hazard
    /// </summary>
    /// <param name="index">
    /// the hazard to be activated
    /// </param>
    public void HazardHoverEvent(int index)
    {
        print("Hover Event Was triggered: ");
        //hazards[index].GetComponent<Hazard>().highLighted = true;

        model.hazards[index + numHazards].state +=1 ;

    }


    /// <summary>
    /// check if a user has already been added to the global state
    /// </summary>
    /// <param name="playerId">
    /// the users ID
    /// </param>
    /// <returns></returns>
    private bool UserExistsInArray(int playerId)
    {
        for (int i = 0; i < maxNumPlayers; i++)
        {
            if (model.hazards[i * numHazards].ownerID == playerId) return true;
        }
        return false;
    }
    


    private void StateChangeEvent(HazardModel hm, int value)
    {

        print("+---------------------------------------------------------+");
        print("|_______ State Change EVENT TRIGGERED ON NETWORK _________|");
        printState();
        print("+---------------------------------------------------------+");

        //refreshView();
    }
    private void ClassChangeEvent(HazardModel hm, int value)
    {

        print("+---------------------------------------------------------+");
        print("|_______ Class Change EVENT TRIGGERED ON NETWORK _________|");
        print("+---------------------------------------------------------+");

        //refreshView();
    }


    protected override void OnRealtimeModelReplaced(HazardListModel previousModel, HazardListModel currentModel)
    {

        StartCoroutine(OnRealtimeModelReplacedLOCK( previousModel, currentModel));
       
    }

    IEnumerator OnRealtimeModelReplacedLOCK(HazardListModel previousModel, HazardListModel currentModel)
    {
        // wait for the user 
        while(!initialized && !(currentModel != null && currentModel.hazards.Count <= 0))
        {
            if(initialized && !(currentModel != null && currentModel.hazards.Count <= 0))
            {
                yield break;
            }
            yield return null;
        }

        print("+------------------------------------------------------------------+");
        print("|_______________________ Model Was Replaced _______________________|");

            if (previousModel != null)
            {
                //previousModel.hazards.modelAdded -= HazardAddedEvent;
                if (previousModel.hazards.Count > 0)
                {

                    print("previous Model========/");
                    print("previous count: " + previousModel.hazards.Count);



                    foreach (HazardModel h in previousModel.hazards)
                    {


                        h.stateDidChange -= StateChangeEvent;
                        h.classTypeDidChange -= ClassChangeEvent;

                }

                }
            }

            if (currentModel != null)
            {

                print("current Model========/");
                print("current count: " + currentModel.hazards.Count);
                if (currentModel.hazards.Count > 0)
                {

                    foreach (HazardModel h in currentModel.hazards)
                    {
                        h.stateDidChange += StateChangeEvent;
                        h.classTypeDidChange += ClassChangeEvent;

                    }

                    

                    refreshView();
                }


            }


            print("+------------------------------------------------------------------+");
        
    }



    public void SetHazardsLowestState()
    {
        foreach (HazardModel hm in model.hazards)
        {
            hm.state = 4;
        }
    }

    public void SetHazardsHighestState()
    {
        foreach (HazardModel hm in model.hazards)
        {
            hm.state = 0;
        }
    }

    public void SetHazardsRandomState()
    {
        foreach(HazardModel hm in model.hazards)
        {
            hm.state = UnityEngine.Random.Range(0, 4);
        }

    }


    /// <summary>
    /// sets all the hazards and the mini model counter parts to their lowest possible hazard priority = NONE (first child in Hierarchy)
    /// </summary>
    [ContextMenu("hide Hazards")]
    public void SetHazardsLowestPriority()
    {
        StartCoroutine(lowestPriority());
    }

    /// <summary>
    /// sets all the hazards and the mini model counter parts to their lowest possible hazard priority = Last child in Hieracrchy
    /// </summary>

    [ContextMenu("Show Hazards")]
    public void SetHazardsHighestPriority()
    {
        StartCoroutine(fadeAnimation1());
    }
    /// <summary>
    /// sets all the hazards and the mini model counter parts to a random hazard priority 
    /// </summary>

    [ContextMenu("Random Hazards")]
    public void SetHazardsRandomPriority()
    {
        StartCoroutine(fadeAnimation3());
      

    }

    private IEnumerator fadeAnimation1()
    {

        yield return Player.instance.fader.Fade(0f, 1f);

        try
        {
            int j = 0;
            foreach (var hazard in hazards)
            {

                for (int i = 0; i < hazard.transform.childCount; ++i)
                {
                    Transform child = hazard.transform.GetChild(i);
                    //Transform minichild = miniHazards[j].transform.GetChild(i);


                    if (i == hazard.transform.childCount - 1)
                    {
                        child.gameObject.SetActive(true);

                        int index = 0;
                        if (NetworkedPlayer.instance) index = NetworkedPlayer.instance.playerIndex;

                        if (index >= 0 && index < 4)
                            model.hazards[index * numHazards + j].classType = i;
                        //minichild.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                        //minichild.gameObject.SetActive(false);
                    }
                }
                j++;

            }
        }
        catch (Exception e)
        {

            Debug.LogException(e);
        }
        yield return Player.instance.fader.Fade(1f, 0f);
    }
    private IEnumerator lowestPriority()
    {

        yield return Player.instance.fader.Fade(0f, 1f); 
        try
        {
            int j = 0;
            foreach (var hazard in hazards)
            {
                //Hazard[] ChildrenObjects = hazard.GetComponentsInChildren<Hazard>();
                //print("Hazard: " + j);
                for (int i = 0; i < hazard.transform.childCount; ++i)
                {
                    Transform child = hazard.transform.GetChild(i);

                    //Transform minichild = miniHazards[j].transform.GetChild(i);
                    //print(minichild);
                    //execute functionality of child transform here

                    if (i == 0)
                    {
                        child.gameObject.SetActive(true);

                        int index = 0;
                        if (NetworkedPlayer.instance) index = NetworkedPlayer.instance.playerIndex;

                        if (index >= 0 && index < 4)
                            model.hazards[index * numHazards + j].classType = i;
                        //minichild.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                        //minichild.gameObject.SetActive(false);
                    }
                }
                j++;

            }
        }
        catch (Exception e)
        {

            Debug.LogException(e);
        }
        yield return Player.instance.fader.Fade(1f, 0f);

    }

    private IEnumerator fadeAnimation3()
    {

        yield return Player.instance.fader.Fade(0f, 1f);
        try
        {
            int j = 0;
            foreach (var hazard in hazards)
            {
                int trnsfChlcC = hazard.transform.childCount;

                int Rnd = UnityEngine.Random.Range(0, trnsfChlcC);

                for (int i = 0; i < trnsfChlcC; ++i)
                {
                    Transform child = hazard.transform.GetChild(i);
                    //Transform minichild = miniHazards[j].transform.GetChild(i);

                    if (i == Rnd)
                    {
                        child.gameObject.SetActive(true);

                        int index = 0;
                        if (NetworkedPlayer.instance) index = NetworkedPlayer.instance.playerIndex;

                        if (index >= 0 && index < 4)
                            model.hazards[index * numHazards + j].classType = i;
                        //minichild.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                        //minichild.gameObject.SetActive(false);
                    }
                }
                j++;



            }

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        yield return Player.instance.fader.Fade(1f, 0f);
    }




}
