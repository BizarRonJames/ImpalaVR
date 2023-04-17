using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbTrigger : MonoBehaviour
{
    [SerializeField] Transform _climbPoint;
    [SerializeField] int _climbCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        _climbCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                _climbCount++;
                if (_climbCount == 3)
                {
                    _climbCount = 0;
                }
                
                if (_climbCount == 0)
                {
                    PlayerActionManager.instance.ClimbUpEnter(_climbPoint.position);
                }
                break;
        }
    }
}
