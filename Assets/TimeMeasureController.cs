using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		if (SceneManager.GetActiveScene().name == "Title" || SceneManager.GetActiveScene().name == "Credit") return;
        if(!Goal.Instance.goal) totalGameTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Tab)) totalCustomizationTime = !totalCustomizationTime;
        if (totalCustomizationTime) customizationTime += Time.deltaTime;
    }
}
