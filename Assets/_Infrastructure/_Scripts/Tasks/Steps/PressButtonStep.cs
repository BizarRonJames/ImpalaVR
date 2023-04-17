using Autohand;
using System.Collections;
using UnityEngine;

public class PressButtonStep : TutorialStep
{
    [SerializeField] private GameObject button, sprayers;

    private void Start()
    {
        if (!button)
            button = GetComponentInChildren<PhysicsGadgetButton>().gameObject;
        if (!button)
            throw new System.NullReferenceException("Button not found");
        button.GetComponent<Collider>().enabled = false;
        sprayers.SetActive(false);
    }

    public override void InitializeStep()
    {
        // set up before the step starts (disabling colliders) 
        stateValues = new char[] { '0', '0' };
    }

    public override void ParseValues(string value)
    {
        //network values 
        if (value.Length >= 2)
        {
            if (value[0] == '0')
                button.GetComponent<Collider>().enabled = false;

            else if(value[0] == '1')
                button.GetComponent<Collider>().enabled = true;

            if (value[1] == '0')
                sprayers.SetActive(false);

            else if (value[1] == '1')
                sprayers.SetActive(true);
        }
    }

    public override void SkipStep()
    {
        throw new System.NotImplementedException();
    }

    public override void StartStep()
    {
        StartCoroutine(StartStep_Enum());
    }
    IEnumerator StartStep_Enum() 
    {
        yield return Basic_Step_Start();
        stateValues[0] = '1';
        SetValues();

        while (stateValues[1] != '1')
            yield return null;

        yield return Basic_Step_End();
    }
   
    public override void StepEnded()
    {
        //does not execute over the network
    }

    protected override void ShowStatus()
    {
        Debug.Log($"{stateValues[0]} , {stateValues[1]}");
        if (stateValues[1] == '0')
            Information.instance.ShowInstructions(headerText, subHeaderText, "Press the fire suppression button");
        else if (stateValues[1] == '1')
            Information.instance.ShowInstructions(headerText, subHeaderText, "",false);
    }

    [ContextMenu("Suppress Fire")]
    public void ButtonPressed()
    {
        stateValues[0] = '0';
        stateValues[1] = '1';

        SetValues();
        ShowStatus();
    }
}
