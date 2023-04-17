using Autohand;
using System;
using System.Collections;
using UnityEngine;

public class DemarcationSprayStep : TutorialStep
{

    private void Start()
    {
        
    }

    public override void InitializeStep()
    {
        // set up before the step starts (disabling colliders) 
        stateValues = new char[] { '0',(char) 0, (char) 0};
    }

    public override void ParseValues(string value)
    {
        //network values 
        
    }

    public override void StartStep()
    {
        StartCoroutine(StartStep_Enum());
    }
    IEnumerator StartStep_Enum() 
    {
        yield return Basic_Step_Start();
        stateValues[0] = '0';
        SetValues();

        while (stateValues[0] != '1')
            yield return null;

        yield return Basic_Step_End();
    }
   
    public override void StepEnded()
    {
        //does not execute over the network
    }

    protected override void ShowStatus()
    {
        Debug.Log($"{stateValues[0]}");
        if (stateValues[0] == '0')
            Information.instance.ShowInstructions(headerText, subHeaderText, "Progress: " + DemarcationManager.Instance.getMetricString());
        else if (stateValues[0] == '1')
            Information.instance.ShowInstructions(headerText, subHeaderText, "",false);
    }

    [ContextMenu("Sprayed")]
    public void SprayedOne()
    {
        if (DemarcationManager.Instance.isComplete == true)
            SprayedAll();
        else
        {
            stateValues[1] = (char) ( (int) stateValues[1] + 1 );
            stateValues[2] = (char)DemarcationManager.Instance.totalDemarcations;

            SetValues();
            ShowStatus();
        }

    }


    /**
     * 
     * this function should be called once all the dirt is cleaned
     * 
     */
    private void SprayedAll()
    {
        stateValues[0] = '1';

        SetValues();
        ShowStatus();
    }

    public override void SkipStep()
    {
        throw new System.NotImplementedException();
    }
}
