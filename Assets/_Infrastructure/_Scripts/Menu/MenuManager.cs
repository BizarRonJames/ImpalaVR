using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;            
        }
        else
            Debug.LogError("There is more than one instance of MenuManager in this scene");
    }

    public bool autoStart = true;
    public bool shouldRotate = true;
    public float waitTime = 0.7f;
    public float animationTime = 0.3f;
    public GameObject[] menus;     

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        if(autoStart)
            ShowMenu_Login();
    }

    public void SelectMenu(int index, float waitTime = 0.3f)
    {
        StartCoroutine(SelectMenu_I(index, waitTime));

    }

    IEnumerator SelectMenu_I(int index, float waitTime)
    {
        for (int i = 0; i < menus.Length; i++)
            if (i != index && menus[i].activeSelf)
            {
                yield return RotateObjects(new Transform[] { menus[i].transform }, this.animationTime, Vector3.zero, new Vector3(0, 0, 90));
                menus[i].SetActive(false);                
            }
        yield return new WaitForSeconds(waitTime);
        if (index >= 0 && index < menus.Length)
        {
            menus[index].transform.localRotation = Quaternion.identity;
            menus[index].SetActive(true);
        }
        else
            Debug.LogError("Menu index " + index + "is out of range");
    }

    public void ShowMenu_Login()
    {
        SelectMenu(0,waitTime);
    }

    public void ShowMenu_Main()
    {
        SelectMenu(1, waitTime);
    }

    public void ShowMenu_Reports()
    {
        SelectMenu(2, waitTime);
    }

    public void ShowMenu_Offline()
    {
        SelectMenu(3, waitTime);
    }

    public void ShowMenu_Exit()
    {
        SelectMenu(4, waitTime);
    }

    public void ShowMenu_Lobby()
    {
        SelectMenu(5, waitTime);
    }

    public void HideAllMenus()
    {
        SelectMenu(-1, waitTime);
    }

    #region public UI functions
    public void RotateObjectsInOnX(Transform[] rects, float waitTime)
    {
        StartCoroutine(RotateObjects(rects, waitTime, new Vector3(90, 0, 0), Vector3.zero));
    }

    public IEnumerator RotateObjects(Transform[] rects, float waitTime, Vector3 startRotation, Vector3 endRotation)
    {        
        foreach (Transform rect in rects)
            rect.localRotation = Quaternion.Euler(startRotation);
        float rate = 1 / waitTime;
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            Quaternion rot = Quaternion.Euler(Vector3.Lerp(startRotation, Vector3.zero, i));
            foreach (Transform rect in rects)
                rect.localRotation = rot;
            yield return null;
        }
    }

    public IEnumerator ResizeObjects(Transform[] rects, float waitTime, Vector3 startSize, Vector3 endSize, bool swipeMenu = false)
    {
        foreach (Transform rect in rects)
            rect.localScale = startSize;
        float rate = 1.0f / waitTime;
        float i = 0.0f;
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            Vector3 size = Vector3.Lerp(startSize, endSize,i);            
            foreach (Transform rect in rects)
                rect.localScale = size;
            if (swipeMenu)
            {
                Vector3 targetRot = Player.instance.head.transform.rotation.eulerAngles;
                targetRot.x = 0;
                targetRot.z = 0;
                Vector3 startRot = new Vector3(0, targetRot.y + 90, 0);
                if (shouldRotate)
                {
                    MenuManager.instance.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRot, targetRot, i));
                    // MenuManager.instance.transform.position = Player.instance.head.transform.position;
                }
            }

            yield return null;
        }
    }
    #endregion


}
