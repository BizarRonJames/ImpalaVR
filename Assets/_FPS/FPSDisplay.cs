using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class FPSDisplayperformance : MonoBehaviour
{
	/*public static FPSDisplay1 instance;
    private void Awake()
    {
		if (!instance)
			instance = this;
		else
			Debug.LogError("There is more than one instance of FPSDisplay1 in this scene");
    }*/

	float deltaTime = 0.0f;

	public TMP_Text txtOutput;
	public TMP_Text txtOutput2;

	float totalFrames = 0;

	int framesCounted = 0;
	float sumFrames = 0;

	float timePassed = 0;



	Image img;
	Color[] cols = new Color[] { Color.yellow, Color.cyan, Color.white };
	int totalChanges = -1;


	float maxFPS;
	float minFPS;
	private void Start()
	{
		minFPS = 500;
		maxFPS = 0;
		img = GetComponentInChildren<Image>();
	}

	void OnDestroy(){
		print("min fps: " + minFPS);
		print("max fps: " + maxFPS);
	}

	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		timePassed += Time.deltaTime;

		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

		if (timePassed > 5)
		{

			timePassed = 0;
			txtOutput2.text = "XXXXX";
			framesCounted = 0;
			sumFrames = 0;

			totalChanges++;
			if (totalChanges >= cols.Length)
				totalChanges = 0;
			img.color = cols[totalChanges];
		}

		framesCounted++;
		sumFrames += fps;
		txtOutput2.text = (sumFrames / framesCounted).ToString();

		totalFrames++;
		if (totalFrames > 6)
		{
			txtOutput.text = text;
			totalFrames = 0;

			if(fps<minFPS)minFPS=fps;
			if(fps>maxFPS)maxFPS=fps;


		}
	}

}