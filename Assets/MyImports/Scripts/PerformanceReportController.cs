using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PerformanceReportController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreMCQText;
    [SerializeField] TextMeshProUGUI _scoreRatingText;
    [SerializeField] TextMeshProUGUI _scoreHazardText;
    [SerializeField] TextMeshProUGUI _scoreOverallText;

    [Header("Progress Bars")]
    [SerializeField] Image _barMCQ;
    [SerializeField] Image _barRating;
    [SerializeField] Image _barHazard;
    [SerializeField] Image _barOveralll;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        //HazardMenuEventSystem.instance.OnReportMenuOpen += OnReportMenuOpen;
        HazardMenuEventSystem.instance.OnNextMenu += OnNextWindow;
        HazardMenuEventSystem.instance.OnReportMenuOpen += OnReportMenuOpen;
    }
    private void OnDisable()
    {
        //HazardMenuEventSystem.instance.OnReportMenuOpen -= OnReportMenuOpen;
        HazardMenuEventSystem.instance.OnNextMenu -= OnNextWindow;
        HazardMenuEventSystem.instance.OnReportMenuOpen -= OnReportMenuOpen;
    }
    private void OnReportMenuOpen()
    {
        InitializeReportInfo();
    }

    private void OnNextWindow(){
        InitializeReportInfo();
    }



    public void InitializeReportInfo()
    {
        ScoreEventManager scoreSystem = ScoreEventManager.instance;
        _scoreMCQText.text = "MCQ Score: " + scoreSystem.ScoreMCQ.ToString() + "/" + scoreSystem.MaxScoreMCQ.ToString();
        _scoreHazardText.text = "Hazard ID Score: " + scoreSystem.ScoreHazards.ToString() + "/" + scoreSystem.MaxScoreHazards.ToString();
        _scoreRatingText.text = "Hazard Rating Score: " + scoreSystem.ScoreRating.ToString() + "/" + scoreSystem.MaxScoreHazards.ToString();
        float percent = (((float) scoreSystem.GetCurrentOverallScore() / (float) scoreSystem.OverallScore) * 100f);
        percent = Mathf.Round(percent * 100f) / 100f;
        _scoreOverallText.text = "Overall Score: " + percent.ToString() + "%";

        float ratioMCQ = (float)scoreSystem.ScoreMCQ / (float)scoreSystem.MaxScoreMCQ;
        _barMCQ.fillAmount = ratioMCQ;

        float ratioHazard = (float)scoreSystem.ScoreHazards / (float)scoreSystem.MaxScoreHazards;
        Debug.Log(ratioHazard + " Hazard Ratio");
        _barHazard.fillAmount = ratioHazard;

        float ratioRating = (float)scoreSystem.ScoreRating / (float)scoreSystem.MaxScoreRating;
        _barRating.fillAmount = ratioRating;

        float overallRatio = (float) scoreSystem.GetCurrentOverallScore() / (float) scoreSystem.OverallScore;
        _barOveralll.fillAmount = overallRatio;


    }
}
