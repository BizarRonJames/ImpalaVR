using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Menu))]
public class MainMenu : MonoBehaviour
{
    public Transform[] mainTransforms;
    public GameObject btnCreateSession;
    public Transform pnlMain;
    public float waitTime = 0.3f;

    private void Start()
    {
        //yield return new WaitForEndOfFrame();
        if (Player.instance.IsAdmin())
            GetComponent<Menu>().AddTransform(btnCreateSession.transform);
    }

    private void OnEnable()
    {
        //StartCoroutine(Start_I());
    }

    public void ShowMenu_Reports()
    {
        MenuManager.instance.ShowMenu_Reports();
    }

    public void ShowMenu_OfflineMode()
    {
        MenuManager.instance.ShowMenu_Offline();
    }

    public void ShowMenu_Exit()
    {
        MenuManager.instance.ShowMenu_Exit();
    }

    [ContextMenu("Show Lobby")]
    public void ShowMenu_Lobby()
    {
        MenuManager.instance.ShowMenu_Lobby();
    }
}
