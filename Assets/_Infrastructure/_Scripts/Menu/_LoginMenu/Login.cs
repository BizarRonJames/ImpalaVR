using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Menu))]
public class Login : MonoBehaviour
{
    [SerializeField] string placeholder_email = "riri@gmail.com";
    [SerializeField] string placeholder_password = "password";

    [SerializeField] float waitTime = 0.3f;
    [SerializeField] TMP_InputField txtEmail;
    [SerializeField] TMP_InputField txtPassword;

    [SerializeField] Color colBase;
    [SerializeField] Color colError;

    [SerializeField] Transform pnlMain;
    [SerializeField] GameObject loadingObject;
    [SerializeField] RectTransform[] mainRects;    
    
    [Header("Errors")]
    [SerializeField] GameObject lblInvalid;
    [SerializeField] GameObject lblError;


    private void Awake()
    {
        GetComponent<Menu>().autoStart = false;
    }

    private void Start()
    {
        colBase = txtEmail.image.color;

        string machineName = SystemInfo.deviceName.ToLower();
        if (machineName.Contains("urgmiff"))
            placeholder_email = "testee_3@gmail.com";
        else if (machineName.Contains("tg40") || machineName.Contains("xt1265"))
            placeholder_email = "test_1@gmail.com";
        else
            placeholder_email = "DebugUser@email.com";
        placeholder_password = "$upa$3cr3t5";

    }

    private void OnEnable()
    {
        StartCoroutine(Start_I());
    }

    IEnumerator Start_I()
    {
        while (!Player.instance)
            yield return null;

        txtEmail.text = "";
        txtPassword.text = "";
        loadingObject.SetActive(false);
        lblError.SetActive(false);
        lblInvalid.SetActive(false);

        /*ToggleMainElements(false);                
        yield return MenuManager.instance.ResizeObjects(new Transform[] { pnlMain }, waitTime, new Vector3(0, 1, 1), Vector3.one);
        yield return Show_Objects_I();*/
        yield return GetComponent<Menu>().FullAnimation();

        if (placeholder_email != "")
            txtEmail.text = placeholder_email;
        if (placeholder_password != "")
            txtPassword.text = placeholder_password;
    }

    IEnumerator LogIn_I(string email, string password)
    {
        lblError.SetActive(false);
        lblInvalid.SetActive(false);
        GetComponent<Menu>().ToggleObjects(false);
        yield return new WaitForSeconds(waitTime);
        loadingObject.SetActive(true);
        Api.instance.LogIn(LoggedIn, LogInError, email, password);
    }

    [ContextMenu("Log In")]
    public void LogIn()
    {
        Debug.Log("About to log in");
        string email = txtEmail.text;
        string password = txtPassword.text;
        StartCoroutine(LogIn_I(email, password));
    }

    public void LogInB()
    {
        Debug.Log("About to log in");
        string email = txtEmail.text;
        string password = txtPassword.text;
        StartCoroutine(LogIn_I("AlphaUser@email.com", password));
    }

    void LoggedIn(string info)
    {
        loadingObject.SetActive(false);
        LoginResults results = new LoginResults();
        try
        {
            results = JsonUtility.FromJson<LoginResults>(info);
            //Debug.Log(info);
        }
        catch (Exception ex) 
        {
            Debug.Log("Could not parse login information: " + ex.ToString());
        }

        if (results.status == "OK")
        {
            Player.instance.id = results.data.id;
            Player.instance.created_at = results.data.created_at;
            Player.instance.first_name = results.data.first_name;
            Player.instance.second_name = results.data.second_name;
            Player.instance.email_address = results.data.email_address;
            Player.instance.id_number = results.data.id_number;
            Player.instance.user_type_id = results.data.user_type_id;
            Player.instance.user_type = results.data.user_type;
            Player.instance.company_id = results.data.company_id;
            Player.instance.company_name = results.data.company_name;
            Player.instance.StartRefresher();
            MenuManager.instance.ShowMenu_Main();
        }
        else
        {
            lblInvalid.SetActive(true);
            StartCoroutine(GetComponent<Menu>().ShowMainObjects());
        }

    }

    void LogInError(string info)
    {
        Debug.Log("Error");
        Debug.Log(info);
        StartCoroutine(Error_I());
    }    

    IEnumerator Error_I()
    {
        loadingObject.SetActive(false);
        yield return new WaitForSeconds(waitTime * 2);
        lblError.SetActive(true);
        yield return new WaitForSeconds(4);
        lblError.SetActive(false);
        yield return GetComponent<Menu>().ShowMainObjects();
    }
   
}

[System.Serializable]
public class LoginResults
{
    public string status;
    public string details;
    public LoginData data;
}

[System.Serializable]
public class LoginData
{
    public int id;
    public string created_at;
    public string first_name;
    public string second_name;
    public string email_address;
    public string id_number;
    public int user_type_id;
    public int company_id;
    public string user_type;
    public string company_name;
}
