using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialService : MonoBehaviour
{
	[SerializeField] private List<TutorialStep> tutorialSteps;
	//[SerializeField] private bool startFromDifferentStep;
	//[SerializeField] private int startFromTutorialElementIndex;

	public GameObject[] objectsToHideAfteStart;

	public int startStep = 0;

	//private TutorialStep currentStep;

	private void Awake()
	{
		StartCoroutine(WaitToInitialize());
	}

	IEnumerator WaitToInitialize()
    {
		InitializeAllSteps();

        yield return null;
    }

    private void Start()
    {
		Start_Task();
    }

	public void ParseAll()
    {
		StartCoroutine(ParseAll_I());
    }

	IEnumerator ParseAll_I()
    {
		yield return new WaitForSeconds(2);
		foreach (TutorialStep t in tutorialSteps)
			t.ParseValues(t.ModelState());

	}

	IEnumerator WaitToParse(TutorialStep tut)
    {
		while (tut.ModelState() == "")
			yield return null;
		tut.ParseValues(tut.ModelState());
		Debug.Log("Broke Free: " + tut.gameObject.name);
    }

	//Maybe call this on network connect
	void InitializeAllSteps()
    {
		for (int i = 0; i < tutorialSteps.Count; i++)
		{
			if (i > 0)
				tutorialSteps[i].head = tutorialSteps[i - 1];
			if (i < tutorialSteps.Count - 1)
				tutorialSteps[i].tail = tutorialSteps[i + 1];
			tutorialSteps[i].InitializeStep();
		}
	}

	[ContextMenu("Start Part")]
    public void Start_Task()
	{
		Debug.Log("Starting this part");
		if (startStep >= 0 & startStep < tutorialSteps.Count)
		{
			Debug.Log("Beginning on step: " + startStep);
			tutorialSteps[startStep].StartStep();
		}
		else
			Debug.LogError("Start step out of range: " + startStep);

		foreach (GameObject go in objectsToHideAfteStart)
			go.SetActive(false);
	}

}