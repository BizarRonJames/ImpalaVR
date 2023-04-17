using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Api : MonoBehaviour
{
    public static Api instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of Api in this scene");
    }

    public string api_key = "GzUkgy2cfHkIS82UJp1w";
    public string url = "http://127.0.0.1:1234/api/";
    public delegate void returnDelegate(string returnString);


    IEnumerator Api_call(string uri, WWWForm form, returnDelegate returnFunction, returnDelegate errorFunction = null)
    {
        Debug.Log($"API Call: ${uri}");
        UnityWebRequest www = UnityWebRequest.Post(uri, form);
        Debug.Log("uri: " + uri);
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
        if (www.result != UnityWebRequest.Result.Success)
        {
            print("fail");
            if (errorFunction != null)
                errorFunction(www.error);
            Debug.LogError(www.error);
        }
        else
        {
            print("success");
            returnFunction(www.downloadHandler.text);
        }
    }

    public WWWForm blankForm()
    {
        WWWForm form = new WWWForm();
        form.AddField("key", api_key);
        return form;
    }

    #region Users
    public void LogIn(returnDelegate returnFunction, returnDelegate errorFunction, string email, string password)
    {
        string uri = url + "users/login";        
        WWWForm form = blankForm();
        form.AddField("email_address", email);
        form.AddField("password", password);
        StartCoroutine(Api_call(uri, form, returnFunction, errorFunction));

    }
    #endregion

    #region Sessions
    public void CreateSession(returnDelegate returnFunction, returnDelegate errorFunction, int user_id)
    {
        string uri = url + "sessions/insert";
        WWWForm form = blankForm();
        form.AddField("user_id", user_id);
        StartCoroutine(Api_call(uri, form, returnFunction, errorFunction));
    }

    public void SetCourse(returnDelegate returnFunction, returnDelegate errorFunction, int session_id, string session_key, int course_id)
    {
        string uri = url + "sessions/set_course";
        WWWForm form = blankForm();
        form.AddField("id", session_id);
        form.AddField("session_key", session_key);
        form.AddField("course_id", course_id);        
        StartCoroutine(Api_call(uri, form, returnFunction, errorFunction));
    }

    public void InviteUser(returnDelegate returnFunction, returnDelegate errorFunction, int session_id, string session_key, int user_id)
    {
        string uri = url + "sessions/invite";
        WWWForm form = blankForm();
        form.AddField("session_id", session_id);
        form.AddField("session_key", session_key);
        form.AddField("user_id", user_id);
        StartCoroutine(Api_call(uri, form, returnFunction, errorFunction));
    }

    public void JoinSession(returnDelegate returnFunction, returnDelegate errorFunction, int session_id, string session_key, int user_id)
    {
        string uri = url + "sessions/join";
        WWWForm form = blankForm();
        form.AddField("session_id", session_id);
        form.AddField("session_key", session_key);
        form.AddField("user_id", user_id);
        StartCoroutine(Api_call(uri, form, returnFunction, errorFunction));
    }

    public IEnumerator RefreshSession_I(WWWForm form, returnDelegate returnFunction, returnDelegate errorFunction = null)
    {
        string uri = url + "sessions/refresh";
        yield return Api_call(uri, form, returnFunction, errorFunction);
    }

    public IEnumerator GetOnlineUsers(int company_ID, returnDelegate returnFunction, returnDelegate errorFunction = null)
    {
        string uri = url + "sessions/online_users";
        WWWForm form = blankForm();
        form.AddField("session_id", SessionDetails.instance.session_id);
        form.AddField("company_id", company_ID);
        yield return Api_call(uri, form, returnFunction, errorFunction);
    }

    public void InviteUser(int user_id, returnDelegate returnFunction, returnDelegate errorFunction = null)
    {
        string uri = url + "sessions/invite";
        WWWForm form = blankForm();
        form.AddField("session_id", SessionDetails.instance.session_id);
        form.AddField("session_key", SessionDetails.instance.session_key);
        form.AddField("user_id", user_id);
        StartCoroutine(Api_call(uri, form, returnFunction, errorFunction));
    }

    public IEnumerator CheckForInvites(int user_id, returnDelegate returnFunction, returnDelegate errorFunction = null)
    {
        string uri = url + "sessions/get_invites";
        WWWForm form = blankForm();
        form.AddField("user_id", user_id);
        yield return Api_call(uri, form, returnFunction, errorFunction);
    }
    #endregion

    #region Courses
    public void GetCourses(returnDelegate returnFunction, returnDelegate errorFunction, int company_id)
    {
        string uri = url + "courses/list";
        WWWForm form = blankForm();
        form.AddField("company_id", Player.instance.company_id);
        StartCoroutine(Api_call(uri, form, returnFunction, errorFunction));
    }
    #endregion

    


}
