using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;
using TMPro;
using UnityEngine.UI;
public class QuestionManager : MonoBehaviour
{

    public static QuestionManager instance;

    private void Awake()
    {
        instance = this;
    }
    [SerializeField] Hazard _lastHazard;
    [SerializeField] MultipleChoiceQuestion _lastQuestion;
    [SerializeField] MultipleChoiceQuestion[] _questions;
    [SerializeField] GameObject _hazardContainer;


    [Header("Question Text")]
    [SerializeField] TextMeshProUGUI[] _answerTexts;
    [SerializeField] TextMeshProUGUI _questionText;
    [SerializeField] Text _header;

    [Header("Consequence Menu and Text")]
    [SerializeField] Canvas _consequenceMenu;
    [SerializeField] TextMeshProUGUI _consequenceText;

    [Header("Hover Buttons")]
    [SerializeField] HoverSelect[] _answerButtons;

    public Hazard LastHazard { get => _lastHazard; set => _lastHazard = value; }
    public MultipleChoiceQuestion LastQuestion { get => _lastQuestion; set => _lastQuestion = value; }

    // Start is called before the first frame update
    void Start()
    {
        InitializeLocalQuestionsArray();
        LastQuestion = MultipleChoiceQuestion.CreateEmpty();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void InitializeLocalQuestionsArray()
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
        MultipleChoiceQuestion[] tempArr = _questions.Where(e => e != question).ToArray();

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
                _answerButtons[loop].select.AddListener(delegate { SelectCorrectAnswser(); });
                _answerButtons[loop].select.RemoveListener(delegate { SelectIncorrectAnswer(); });
            }
            else
            {
                _answerButtons[loop].select.AddListener(delegate { SelectIncorrectAnswer(); });
                _answerButtons[loop].select.RemoveListener(delegate { SelectCorrectAnswser(); });
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


    public void InitializeNewQuestion(MultipleChoiceQuestion question)
    {
        if (question == null)
        {
            question = _questions[UnityEngine.Random.Range(0, _questions.Length - 1)];
        }
        _header.text = question.title;

        _lastQuestion = question;

        _questionText.text = question.question;

        //Choose a random position for the correct answer to the question
        int correctAnswerIndex = UnityEngine.Random.Range(0, 4);
        question.questionsInOrder[correctAnswerIndex] = question.answer;
        question.correctIndex = correctAnswerIndex;
        _answerTexts[correctAnswerIndex].text = AnswerIndex(correctAnswerIndex) + question.answer;
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
        if (_lastHazard == null)
        {
            return;
        }
        _consequenceText.text = _lastQuestion.consequence;
        _consequenceMenu.enabled = true;

        _consequenceMenu.transform.position = transform.position;
        _consequenceMenu.transform.rotation = transform.rotation;

    }
    public void DisableConsequenceMenu()
    {
        _consequenceText.text = "";
        _consequenceMenu.enabled = false;
    }
}

