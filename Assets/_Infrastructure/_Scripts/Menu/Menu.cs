using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    public bool autoStart = true;
    public Transform pnlMain;
    public Transform[] mainTransforms;
    public UnityEvent event_PostShown;
    public GameObject pnlInvites;
    float waitTime = 0.3f;

    private void OnEnable()
    {
        StartCoroutine(OnEnable_I());
    }

    IEnumerator OnEnable_I()
    {
        while (!Player.instance)
            yield return null;
        while (!MenuManager.instance)
            yield return null;

        SetWaitTime();
        if (autoStart)
            StartCoroutine(FullAnimation());
    }

    public void AddTransform(Transform tran)
    {
        List<Transform> trans = new List<Transform>();
        foreach (Transform t in mainTransforms)
            trans.Add(t);
        trans.Add(tran);
        mainTransforms = trans.ToArray();
    }

    public IEnumerator FullAnimation(bool shouldSwipe = true)
    {
        ToggleObjects(false);      
        yield return MenuManager.instance.ResizeObjects(new Transform[] { pnlMain }, waitTime, new Vector3(0, 1, 1), Vector3.one, shouldSwipe);
        yield return ShowMainObjects();
        if (event_PostShown != null)        
            event_PostShown.Invoke();
        
    }

    public IEnumerator ShowMainObjects()
    {
        yield return new WaitForSeconds(waitTime);
        ToggleObjects(true);
        yield return MenuManager.instance.RotateObjects(mainTransforms, waitTime, new Vector3(90, 0, 0), Vector3.zero);
        ShowInvites();
    }

    void SetWaitTime()
    {
        waitTime = MenuManager.instance.animationTime;
    }

    public void ToggleObjects(bool flag)
    {
        foreach (Transform tran in mainTransforms)
            tran.gameObject.SetActive(flag);
    }

    public void Add_PostShown(UnityAction action)
    {
        if (action != null)
            event_PostShown.AddListener(action);
        else
            Debug.LogError("Action is null");
    }

    public void ShowInvites()
    {
        if(pnlInvites)
            pnlInvites.SetActive(true);
    }
}
