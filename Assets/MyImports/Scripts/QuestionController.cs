using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour
{
    [SerializeField] Hazard _lastHazard;
    [SerializeField] MultipleChoiceQuestion _lastQuestion;
    [SerializeField] MultipleChoiceQuestion[] _questions;
    [SerializeField] MultipleChoiceQuestion[] _questionPool;
    [SerializeField] GameObject _hazardContainer;


    [Header("Question Text")]
    [SerializeField] TextMeshProUGUI[] _answerTexts;
    [SerializeField] TextMeshProUGUI _questionText;
    [SerializeField] TextMeshProUGUI _header;

    [Header("Consequence Menu and Text")]
    [SerializeField] TextMeshProUGUI _consequenceText;

    [Header("Hover Buttons")]
    [SerializeField] HoverSelect[] _answerButtons;

    public Hazard LastHazard { get => _lastHazard; set => _lastHazard = value; }
    public MultipleChoiceQuestion LastQuestion { get => _lastQuestion; set => _lastQuestion = value; }



    private void OnEnable() {
        HazardMenuEventSystem.instance.OnMenuOpen += OnMenuOpen;
        HazardMenuEventSystem.instance.OnBrowMenuOpen += OnBrowMenuOpen;

        HazardMenuEventSystem.instance.OnTarpMenuOpen += OnTarpMenuOpen;
    }


    private void OnDisable() {
        HazardMenuEventSystem.instance.OnMenuOpen -= OnMenuOpen;
        HazardMenuEventSystem.instance.OnBrowMenuOpen -= OnBrowMenuOpen;
        HazardMenuEventSystem.instance.OnTarpMenuOpen -= OnTarpMenuOpen;
    }
    // Start is called before the first frame update
    void Start()
    {
        PopulateLocalQuestionsArray();
        PopulateQuestionPool();
        LastQuestion = MultipleChoiceQuestion.CreateEmpty();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMenuOpen(Hazard hazard){
        _lastHazard = hazard;
        MultipleChoiceQuestion question = hazard.GetRandomMCQ();
        if(question == null) question = _questionPool[UnityEngine.Random.Range(0, _questionPool.Length)];
        _lastQuestion = question;
        InitializeNewQuestionText(question);
    }

    private void OnBrowMenuOpen(Hazard hazard){
        _lastHazard = hazard;
        MultipleChoiceQuestion question = hazard.GetRandomMCQ();
        if(question == null) question = _questionPool[UnityEngine.Random.Range(0, _questionPool.Length)];
        _lastQuestion = question;
        SetBrowQuestion(question);
    }

    private void OnTarpMenuOpen(Hazard hazard){
        _lastHazard = hazard;
        MultipleChoiceQuestion question = hazard.GetRandomMCQ();
        if(question == null) question = _questionPool[UnityEngine.Random.Range(0, _questionPool.Length)];
        _lastQuestion = question;
        InitializeNewQuestionText(question);
    }

    [ContextMenu("Question Pool")]
    private void PopulateQuestionPool(){
        List<MultipleChoiceQuestion> questions = new List<MultipleChoiceQuestion>();
        foreach(MultipleChoiceQuestion question in _questions){
            if(!question.IsBlank() && question != null && !question.isBrow){
                questions.Add(question);
            }
        }

        _questionPool = questions.ToArray();
    }
    [ContextMenu("Populate Questions")]
    private void PopulateLocalQuestionsArray()
    {
        List<MultipleChoiceQuestion> questions = _questions.ToList();

        Hazard[] hazards = _hazardContainer.GetComponentsInChildren<Hazard>();
        foreach (Hazard item in hazards)
        {
            if (item != null)
            {
                foreach (MultipleChoiceQuestion question in item.Questions)
                {
                    if (question == null)
                    {
                        continue;
                    }
                    if (!questions.Contains(question))
                    {
                        questions.Add(question);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }



        _questions = questions.ToArray();
    }
    private string AnswerIndex(int index)
    {
        switch (index)
        {
            default:
                return "A - ";
            case 0:
                return "A - ";
            case 1:
                return "B - ";
            case 2:
                return "C - ";
            case 3:
                return "D - ";
        }
    }
    public MultipleChoiceQuestion[] GenerateAlternativeAnswers(MultipleChoiceQuestion question)
    {
        MultipleChoiceQuestion[] tempArr = _questionPool.Where(e => e != question).ToArray();

        if (tempArr.Length < 3)
        {
            Debug.Log("Not enough Questions Initialised");
            return null;
        }

        var random = new System.Random();
        return tempArr.OrderBy(e => random.Next()).Take(3).ToArray();

    }

    public void SetCorrectAnswer(int index)
    {
        if (index > 3 || index < 0)
        {
            index = 0;
        }

        for (int loop = 0; loop < _answerButtons.Length; loop++)
        {
            if (loop == index)
            {
                _answerButtons[loop].select.RemoveAllListeners();
                _answerButtons[loop].select.AddListener(() => SelectCorrectAnswser());
                
            }
            else
            {
                _answerButtons[loop].select.RemoveAllListeners();
                _answerButtons[loop].select.AddListener(() => SelectIncorrectAnswer());
            }
        }
    }


    public void InitializeQuestion(MultipleChoiceQuestion question)
    {
        _questionText.text = question.question;
        for (int loop = 0; loop < 4; loop++)
        {
            _answerTexts[loop].text = question.questionsInOrder[loop];
        }
    }

    public MultipleChoiceQuestion [] GetValidQuestions(){
        List<MultipleChoiceQuestion> questions = new List<MultipleChoiceQuestion>();
        foreach(MultipleChoiceQuestion q in questions){
            if(!q.isBrow){
                questions.Add(q);
            }
            else{
                continue;
            }
        }

        return questions.ToArray();
    }
    public void InitializeNewQuestionText(MultipleChoiceQuestion question)
    {
        _consequenceText.text = question.consequence;
        if (question == null)
        {
            question = _questions[UnityEngine.Random.Range(0, _questions.Length - 1)];
        }
        //_header.text = question.title;

        _lastQuestion = question;

        _questionText.text = question.question;

        //Choose a random position for the correct answer to the question
        int correctAnswerIndex = UnityEngine.Random.Range(0, 4);
        question.questionsInOrder[correctAnswerIndex] = question.answer;
        question.correctIndex = correctAnswerIndex;
        _answerTexts[correctAnswerIndex].text = AnswerIndex(correctAnswerIndex) + question.answer + "*";
        SetCorrectAnswer(correctAnswerIndex);


        int remainingAnswerIndex = 0;
        MultipleChoiceQuestion[] remainingAnswers = GenerateAlternativeAnswers(question);

        for (int loop = 0; loop < 4; loop++)
        {
            if (loop != correctAnswerIndex)
            {
                _answerTexts[loop].text = AnswerIndex(loop) + remainingAnswers[remainingAnswerIndex].answer;
                question.questionsInOrder[loop] = remainingAnswers[remainingAnswerIndex].answer;
                remainingAnswerIndex++;
            }
            else
            {
                continue;
            }
        }



    }

    

    public void SelectCorrectAnswser()
    {
        //add to score here
        ScoreEventManager.instance.ScoreMCQ += 1;
        HazardMenuEventSystem.instance.MenuClose();
        /*
        if(LastQuestion.isBrow){
            
            HazardMenuEventSystem.instance.MatrixWindowOpen();
        }
        else{
            HazardMenuEventSystem.instance.ReportMenuOpen();
        }
        */
        
    }

    private int FindCorrectAnswerIndex(MultipleChoiceQuestion question)
    {
        for (int loop = 0; loop < _answerTexts.Length; loop++)
        {
            if (_answerTexts[loop].text == question.answer)
            {
                return loop;
            }
        }
        return -1;
    }
    public void SelectIncorrectAnswer()
    {
        HazardMenuEventSystem.instance.MenuClose();

        /*
        if(LastQuestion.isBrow){
            Debug.Log("Brow found");
            HazardMenuEventSystem.instance.MatrixWindowOpen();
        }
        else{
            HazardMenuEventSystem.instance.NextWindow();
        }
        */

        

    }
    public void DisableConsequenceMenu()
    {
        HazardMenuEventSystem.instance.MenuClose();
    }

    private void SetBrowQuestion(MultipleChoiceQuestion question){
        SetCorrectAnswer(question.hazardScale);
        _answerTexts[0].text = "A - Minor";
        _answerTexts[1].text = "B - Major";
        _answerTexts[2].text = "C - Critical";
        _answerTexts[3].text = "D - Very Critical";

        _questionText.text = "This brow is " + question.browMeasurement.ToString() + "cm in length, what is the correct risk rating associated with it?";

    }


}
