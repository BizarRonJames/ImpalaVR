using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMover : MonoBehaviour
{

    [SerializeField] private AnimationCurve _upSpeedCurve;
    [SerializeField] private float _upwardMovementTime;
    [SerializeField] private AnimationCurve _forwardSpeedCurve;
    [SerializeField] private float _forwardMovementTime;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;


    // Start is called before the first frame update
    void Start()
    {
        //_rigidbody = GetComponentInChildren<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        PlayerActionManager.instance.OnClimbUpEnter += OnClimbUpEnter;
        PlayerActionManager.instance.OnClimbExit += OnClimbExit;
        PlayerActionManager.instance.OnClimbDownEnter += OnClimbDownEnter;
    }

    private void OnDisable()
    {
        PlayerActionManager.instance.OnClimbUpEnter -= OnClimbUpEnter;
        PlayerActionManager.instance.OnClimbExit -= OnClimbExit;
        PlayerActionManager.instance.OnClimbDownEnter -= OnClimbDownEnter;
    }

    private void OnClimbUpEnter(Vector3 position)
    {
        _collider.enabled = false;
        //Vector3 moveHeight = new Vector3(transform.position.x, _movePoint.position.y, transform.position.z);
        _rigidbody.isKinematic = true;
        float moveHeight = position.y; ;
        Action climbExit = PlayerActionManager.instance.ClimbExit;
        //Vector3 movePosition = _movePoint.position + (Vector3.up * (_collider.bounds.size.y / 2));
        StartCoroutine(MoveToHeight(moveHeight, _upwardMovementTime, _upSpeedCurve, () => StartCoroutine(MoveToPosition(position, _forwardMovementTime, _forwardSpeedCurve, climbExit))));
    }

    private void OnClimbDownEnter(Vector3 position)
    {
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        Action climbExit = PlayerActionManager.instance.ClimbExit;
        Vector3 movePosition = new Vector3(position.x, transform.position.y, position.z);
        float moveHeight = position.y;
        StartCoroutine(MoveToPosition(movePosition, _forwardMovementTime, _forwardSpeedCurve, () => StartCoroutine(MoveToHeight(moveHeight, _upwardMovementTime, _upSpeedCurve, climbExit))));
    }

    private void OnClimbExit()
    {
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
    }


    public void MoveUpToPoint(Vector3 position)
    {
        //Vector3 moveHeight = new Vector3(transform.position.x, _movePoint.position.y, transform.position.z);
        float moveHeight = position.y;
        //Vector3 movePosition = _movePoint.position + (Vector3.up * (_collider.bounds.size.y / 2));
        StartCoroutine(MoveToHeight(moveHeight, _upwardMovementTime, _upSpeedCurve, () => StartCoroutine(MoveToPosition(position, _forwardMovementTime, _forwardSpeedCurve))));
    }

    public IEnumerator MoveToHeight(float targetHeight, float movementTime, AnimationCurve speedCurve, Action callback = null)
    {
        _rigidbody.isKinematic = true;
        Vector3 startingPosition = transform.position;
        float distance = Mathf.Abs(targetHeight - startingPosition.y);
        float elapsedTime = 0f;
        while (Mathf.Abs(transform.position.y - targetHeight) > 0.01f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / movementTime);
            float speedMultiplier = speedCurve.Evaluate(t);
            float direction = Mathf.Sign(targetHeight - startingPosition.y);
            float newPosition = startingPosition.y + distance * speedMultiplier * direction;
            transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);


        if (callback != null)
        {
            callback();
        }
    }

    public IEnumerator MoveToPosition(Vector3 targetPosition, float speed, AnimationCurve speedCurve, Action onReachedTarget = null)
    {
        _rigidbody.isKinematic = true;
        float distance = Vector3.Distance(transform.position, targetPosition);
        float totalTime = speedCurve.keys[speedCurve.length - 1].time;
        float startTime = Time.time;
        float journeyLength = distance;
        float currentSpeed = 0f;
        Vector3 startPosition = transform.position;

        while (journeyLength > 0.01f)
        {
            float elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            currentSpeed = speed * speedCurve.Evaluate(t);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
            journeyLength = Vector3.Distance(transform.position, targetPosition);
            yield return null;
        }

        transform.position = targetPosition;
        onReachedTarget?.Invoke();
    }




}
