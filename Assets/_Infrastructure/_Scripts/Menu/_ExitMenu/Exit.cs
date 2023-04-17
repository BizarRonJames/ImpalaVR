using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void Yes()
    {
        Application.Quit();
    }

    public void No()
    {
        MenuManager.instance.SelectMenu(1);
    }
}
