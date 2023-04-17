using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HAZARD_CONTROLLER : MonoBehaviour
{

    [Header("Hazard ClassType")]
    public Button RandomisedHazardsClassTypes;
    public Button HiddenHazardsClassTypes;
    public Button ShownHazardsClassTypes;

    [Header("Hazard States")]
    [Label("Random Highlight effect for all hazards")]
    public Button RandomisedHazardsStates;
    [Label("Remove Highlight effect from all hazards")]
    public Button HiddenHazardsStates;
    [Label("Add red highlight effect from All hazards")]
    public Button ShownHazardsStates;

    // Start is called before the first frame update
    void Start()
    {

    }
    [ContextMenu("Random ClassType's")]
    public void randomisedHazardsClassTypes()
    {
        RandomisedHazardsClassTypes.onClick.Invoke();
    }
    [ContextMenu("Hidden ClassType's")]
    public void hiddenHazardsClassTypes()
    {
        HiddenHazardsClassTypes.onClick.Invoke();
    }
    [ContextMenu("Shown ClassType's")]
    public void shownHazardsClassTypes()
    {
        ShownHazardsClassTypes.onClick.Invoke();
    }
    [ContextMenu("Random States's")]
    public void randomisedHazardsStates()
    {
        HazardManager.Instance.SetHazardsRandomState();


    }
    [ContextMenu("Hidden States's")]
    public void hiddenHazardsStates()
    {
        HazardManager.Instance.SetHazardsLowestState();
    }
    [ContextMenu("Shown States's")]
    public void shownHazardsStates()
    {
        HazardManager.Instance.SetHazardsHighestState();
    }
}
