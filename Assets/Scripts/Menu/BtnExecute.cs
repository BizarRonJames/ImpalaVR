using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BtnExecute : MonoBehaviour
{
    public Animator _anim;
    public string loadAnim = "load";
    public string idleAnim = "idle";

    public UnityEvent action;

    public void InvokeAction()
    {
        //Debug.Log($"Invoking Action: {gameObject.name}");
        if (action != null)
            action.Invoke();
    }

    public void Load()
    {
        //Debug.Log($"btnExecute: Loading: {gameObject.name}");
        _anim.Play(loadAnim);
    }

    public void Idle()
    {
        //Debug.Log($"btnExecute: Idling: {gameObject.name}");
        _anim.Play(idleAnim);
    }
}
