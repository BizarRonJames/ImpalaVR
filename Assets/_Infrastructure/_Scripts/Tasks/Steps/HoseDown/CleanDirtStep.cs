using Autohand;
using System;
using System.Collections;
using UnityEngine;

public class CleanDirtStep : TutorialStep
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
            Information.instance.ShowInstructions(headerText, subHeaderText, "Progress: " + HoseDownManager.Instance.getMetricString());
        else if (stateValues[0] == '1')
            Information.instance.ShowInstructions(headerText, subHeaderText, "",false);
    }


    /**
     * 
     * this function should be called for each dirt that gets washed down
     * 
     */
    [ContextMenu("Attach Hose")]
    public void CleanedOne()
    {
        if (HoseDownManager.Instance.isWashed == true) 
            CleanedAll();
        else
        {
            stateValues[1] = (char) ( (int) stateValues[1] + 1 );
            stateValues[2] = (char) HoseDownManager.Instance.totalDirt;
            //print(HoseDownManager.Instance.getMetricString());
            //print(HoseDownManager.Instance.getMetricFloat());


            SetValues();
            ShowStatus();
        }

    }


    /**
     * 
     * this function should be called once all the dirt is cleaned
     * 
     */
    private void CleanedAll()
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
