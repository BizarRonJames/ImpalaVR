using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * <summary>
 * This manager tracks all the dirt objects in the scene. It will calculate a metric to 
 * grade the user on their cleaning task
 * </summary>
 */
public class HoseDownManager : MonoBehaviour
{
    // metric properties
    public int totalDirt;
    private int cleanedTotalDirt;

    [SerializeField] public UnityEvent AdapterAttached = new UnityEvent();
    [SerializeField] public UnityEvent DirtCleaned = new UnityEvent();

    // house down state
    public bool isWashed;
    public bool isConnected;

    // singleton
    public static HoseDownManager Instance;

    /**
     * <summary>
     * initialization
     * </summary>
     */
    void Awake()
        {

            // initialize the total dirt
            totalDirt = 0;
            cleanedTotalDirt = 0;
            isWashed = false;

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
     * The dirt class will call this once its been cleaned
     * </summary>
     */
    public void countCleanedDirt(){

        cleanedTotalDirt++;
        if (cleanedTotalDirt >= totalDirt){
            isWashed = true;
        }
        print(cleanedTotalDirt);

        print(totalDirt);
        DirtCleaned.Invoke();
    }


    /**
     * <summary>
     * The dirt class will call this once it awakes
     * </summary>
     */
    public void countDirt(){
        totalDirt++;
    }

    /**
     * <summary>
     * returns the users cleaning score
     * </summary>
     * <returns> float </returns>
     */
    public double getMetricFloat(){
        double f = (double) cleanedTotalDirt / (double) totalDirt;
        return  f ;
    }

    /**
     * <summary>
     * returns the users cleaning score
     * </summary>
     * <returns> float </returns>
     */
    public string getMetricString()
    {
        double val = Math.Round(getMetricFloat() * 100,2);
        return val.ToString()+"%";
    }
}
