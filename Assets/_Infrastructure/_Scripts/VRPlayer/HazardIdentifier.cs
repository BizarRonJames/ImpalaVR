using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
[RequireComponent(typeof(LineRenderer))]
public class HazardIdentifier : MonoBehaviour
{
    [SerializeField] Canvas loadCanvas;
    [SerializeField] Image loadBar;
    [SerializeField] float raycastDistance = 10f;
    [SerializeField] float pointerInitialDelay = 1.5f;
    [SerializeField] float loadBarTime = 3f;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] Hazard currentHazard;
    private LineRenderer lineRenderer;
    private Coroutine pointDelayRoutine = null;
    private Coroutine loadHazardMenuRoutine;
    RaycastHit hitInfo;
    [SerializeField] bool hitHazard;



    private void Start()
    {
        loadCanvas.enabled = false;
        hitHazard = false;
        pointDelayRoutine = null;
        // Get the LineRenderer component on this object
        lineRenderer = GetComponent<LineRenderer>();
        // Set the number of points on the line to 2 (start and end)
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        // Get the forward direction of the raycast origin
        Vector3 forward = raycastOrigin.forward;

        // Cast a ray in the forward direction and check if it hits anything
        hitHazard = Physics.Raycast(raycastOrigin.position, forward, out hitInfo, raycastDistance) && hitInfo.collider.tag == "Hazard";

        // Update the end position of the line to be the end of the raycast or the maximum distance
        Vector3 endPosition = raycastOrigin.position + (forward * raycastDistance);

        if (hitHazard)
        {
            endPosition = hitInfo.point;
            //Debug.Log("Hit Hazard");
            Hazard hazard = hitInfo.transform.GetComponent<Hazard>();
            if (hazard != null)
            {
                currentHazard = hazard;
                PointDelay(pointerInitialDelay, () => LoadHazardMenu(loadBarTime));
            }
        }

        // Update the positions of the LineRenderer
        lineRenderer.SetPosition(0, raycastOrigin.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    public void LoadHazardMenu(float duration)
    {
        if (loadHazardMenuRoutine == null)
        {
            loadHazardMenuRoutine = StartCoroutine(LoadHazardMenuRoutine(duration));
        }
        else
        {
            return;
        }
    }

    IEnumerator LoadHazardMenuRoutine(float loadDuration)
    {
        loadCanvas.enabled = true;
        float elapsedTime = 0;
        loadBar.fillAmount = 0;
        if (currentHazard != null) currentHazard.SetHazardHighlightState(HazardHighlightState.hover);
        while (elapsedTime < loadDuration)
        {
            if (!hitHazard)
            {
                currentHazard.SetHazardHighlightState(HazardHighlightState.none);
                loadHazardMenuRoutine = null;
                loadCanvas.enabled = false;
                loadBar.fillAmount = 0;
                yield break;
            }
            loadCanvas.transform.position = raycastOrigin.forward * 5f;
            loadCanvas.transform.LookAt(Camera.main.transform.position);
            float ratio = Mathf.Clamp01(elapsedTime / loadDuration);
            loadBar.fillAmount = ratio;
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
        loadCanvas.enabled = false;
        loadBar.fillAmount = 0;
        loadHazardMenuRoutine = null;
        ScoreEventManager.instance.ScoreHazards += 1;
        currentHazard.gameObject.tag = "Untagged";
        currentHazard.SetHazardHighlightState(HazardHighlightState.none);

        currentHazard.IsIdentified = true;
        if (!currentHazard.IsTarp)
        {
            HazardMenuEventSystem.instance.TarpMenuOpen(currentHazard);
        }
        



    }

    public void PointDelay(float maxTime, Action callback = null)
    {
        if (pointDelayRoutine == null)
        {
            pointDelayRoutine = StartCoroutine(PointDelayRoutine(maxTime, callback));
        }
        else
        {
            return;
        }
    }
    IEnumerator PointDelayRoutine(float maxTime, Action callback = null)
    {
        float elapsedTime = 0;
        while (elapsedTime < maxTime)
        {
            if (!hitHazard)
            {
                pointDelayRoutine = null;
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pointDelayRoutine = null;
        if (callback != null)
        {
            callback();
        }
    }
}
