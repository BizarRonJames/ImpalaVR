using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DemarcationManager : MonoBehaviour
{
    // metric properties
    public int totalDemarcations;
    private int sprayedTotalDemarcations;



    [SerializeField] public UnityEvent DemarcationMade = new UnityEvent();

    // demarcation state
    public bool isComplete;

    // singleton
    public static DemarcationManager Instance;

    void Awake()
        {

            isComplete = false;

            // initialize the total Demarcations
            totalDemarcations = 0;
            sprayedTotalDemarcations = 0;

            //singleton check
            if (Instance != null && Instance != this) 
            { 
                print("Warning!: multiple instances of house down manager");
                Destroy(this); 
            } 
            else 
            { 
                Instance = this;    
            } 
        }

    void Update()
    {
        
    }

    /**
     * <summary>
     * The demarcation will be called once sprayed
     * </summary>
     */
    public void countSprayedDemarcation(){
        sprayedTotalDemarcations++;

        if (sprayedTotalDemarcations >= totalDemarcations)
        {
            isComplete = true;
        }
        print(sprayedTotalDemarcations);

        DemarcationMade.Invoke();
    }


    /**
     * <summary>
     * The demarcation class will call this once it awakes
     * </summary>
     */
    public void countDemarcation(){
        totalDemarcations++;
    }
    /**
        * <summary>
        * returns the users spraying score
        * </summary>
        * <returns> float </returns>
        */
    public double getMetricFloat()
    {
        double f = (double)sprayedTotalDemarcations / (double)totalDemarcations;
        return f;
    }

    /**
     * <summary>
     * returns the users spraying score
     * </summary>
     * <returns> float </returns>
     */
    public string getMetricString()
    {
        
        return sprayedTotalDemarcations.ToString() + "/" + totalDemarcations.ToString();
    }
}
