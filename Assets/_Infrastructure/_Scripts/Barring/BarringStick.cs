using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * has a collision event that checks for rock collisions. This is used to call the checkRotation function of the Barring Rock class
 * </summary>
 */
public class BarringStick : MonoBehaviour
{

    /**
     * <summary>
     * event listener that checks for a collision with a barring rock
     * </summary>
     * <param name="collision">
     * the barring rock that gets collided with
     * </param>
     */
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "BarringRock")
        {
            collision.gameObject.GetComponent<BarringRock>().checkNewRotation();
        }
    }
}
