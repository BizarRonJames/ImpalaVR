using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingAreas : MonoBehaviour
{
    public static WorkingAreas instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of WorkingArea in this scene");
    }

    [Header("Areas")]
    public WorkingArea_Battery[] areas;

    [Header("Placement Points - Areas")]
    public Transform[] placementAreas;

    [Header("Placement Points - Scry")]
    public Transform scryParent;
    public Transform[] placementScry;

    [ContextMenu("PlaceAreas")]
    public void PlaceAreas()
    {
        for(int i = 1; i < areas.Length; i++)
        {
            areas[i].transform.position = placementAreas[i-1].position;

            areas[i].scryArea.transform.position = placementScry[i-1].position;
            areas[i].scryArea.transform.parent= scryParent;
            areas[i].gameObject.name = "Working Area_" + i.ToString();
            areas[i].scryArea.gameObject.name = "Scry Area_" + i.ToString();
        }
    }
}
