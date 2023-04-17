using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SprayPaint : MonoBehaviour
{
    public GameObject sprayAnimation;
	private Vector3 lastSpawnPoint;

    private Quaternion lastSpawnRotation;
    [SerializeField]
	[Tooltip("The number of decals to keep alive at a time.  After this number are around, old ones will be replaced.")]
	private int maxConcurrentDecals = 1000;

	private Queue<GameObject> decalsInPool;
    private Queue<GameObject> decalsActiveInWorld;

    public bool isSpraying = false;
    public GameObject sprayMark;

    IEnumerator spray;

    [ContextMenu("Start Spray")]
    public void StartSpray()
    {
        if (isSpraying){
            print("already spraying");
            return;
        }

        isSpraying = true;
        if (spray != null)
            StopCoroutine(spray);
        sprayAnimation.SetActive(true);
        spray = Spray_I();
        StartCoroutine(spray);
    }

    [ContextMenu("Stop Spray")]
    public void StopSpray()
    {
        isSpraying = false;
        if (spray != null)
            StopCoroutine(spray);
        sprayAnimation.SetActive(false);
    }
	/**
	* <summary>
	* coroutine to loop the spray cycle
	* makes use of the lock 'isSpraying'
	* </summary>
	*/
    IEnumerator Spray_I()
    {
        while (isSpraying)
        {
			// initialize raycast
            RaycastHit hit;                
			Vector3 dir = Vector3.forward;
            // Shoot raycast and check if it collides with anything
            if (Physics.Raycast(transform.position, transform.TransformDirection(dir), out hit, 0.4f))
            {

                // check that the new spawn point is out of range of the last
                if (lastSpawnRotation == null||
					lastSpawnPoint == null || 
					((lastSpawnPoint - this.transform.position).magnitude > 0.01f)|| 
					Quaternion.Angle(lastSpawnRotation, this.transform.rotation)> 1f)
                {

                    lastSpawnPoint = this.transform.position;
                    lastSpawnRotation = this.transform.rotation;
                    SpawnDecal();
                    // ensure there is a rigidbody attached to the hit 
                    if (hit.rigidbody != null)
                    {
                        // check that the hit object is sprayable
                        if (hit.rigidbody.gameObject.tag == "Sprayable"){

							// spawn in an available decal and save the latest spawn point
							

                            
                            // check if we hit a demarcation object
                            Demarcation demarcation = hit.collider.gameObject.GetComponent<Demarcation>();
							if(demarcation != null) {
								demarcation.Hit();
                            }

                        }
					}
				}
			}

            yield return null;
        }
    }


	private void Start()
	{
        sprayAnimation.SetActive(false);
		InitializeDecals();
	}

	/**
	 * <summary>
	 * initialize the decal pools and instantiate the decals
	 * </summary>
	 */
	private void InitializeDecals()
	{
		// create queues
		decalsInPool = new Queue<GameObject>();
		decalsActiveInWorld = new Queue<GameObject>();

		// create decals
		for (int i = 0; i < maxConcurrentDecals; i++)
		{
			InstantiateDecal();
		}
	}

	/**
	 * <summary>
	 * creates a decal and adds it as a child to this game object
	 * leave the decal deactivated, waiting in the decalPool
	 * </summary>
	 */
	private void InstantiateDecal()
	{
		var spawned = GameObject.Instantiate(sprayMark);
		
        spawned.transform.SetParent(this.transform);
		decalsInPool.Enqueue(spawned);
		spawned.SetActive(false);
	}

	/**
	 * <summary>
	 * moves a decal from the decal pool to the hit location of the given raycast
	 * decal becomes active
	 * </summary>
	 * <param name="hit">
	 * the gameObject that gets projected on to
	 * </param>
	 */
	public void SpawnDecal()
	{
		GameObject decal = GetNextAvailableDecal();
		if (decal != null)
		{
			// change the decal's rotaion, position and parent to that of the hit object
			decal.transform.position =this.transform.position;
			decal.transform.rotation = this.transform.rotation;
			decal.transform.SetParent(null) ;

			// activate the decal
			decal.SetActive(true);
			decalsActiveInWorld.Enqueue(decal);
		}
	}

	private GameObject GetNextAvailableDecal()
	{
		if (decalsInPool.Count > 0)
			return decalsInPool.Dequeue();

		var oldestActiveDecal = decalsActiveInWorld.Dequeue();
		return oldestActiveDecal;
	}

#if UNITY_EDITOR

	private void Update()
	{
		//if (decalsActiveInWorld.Count < maxConcurrentDecals)
		//	InstantiateDecal();
		//else if (ShoudlRemoveDecal())
		//	DestroyExtraDecal();
	}

	private bool ShoudlRemoveDecal()
	{
		return decalsActiveInWorld.Count > maxConcurrentDecals;
	}

	private void DestroyExtraDecal()
	{
		if (decalsInPool.Count > 0)
			Destroy(decalsInPool.Dequeue());
		else if (ShoudlRemoveDecal() && decalsActiveInWorld.Count > 0)
			Destroy(decalsActiveInWorld.Dequeue());
	}

#endif
   
}
	
