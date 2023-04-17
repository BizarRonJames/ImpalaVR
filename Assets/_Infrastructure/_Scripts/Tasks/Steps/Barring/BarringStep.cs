using Autohand;
using System;
using System.Collections;
using UnityEngine;

public class BarringStep : TutorialStep
{

    private void Start()
    {

    }

    public override void InitializeStep()
    {
        // set up before the step starts (disabling colliders) 
        stateValues = new char[] { '0', (char) 0, (char) 0 };
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
            Information.instance.ShowInstructions(headerText, subHeaderText, "Progress: " + BarringManager.Instance.getMetricString());
        else if (stateValues[0] == '1')
            Information.instance.ShowInstructions(headerText, subHeaderText, "", false);
    }

    [ContextMenu("Sprayed")]
    public void BarredOne()
    {
        if (BarringManager.Instance.isComplete == true)
            BarredAll();
        else
        {
            stateValues[1] = (char)(stateValues[1] + 1);
            stateValues[2] = (char) BarringManager.Instance.totalBarringRocks;

            SetValues();
            ShowStatus();
        }

    }


    /**
     * 
     * this function should be called once all the dirt is cleaned
     * 
     */
    private void BarredAll()
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
