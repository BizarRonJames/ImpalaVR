using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixWindowController : MonoBehaviour
{
    [SerializeField] Hazard _currentHazard;
    [SerializeField] MultipleChoiceQuestion _currentQuestion;
    [SerializeField] MatrixColumn [] _columns;

    // Start is called before the first frame update
    void Start()
    {
        _columns = GetComponentsInChildren<MatrixColumn>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void OnEnable() {
        HazardMenuEventSystem.instance.OnMenuOpen += OnMenuOpen;
        HazardMenuEventSystem.instance.OnBrowMenuOpen += OnBrowMenuOpen;
    }
    private void OnDisable() {
        HazardMenuEventSystem.instance.OnMenuOpen -= OnMenuOpen;
        HazardMenuEventSystem.instance.OnBrowMenuOpen -= OnBrowMenuOpen;
    }

    private void OnMenuOpen(Hazard hazard){
        Debug.Log("Hazard is " + hazard.gameObject.name);
        _currentHazard = hazard;
        _currentQuestion = hazard.GetRandomMCQ();


    }
    private void OnBrowMenuOpen(Hazard hazard){
        _currentHazard = hazard;
        _currentQuestion = hazard.GetRandomMCQ();

    }


    public void SetHazardPriority(int value){
        //set hazard priority things
        _currentHazard.SetHazardCorrect(true);
        if(value==0)
        {
            //_currentHazard.SetHazardHighlightState("None");

            //SetGameLayerRecursive(hazard, 0);
        }
        else if(value < 8)
        {
            //_currentHazard.SetHazardHighlightState("Low");
            if(_currentQuestion.hazardRating == 1){
                ScoreEventManager.instance.ScoreRating += 1;
            }
            //SetGameLayerRecursive(hazard, 21);
        }
        else if (value < 20)
        {
            //_currentHazard.SetHazardHighlightState("Medium");
            if(_currentQuestion.hazardRating == 2){
                ScoreEventManager.instance.ScoreRating += 1;
            }
        }

        else if (value <=36)
        {
            //_currentHazard.SetHazardHighlightState("High");
            if(_currentQuestion.hazardRating == 3){
                ScoreEventManager.instance.ScoreRating += 1;
            }
        }

        _currentHazard.tag = "Untagged";
        _currentHazard.GetComponent<Collider>().enabled = false;
    }
    


    
}
