using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class ScoreEventManager : MonoBehaviour
{
    #region SINGLETON_CODE
    public static ScoreEventManager instance;
    private void Awake() {
        instance = this;
    }
    #endregion

    [Header("Current Scores")]
    [SerializeField] int _scoreMCQ = 0;
    [SerializeField] int _scoreRating = 0;
    [SerializeField] int _scoreHazards = 0;

    [Header("Max Scores")]
    [SerializeField] int _maxScoreMCQ = 0;
    [SerializeField] int _maxScoreRating = 0;
    [SerializeField] int _maxScoreHazards = 0;
    [SerializeField] int _overallScore;


    [Header("Hazard Tracking")]
    [SerializeField] int _remainingHazards;

    [Header("Hazard Container")]
    [SerializeField] GameObject _hazardContainer;

    public int ScoreMCQ { get => _scoreMCQ; set => _scoreMCQ = value; }
    public int ScoreRating { get => _scoreRating; set => _scoreRating = value; }
    public int ScoreHazards { get => _scoreHazards; set => _scoreHazards = value; }
    public int MaxScoreMCQ { get => _maxScoreMCQ; set => _maxScoreMCQ = value; }
    public int MaxScoreRating { get => _maxScoreRating; set => _maxScoreRating = value; }
    public int MaxScoreHazards { get => _maxScoreHazards; set => _maxScoreHazards = value; }
    public int OverallScore { get => _overallScore; set => _overallScore = value; }
    public int RemainingHazards { get => _remainingHazards; set => _remainingHazards = value; }

    public event Action<int> OnRatingScoreUp;
    public void RatingScoreUp(int amount)
    {
        if (OnRatingScoreUp != null)
        {
            _scoreRating += amount;
            OnRatingScoreUp(amount);
        }
    }

    public event Action<int> OnMCQScoreUp;
    public void MCQScoreUp(int amount)
    {
        if (OnMCQScoreUp != null)
        {
            _scoreMCQ += amount;
            _remainingHazards -= 1;
            OnMCQScoreUp(amount);
        }
    }

    public event Action<int> OnHazardScoreUp;
    public void HazardScoreUp(int amount)
    {
        if (OnRatingScoreUp != null)
        {
            _scoreHazards += amount;
            OnHazardScoreUp(amount);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        int numberOfHazards = GetNumberOfHazards();
        RemainingHazards = numberOfHazards;
        _maxScoreMCQ = numberOfHazards;
        _maxScoreHazards = numberOfHazards;
        _maxScoreRating = numberOfHazards;
        _overallScore = _maxScoreHazards + _maxScoreMCQ + _maxScoreRating;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public int GetNumberOfHazards(){
        Hazard [] hazards = _hazardContainer.GetComponentsInChildren<Hazard>();
        return hazards.Length;
    }

    public int GetCurrentOverallScore(){
        return _scoreMCQ + _scoreHazards + _scoreRating;
    }
}
