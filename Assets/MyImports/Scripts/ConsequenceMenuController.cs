using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsequenceMenuController : MonoBehaviour
{
    [SerializeField] HoverSelect _closeButton;
    // Start is called before the first frame update
    void Start()
    {
        _closeButton.select.AddListener(delegate{HazardMenuEventSystem.instance.MenuClose();});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
