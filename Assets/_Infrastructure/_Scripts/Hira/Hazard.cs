using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public enum HazardHighlightState
{
    none = 0,
    hover = 1,
    low = 2,
    medium = 3,
    high = 4
}

public enum HazardCorrectionState{
    incorrect = 0,
    correct = 1
}

public class Hazard : MonoBehaviour
{
    
    // used to remeber the last active priority level for when an object is highlighed
    private int _lastHighlightLayer;
    private bool _highLighted;

    public int RskRating_Actual;
    private int RskRating_UserAnswer;
    [SerializeField] bool _isBrow = false;
    [SerializeField] HazardHighlightState highlightState = HazardHighlightState.none;

    [SerializeField] MultipleChoiceQuestion[] _questions = new MultipleChoiceQuestion[1];

    [Header("Hazard Properties")]
    [SerializeField] string _hazardName;
    [SerializeField] bool _isTarp = false;
    [SerializeField] int _userTarpRating = 0;
    [SerializeField] int _correctTarpRating = 1;
    [SerializeField] bool _isIdentified = false;

    [SerializeField] GameObject correctHazard;
    [SerializeField] GameObject incorrectHazard;


    private void Awake()
    {
        //AddBlankQuestion();
    }
    public MultipleChoiceQuestion GetRandomMCQ()
    {
        if (Questions.Length == 0)
        {
            Debug.Log("No Questions inputted");
            return null;
        }
        else
        {
            return Questions[0];
        }



    }

    public void SetHazardState(string data){
        if(data != "0" && data != "1"){
            data = "0";
        }
        
        if(data == "0"){
            SetHazardCorrect(false);
            IsIdentified = false;
        }
        else{
            SetHazardCorrect(true);
            IsIdentified = true;
        }
    }
    public string GetHazardData(){
        string data = "0";
        if(IsIdentified){
            data = "1";
        }
        else{
            data = "0";
        }
        return data;
    }
    public void SetHazardHighlightState(string state)
    {
        switch (state)
        {
            default:
                highlightState = HazardHighlightState.none;
                SetGameLayerRecursive(this.gameObject, 0);
                hazardState = "None";
                break;
            case "Hover":
                highlightState = HazardHighlightState.hover;
                SetGameLayerRecursive(this.gameObject, 20);
                break;
            case "Low":
                highlightState = HazardHighlightState.low;
                SetGameLayerRecursive(this.gameObject, 21);
                break;
            case "Medium":
                highlightState = HazardHighlightState.medium;
                SetGameLayerRecursive(this.gameObject, 22);
                break;
            case "High":
                highlightState = HazardHighlightState.medium;
                SetGameLayerRecursive(this.gameObject, 23);
                break;

        }
    }

    public void SetHazardHighlightState(HazardHighlightState state)
    {
        switch ((int)state)
        {
            default:
                highlightState = HazardHighlightState.none;
                SetGameLayerRecursive(this.gameObject, 0);
                hazardState = "None";
                break;
            case 1:
                highlightState = HazardHighlightState.hover;
                SetGameLayerRecursive(this.gameObject, 20);
                break;
            case 2:
                highlightState = HazardHighlightState.low;
                SetGameLayerRecursive(this.gameObject, 21);
                break;
            case 3:
                highlightState = HazardHighlightState.medium;
                SetGameLayerRecursive(this.gameObject, 22);
                break;
            case 4:
                highlightState = HazardHighlightState.medium;
                SetGameLayerRecursive(this.gameObject, 23);
                break;

        }
    }
    private void SetGameLayerRecursive(GameObject _go, int _layer)
    {
        _go.layer = _layer;
        foreach (Transform child in _go.transform)
        {
            child.gameObject.layer = _layer;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetGameLayerRecursive(child.gameObject, _layer);

        }
    }
    public GameObject[] HideShowObjects;
    /// <summary>
    /// the property that controls the highlight effect for an idividual hazard
    /// </summary>
    public bool highLighted
    {
        get { return _highLighted; }
        set
        {
            // check if there is a change
            if (value != _highLighted)
            {
                if (value == true)
                {
                    _lastHighlightLayer = this.gameObject.layer;
                    _highLighted = value;
                    this.gameObject.layer = 20;
                }
                else if (value == false)
                {

                    this.gameObject.layer = _lastHighlightLayer;
                    _highLighted = value;
                }

                else throw new Exception("error when changing hazard highlight state");


            }
        }
    }

    /// <summary>
    /// User will set this to the danger level the think the hazard is
    /// Hazard States: Low, Medium, High, None,
    /// </summary>
    private string _hazardState;
    public string hazardState
    {
        get { return _hazardState; }
        set
        {

            if (value == "Low")
            {
                this.gameObject.layer = 21;

                SetGameLayerRecursive(this.gameObject, 21);
            }
            else if (value == "Medium")
                this.gameObject.layer = 22;

            else if (value == "High")
            {
                this.gameObject.layer = 23;
            }
            else if (value == "None")
            {
                SetGameLayerRecursive(this.gameObject, 0);
            }
            else
            {
                throw new Exception("Hazard received an invalid Input");
            }
            _hazardState = value;
        }

    }




    /// <summary>
    /// to identify which hazard this is. This is set by the manager
    /// </summary>
    private int _index;
    public int index
    {
        get { return _index; }
        set { _index = value; }
    }

    public MultipleChoiceQuestion[] Questions { get => _questions; set => _questions = value; }
    public bool IsBrow { get => _isBrow; set => _isBrow = value; }
    public bool IsBrow1 { get => _isBrow; set => _isBrow = value; }
    public int CorrectTarpRating { get => _correctTarpRating; set => _correctTarpRating = value; }
    public int UserTarpRating { get => _userTarpRating; set => _userTarpRating = value; }
    public bool IsTarp { get => _isTarp; set => _isTarp = value; }
    public bool IsIdentified { get => _isIdentified; set => _isIdentified = value; }

    public void Start()
    {
        if(_hazardName == ""){
            _hazardName = this.gameObject.name;
        }
        SetHazardCorrect(false);
        hazardState = "None";
        _lastHighlightLayer = 0;



    }

    public void SetHazardCorrect(bool isCorrect)
    {
        if (correctHazard == null || incorrectHazard == null)
        {
            return;
        }

        if (isCorrect)
        {
            correctHazard.SetActive(true);
            incorrectHazard.SetActive(false);

        }
        else
        {
            correctHazard.SetActive(false);
            incorrectHazard.SetActive(true);
        }
    }

    private void AddBlankQuestion()
    {
        if (Questions.Length == 0)
        {
            List<MultipleChoiceQuestion> questions = new List<MultipleChoiceQuestion>();
            MultipleChoiceQuestion q = MultipleChoiceQuestion.CreateEmpty();
            questions.Add(q);
            Questions = questions.ToArray();
        }

        if (Questions.Length == 1 && Questions[0] == null)
        {
            Questions[0] = MultipleChoiceQuestion.CreateEmpty();
        }

    }



}
