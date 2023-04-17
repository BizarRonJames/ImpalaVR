using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class QuestionGenerator : MonoBehaviour
{
    [SerializeField] TextAsset jsonData;
    [SerializeField] string filePath = "Assets/Questions";

    [System.Serializable]
    public class Question
    {
        public string unityName;
        public string hazardTitle;
        public string description;
        public string riskRating;
        public string mcq;
        public string correctAnswer;
        public string consequence;

        public int ConvertRiskRating(string rating)
        {
            switch (rating)
            {
                default:
                    return 1;
                case "Minor":
                    return 1;
                case "Major":
                    return 2;
                case "Critical":
                    return 2;
                case "Very Critical":
                    return 3;
            }
        }

        public void GenerateMCQobject(string path)
        {
            MultipleChoiceQuestion question = ScriptableObject.CreateInstance<MultipleChoiceQuestion>();
            question.title = this.hazardTitle;
            question.answer = this.correctAnswer;
            question.hazardRating = ConvertRiskRating(this.riskRating);
            question.question = this.mcq;
            question.consequence = this.consequence;
            if (this.unityName != "none")
            {
                AssetDatabase.CreateAsset(question, path + "/" + this.unityName + ".asset");
            }
            else
            {
                AssetDatabase.CreateAsset(question, path + "/" + this.hazardTitle + ".asset");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    [System.Serializable]
    public class QuestionList
    {
        public Question[] question;


        public int ConvertRiskRating(string rating)
        {
            switch (rating)
            {
                default:
                    return 1;
                case "Minor":
                    return 1;
                case "Major":
                    return 2;
                case "Critical":
                    return 2;
                case "Very Critical":
                    return 3;
            }
        }

        public void GenerateQuestions(string path)
        {


            for (int loop = 0; loop < question.Length; loop++)
            {
                MultipleChoiceQuestion question = ScriptableObject.CreateInstance<MultipleChoiceQuestion>();
                question.title = this.question[loop].hazardTitle;
                question.answer = this.question[loop].correctAnswer;
                question.hazardRating = ConvertRiskRating(this.question[loop].riskRating);
                question.question = this.question[loop].mcq;
                question.consequence = this.question[loop].consequence;
                if (this.question[loop].unityName != "none")
                {
                    AssetDatabase.CreateAsset(question, path + "/" + this.question[loop].unityName + loop.ToString() + ".asset");
                }
                else
                {
                    AssetDatabase.CreateAsset(question, path + "/" + this.question[loop].hazardTitle + loop.ToString() + ".asset");
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

        }
    }


    public QuestionList questionList = new QuestionList();
    // Start is called before the first frame update
    void Start()
    {
        questionList = JsonUtility.FromJson<QuestionList>(jsonData.text);
        questionList.GenerateQuestions(filePath);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
