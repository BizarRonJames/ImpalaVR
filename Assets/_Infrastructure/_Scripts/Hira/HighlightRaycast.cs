using Autohand;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Shoots a raycast that targets a hazard object for highlighting
/// </summary>
public class HighlightRaycast : MonoBehaviour{

    private GameObject lastHIT;
    public bool isPointing = false;
    IEnumerator point;

    public HazardMenu hazardMenu;

    public LineRenderer laserLineRenderer;
    public LineRenderer laserLineRendererAnimation;

    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;


    public bool loaded = false;

    void Start()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRendererAnimation.SetPositions(initLaserPositions);
    }



    [ContextMenu("Start Pointing")]
    public void StartPoint()
    {

        if (isPointing)
        {
            print("already pointing");
            return;
        }

        isPointing = true;
        if (point != null)
            StopCoroutine(point);
        point = Pointing_I();
        StartCoroutine(point);

    }

    [ContextMenu("Stop Pointing")]
    public void StopPoint()
    {
        isPointing = false;
        if (point != null)
            StopCoroutine(point);

        if (lastHIT != null) lastHIT.GetComponent<Hazard>().highLighted = false; //SetGameLayerRecursive(lastHIT, 0);
        lastHIT = null;
        laserLineRenderer.enabled = false;

        laserLineRendererAnimation.enabled = false;
    }
    const float timeConst = 100.0f;
    private float timeout = timeConst;
    IEnumerator Pointing_I()
    {
        while (isPointing==true)
        {
            // initialize raycast
            RaycastHit hit;
            Vector3 dir = this.gameObject.transform.forward;

            Vector3 endPosition = transform.position + (5f * dir);
            Vector3 startPosition = transform.position + 0.15f*transform.forward;

            // Shoot raycast and check if it collides with anything
            if (Physics.Raycast(startPosition, dir, out hit, 20f))
            {
                Hazard haz = hit.collider.gameObject.GetComponent<Hazard>();
                 
                if (hit.collider.gameObject.tag == "Hazard" 
                    && haz != null)
                {

                    if (lastHIT == hit.collider.gameObject)
                    {
                      
                        // Load Menu
                        if(timeout > 0)
                        {
                            Vector3 endPosition2;
                            laserLineRendererAnimation.enabled = true;
                            if (!loaded)
                            {
                                 endPosition2 = transform.position + ( (float) ( -( timeout - timeConst ) / timeConst)* 5f * dir);
                            }
                            else
                            {
                                endPosition2 = transform.position + ( 5f * dir);
                            }

                            laserLineRendererAnimation.SetPosition(0, startPosition);
                            laserLineRendererAnimation.SetPosition(1, endPosition2);
                           
                            timeout--;
                        }
                        else
                        {
                            loaded = true;
                            laserLineRendererAnimation.enabled = false;
                            hazardMenu.gameObject.GetComponent<Canvas>().enabled = (true);
                            hazardMenu.Hazard = haz;
                            lastHIT = null;

                            //HazardMenuEventSystem.instance.MenuOpen(haz);
                            //HazardMenuEventSystem.instance.transform.position = this.transform.position + (10*this.transform.forward);
                            //HazardMenuEventSystem.instance.transform.LookAt(this.transform.position, Vector3.up);
                           
                           
                            QuestionManager.instance.InitializeNewQuestion(haz.GetRandomMCQ());
                            QuestionManager.instance.LastHazard = haz;

                            // display menu infront of user
                            hazardMenu.transform.position = (this.transform.position + 10*this.transform.forward);

                            hazardMenu.transform.LookAt(this.transform.position, Vector3.up);
                            //timeout = 100;
                        }
                    }
                    else
                    {

                        loaded = false;
                        laserLineRendererAnimation.enabled = false;
                        timeout = 100;
                        if (lastHIT != null)
                        {
                            lastHIT.GetComponent<Hazard>().highLighted = false;
                            //SetGameLayerRecursive(lastHIT, 0);
                        }
                        lastHIT = hit.collider.gameObject;
                        haz.highLighted= true;
                        //SetGameLayerRecursive(hit.collider.gameObject, 20);
                    }
                    
                }
                else if (lastHIT != null)
                {
                    //loaded = false;
                    laserLineRendererAnimation.enabled = false;
                    timeout = 100;
                    //SetGameLayerRecursive(lastHIT, 0);

                    lastHIT.GetComponent<Hazard>().highLighted = false;
                    lastHIT = null;
                }


            }


            laserLineRenderer.SetPosition(0, startPosition);
            laserLineRenderer.SetPosition(1, endPosition);
            // ShootLaserFromTargetPosition(transform.position, Vector3.forward, laserMaxLength);
            laserLineRenderer.enabled = true;

            yield return null;
        }

    }
    





}
