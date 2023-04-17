using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "MultipleChoiceQuestion", menuName = "ImplatsVR/MultipleChoiceQuestion", order = 0)]
public class MultipleChoiceQuestion : ScriptableObject {
    public string title;
    public string description;
    public string question;
    public string answer;
    public int hazardRating = 1;
    public bool isBrow = false;
    public int hazardScale = 0;
    public int browMeasurement = 0;
    
    public string [] questionsInOrder = new string[4];
    public string consequence;
    public int correctIndex;


    public static MultipleChoiceQuestion CreateEmpty(){
        MultipleChoiceQuestion empty = new MultipleChoiceQuestion();
        empty.question = "This is a Test Question " + Random.Range(0, 100).ToString();
        empty.answer = "Answer " + Random.Range(0, 100).ToString();
        empty.questionsInOrder[0] = empty.answer;
        empty.questionsInOrder[1] = "Incorrect Answer";
        empty.questionsInOrder[2] = "Incorrect Answer";
        empty.questionsInOrder[3] = "Incorrect Answer";
        empty.consequence = "Consequence";
        return empty;

    }

    public override string ToString(){
        string word = "";
        word += title + "%";
        word += hazardRating.ToString() + "%";

        for(int loop = 0; loop < questionsInOrder.Length; loop++){
            if(loop < questionsInOrder.Length - 1){
                word += questionsInOrder[loop] + "%";
            }
            else{
                word += questionsInOrder[loop];
            }
            
        }
        return word;
    }

    public MultipleChoiceQuestion DecodeString(string input){
        MultipleChoiceQuestion q = new MultipleChoiceQuestion();
        string [] questions = input.Split('%');
        q.title = questions[0];
        q.hazardRating = Int16.Parse(questions[1]);

        int questionIndex = 0;
        for(int loop = 2; loop <questions.Length; loop++){
            q.questionsInOrder[questionIndex] = questions[loop];
            questionIndex++;
        }

        return q;
    }

    public bool IsBlank(){
        if(question == "" || answer == ""){
            return true;
        }
        else{
            return false;
        }
    }

    
}
