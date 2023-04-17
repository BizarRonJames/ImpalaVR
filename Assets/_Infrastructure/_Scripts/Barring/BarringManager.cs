using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * <summary>
 * This manager tracks all the baring objects in the scene. It will calculate a metric to 
 * grade the user on their barring task
 * </summary>
 */
public class BarringManager : MonoBehaviour
{
    // metric properties
    public int totalBarringRocks;
    private int cleanedTotalBarredRocks;

    [SerializeField] public UnityEvent RockBarred = new UnityEvent();


    // barring down state
    public bool isComplete;

    // singleton
    public static BarringManager Instance;
    
    void Awake(){
        isComplete = false;

        // initialize the total dirt
        totalBarringRocks = 0;
        cleanedTotalBarredRocks = 0;

        //singleton check
        if (Instance != null && Instance != this) 
        { 
            print("Warning!: multiple instances of barring rock manager");
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;    
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * <summary>
     * The dirt class will call this once its been cleaned
     * </summary>
     */
    public void countBarredRock(){
        cleanedTotalBarredRocks++;

        if (cleanedTotalBarredRocks >= totalBarringRocks)
        {
            isComplete = true;
        }
        print(cleanedTotalBarredRocks);

        RockBarred.Invoke();
        Destroy(this);
    }


    /**
     * <summary>
     * The dirt class will call this once it awakes
     * </summary>
     */
    public void countRock(){
        totalBarringRocks++;
    }
    public double getMetricFloat()
    {
        double f = (double)cleanedTotalBarredRocks / (double)totalBarringRocks;
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

        return cleanedTotalBarredRocks.ToString() + "/" + totalBarringRocks.ToString();
    }
}

