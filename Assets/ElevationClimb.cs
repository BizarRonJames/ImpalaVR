using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoHand;
using Autohand.Demo;
using System;
using NaughtyAttributes;
using UnityEngine.Serialization;

public class ElevationClimb : MonoBehaviour
{
    
    [SerializeField] Transform _movePoint;
    [SerializeField] AnimationCurve _upCurve;
    [SerializeField] float _maxSpeed = 5f; 
    [SerializeField] float accelaratePeriod = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            StartCoroutine(ClimbUp(other.transform, _maxSpeed, accelaratePeriod));
        }
    }

    IEnumerator ClimbUp(Transform player, float maxMoveSpeed, float accelaratePeriod){
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        Collider playerCollider = player.GetComponent<Collider>();
        
        if(playerRb == null || playerCollider == null){
            yield return null;
        }
        else{
            playerRb.isKinematic = true;
            float colliderHeight = playerCollider.bounds.size.y/2;
            float time = 0;
            float heightDifference = (_movePoint.position.y + colliderHeight + 0.1f) - player.position.y;
            Vector3 target = _movePoint.position + (colliderHeight * Vector3.up);
            while(true){
                if(player.position.y >= (_movePoint.position.y + colliderHeight + 0.1f)){
                    break;
                }
                else{
                    float ratio = time/accelaratePeriod;
                    Mathf.Clamp(ratio, 0, 1);
                    float speed = _upCurve.Evaluate(ratio) * maxMoveSpeed;
                    Vector3 movePosition = Vector3.MoveTowards(player.position, target, speed * Time.deltaTime);
                    playerRb.MovePosition(movePosition);
                    yield return null;
                }
            }
            
        }
        
    }
}
