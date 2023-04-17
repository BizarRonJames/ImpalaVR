using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCollider : MonoBehaviour
{
    public Hand[] hands;
    public float vibrateTime = 0.5f;
    public LayerMask layer;

    public float hapticAmp = 0.8f;
    public float velocityAmp = 0.5f;
    public float repeatDelay = 0.2f;
    public float maxDuration = 0.5f;

    bool playing = false;

    Coroutine playRoutine;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playing && Checked())
            StartCoroutine(Vibrate());
    }

    IEnumerator Vibrate()
    {
        WaitForSeconds wait = new WaitForSeconds(vibrateTime);
        playing = true;
        while (Checked())
        {                        
            foreach (Hand h in hands)
                h.PlayHapticVibration(vibrateTime+0.1f);
            yield return wait;
        }
        playing = false;
    }

    bool Checked()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10, layer))
        {
            Debug.Log("Hitting: " + hit.transform.gameObject.name);
            return true;
        }
        else
            return false;
    }
}
