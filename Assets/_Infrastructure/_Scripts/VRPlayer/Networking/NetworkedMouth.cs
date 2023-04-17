using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedMouth : MonoBehaviour
{
    public float modifier = 120.0f;
    public Transform lip;
    public Vector3 openRot;
    public Vector3 closedRot;

    private RealtimeAvatarVoice _voice;
    private float _mouthSize;

    private void Awake()
    {
        _voice = GetComponent<RealtimeAvatarVoice>();
    }
    
    void Update()
    {
        float targetMouthSize = Mathf.Lerp(0.0f, 1.0f, _voice.voiceVolume);
        //_mouthSize = Mathf.Lerp(_mouthSize, targetMouthSize, 30.0f * Time.deltaTime);

        lip.localRotation = Quaternion.Lerp(Quaternion.Euler(closedRot), Quaternion.Euler(openRot), targetMouthSize * modifier * Time.deltaTime);
    }
}
