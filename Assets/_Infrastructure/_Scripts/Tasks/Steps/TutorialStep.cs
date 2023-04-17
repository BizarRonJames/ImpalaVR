using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using Normal.Realtime;
using Autohand;

public abstract class TutorialStep : RealtimeComponent<PartsModel>
{
	//[Header("Assessment")]
	//public char mock_Screw_Char;
	//public Grabbable[] mock_Grabbles;
	//public string[] mock_ColliderString;
	//public bool shouldScore = true;


	[Header("State")]
	public string statePrefix;
	public char[] stateValues;
	[HideInInspector] public bool initialized = false;
	public Sprite imgTopix;
	public string currentState;


	public bool completed = false;
	public bool shouldCheckIfPreviousStepCompleted = false;

	//public bool Initialized { get; set; }

	public delegate void OnCompleted(TutorialStep tutorialStep);

	public OnCompleted onCompleted;

	[HideInInspector] public TutorialStep head;
	public TutorialStep tail;

	public bool shouldAnimatInfo = true;
	[SerializeField] protected string headerText;
	[SerializeField] protected string subHeaderText;

	public UnityEvent enableStepEvent;
	public UnityEvent hideStepEvent;
	public UnityEvent initializeStepEvent;

	public TutorialService tService;

	public UnityEvent stepCompletedEvent;

	[Header("Meta")]
	public List<GameObject> objectsToHide = new List<GameObject>();

	[Header("Begining Step")]
	public bool preStepAction = false;
	public UnityEvent eventPreStep;

	[Header("Next Step")]
	public bool nextActionEvent = false;
	public UnityEvent eventNextStep;	
	
	
	public abstract void StartStep();

	[ContextMenu("Start Step")]
	void TestStart()
    {
		StartStep();
    }

	[ContextMenu("End Step")]
	void TestEnd()
    {
		StartCoroutine(Basic_Step_End());
    }

	public abstract void ParseValues(string value);

	public void SetValues()
	{
		string state = new string(stateValues);
		if (NetworkStatus.instance.IsConnected)
		{
			model.state = state;
		}
		else
		{
			currentState = state;
			ParseValues(state);
		}
	}

	protected abstract void ShowStatus();
	
	//private void ShowEquipmentRequired() => Information.instance.ShowEquipmentRequired(requiredItems);

	public abstract void InitializeStep();


	public void HideStep()
    {
		if (head)
			head.HideStep();
		if (hideStepEvent != null)
			hideStepEvent.Invoke();
		foreach (GameObject go in objectsToHide)
			if(go)
				go.SetActive(false);
	}
			
	public IEnumerator Basic_Step_Start(bool shouldAnimate = false)
    {
		//if (requiredPPE == "")
		//	requiredPPE = "00000";

		if (Information.instance != null)
		{
			Information.instance.ShowInstructions(headerText, subHeaderText, "", shouldAnimatInfo);

			yield return new WaitForSeconds(1);

			ShowStatus();

			if (preStepAction)
				eventPreStep.Invoke();

		}
	}

	public IEnumerator Basic_Step_End()
    {
		completed = true;
		//AssessmentChecker.instance.EndTask();

		Debug.Log("Ending Step: " + gameObject.name);
		StepEnded();
		///Information.instance.ShowTopix(null);

		yield return new WaitForSeconds(1);
		if (tail)
			tail.StartStep();
		else if (tService)
			tService.Start_Task();

		if (eventNextStep!=null)
			eventNextStep.Invoke();
	}

	public void ToggleColliders(GameObject go, bool flag)
	{
		foreach (Collider c in go.GetComponentsInChildren<Collider>(true))
			c.enabled = flag;
	}

	public abstract void SkipStep();

	public abstract void StepEnded();

	protected override void OnRealtimeModelReplaced(PartsModel previousModel, PartsModel currentModel)
	{
		if (previousModel != null)
		{
			previousModel.stateDidChange -= ParseState;
		}
		if (currentModel != null)
		{
			if (currentModel.isFreshModel)
			{
				currentModel.state = "";
			}
			ParseStep();

			currentModel.stateDidChange += ParseState;
		}
	}

    private void ParseState(PartsModel model, string value)
    {
		ParseStep();
    }

	public string ModelState()
    {
		return currentState;
    }

	void ParseStep()
    {
		string val = model.state;
		if(val!="" && val != currentState)
        {
			Debug.Log("Setting Network Values on: " + gameObject.name);
			currentState = val;
			ParseValues(val);
        }
    }

	[ContextMenu("View State")]
	void ViewState()
    {
		Debug.Log("State: " + model.state);
    }
}