using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;
using System;
using TMPro;

public class CourseDetails : RealtimeComponent<CourseDetailsModel>
{
    public static CourseDetails instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of CourseDetails in this scene");
    }

    public string courseName;
    public string courseType;
    public int courseID;
    public bool isAlwaysTraining = false;
    public bool isAlwaysInAssessment = false;

    public int playerIndex = -1;

    [Header("UI Elements")]
    [SerializeField] GameObject pnlSessionType;
    [SerializeField] TMPro.TMP_Dropdown drpSessionType;
    [SerializeField] GameObject loading_SessionType;
    [SerializeField] TMPro.TMP_Text lblType;
    
    [SerializeField] GameObject pnlCourse;
    [SerializeField] TMPro.TMP_Dropdown drpCourse;
    [SerializeField] GameObject loading_Course;
    [SerializeField] TMPro.TMP_Text lblCourse;

    [SerializeField] GameObject btnStart;

    private CoursesResults courses;
    private Realtime realTime;

    public bool courseStarted = false;

    public void TurnOnAlwaysTraining()
    {
        isAlwaysTraining = true;
    }

    private void Start()
    {
        if(loading_Course)
            loading_Course.SetActive(false);
        if(loading_SessionType)
            loading_SessionType.SetActive(false);

        realTime = FindObjectOfType<Realtime>();
        if (realTime)
        {
            realTime.didConnectToRoom += ServerConnected;            
        }
        else
            Debug.LogError("Realtime not found in scene");        
    }

    protected override void OnRealtimeModelReplaced(CourseDetailsModel previousModel, CourseDetailsModel currentModel)
    {
        if(previousModel != null)
        {
            previousModel.courseNameDidChange -= DisplayNetworkValues;
            previousModel.courseTypeDidChange -= DisplayNetworkValues;
            previousModel.courseStartedDidChange -= StartCourse;
        }

        if(currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.courseName = "";
                currentModel.courseType = "";
                currentModel.courseStarted = false;
            }

            SetValues();

            currentModel.courseNameDidChange += DisplayNetworkValues;
            currentModel.courseTypeDidChange += DisplayNetworkValues;
            currentModel.courseStartedDidChange += StartCourse;
        }
    }
    
    private void DisplayNetworkValues(CourseDetailsModel model, string value)
    {
        SetValues();
    }

    void SetValues()
    {
        courseName = model.courseName;
        if(!CourseDetails.instance.courseStarted)
            courseType = model.courseType;
        courseID = model.courseID;
        if(lblCourse)
            lblCourse.text = "Course: " + model.courseName;
        if(lblType)
            lblType.text = model.courseType;
    }

    private void ServerConnected(Realtime realtime)
    {
        if (Player.instance.isHost)
        {
            drpCourse.onValueChanged.AddListener(CourseChanged);
            drpSessionType.onValueChanged.AddListener(TypeListened);
            StartCoroutine(WaitToloadCourses());
        }
        else
        {
            if(pnlCourse)
                pnlCourse.SetActive(false);
            if(pnlSessionType)
                pnlSessionType.SetActive(false);
            if(btnStart)
                btnStart.SetActive(false);
        }
    }

    IEnumerator WaitToloadCourses()
    {
        loading_Course.SetActive(true);
        loading_SessionType.SetActive(true);
        drpCourse.gameObject.SetActive(false);
        drpSessionType.gameObject.SetActive(false);
        while (Player.instance.company_id == 0)
        {
            Debug.Log("Still waiting for player to initialize");
            yield return null;
        }
        Api.instance.GetCourses(CoursesFound, CoursesNotFound, Player.instance.company_id);
        SetValues();
    }

    private void CoursesNotFound(string returnString)
    {
        Debug.LogError("Courses Not Found");
        Debug.Log(returnString);        
    }

    private void CoursesFound(string returnString)
    {
        courses = JsonUtility.FromJson<CoursesResults>(returnString);

        drpCourse.options.Clear();
        foreach(CourseResult course in courses.data)
        {
            drpCourse.options.Add(new TMP_Dropdown.OptionData() { text = course.name });
        }
        
        drpCourse.gameObject.SetActive(true);
        drpSessionType.gameObject.SetActive(true);
        loading_Course.SetActive(false);
        loading_SessionType.SetActive(false);
        StartCoroutine(WaitToRefreshValues());
    }

    IEnumerator WaitToRefreshValues()
    {
        while (drpCourse.options.Count == 0)
            yield return null;
        drpCourse.value = 1;
        drpSessionType.value = 1;
        drpCourse.value = 0;
        drpSessionType.value = 0;
    }

    private void CourseChanged(int arg0)
    {
        model.courseName = drpCourse.options[arg0].text;
        model.courseID = courses.data[arg0].id;
    }

    private void TypeListened(int arg0)
    {
        if(!CourseDetails.instance.courseStarted)
            model.courseType = drpSessionType.options[arg0].text; 
    }

    [ContextMenu("AssessmentMode")]
    private void TestAssessment(){
        TypeListened(1);
    }

    public bool GetCourseStarted
    {
        get{ return model.courseStarted; }
    }

    [ContextMenu("StartSession")]
    public void StartSession()
    {
        if(courseStarted)
            return;

        Debug.Log("Starting Session");
        Course[] courses = FindObjectsOfType<Course>();
        if (courses.Length > 0)
        {
            bool flag = false;
            foreach (Course c in courses)
            {
                if (c.courseName == this.courseName)
                {
                    model.courseStarted = true;
                    flag = true;
                    
                    break;
                }
            }

            if (!flag)
                Debug.LogError("No course with the name " + courseName + " could be found");
        }
        else
            Debug.LogError("There are no courses in this scene");
          
    }

    private void StartCourse(CourseDetailsModel model, bool value)
    {
        if(courseStarted)
            return;

        Debug.Log("Starting course: " + value);

        
        if (!value || model.courseName == "")
            return;

        Course[] courses = FindObjectsOfType<Course>();
        if (courses.Length > 0)
        {
            bool flag = false;
            foreach (Course c in courses)
            {
                if (c.courseName == this.courseName)
                {
                    flag = true;
                    c.StartCourse.Invoke();
                    courseStarted = true;
                    break;
                }
            }

            if (!flag)
                Debug.LogError("No course with the name " + courseName + " could be found");
        }
        else
            Debug.LogError("There are no courses in this scene");
    }

    public bool IsTraining()
    {
        if(isAlwaysInAssessment)
            return false;

        if(isAlwaysTraining)
            return true;
        return (courseType == "Training" || courseType == "Demonstration");        
    }

    [ContextMenu("Do training")]
    public void SetTraining()
    {
        if(!CourseDetails.instance.courseStarted)
            courseType = "Training";
    }

    [ContextMenu("Do assessment")]
    void SetAssessment()
    {
        if(!CourseDetails.instance.courseStarted)
            courseType = "Assessment";
    }

    public void SetDemonstration()
    {
        courseType = "Demonstration";
    }

    public void SetOffline()
    {
        courseType = "Offline";
    }

}

[System.Serializable]
public class CoursesResults
{
    public CourseResult[] data;
}

[System.Serializable]
public class CourseResult
{
    public string company;
    public int id;
    public string created_at;
    public string name;
    public string description;
    public int company_id;
}


