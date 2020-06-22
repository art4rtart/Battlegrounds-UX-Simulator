using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMeasureController : MonoBehaviour
{
	public static TimeMeasureController Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(gameObject);
	}

	public float totalGameTime;
    public float customizationTime;

	public float[] customTime = new float[3];

    bool totalCustomizationTime = false;
    bool goal = false;

    private void Update()
    {
        if(!Goal.Instance.goal) totalGameTime += Time.deltaTime;
        else Debug.Log("Customization Time : " + customizationTime + " Total Game Time : " + totalGameTime);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            totalCustomizationTime = !totalCustomizationTime;
        }

        if (totalCustomizationTime) customizationTime += Time.deltaTime;
    }
}
