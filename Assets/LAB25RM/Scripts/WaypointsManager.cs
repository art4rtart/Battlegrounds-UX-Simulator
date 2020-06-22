using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointsManager : MonoBehaviour
{
    public static WaypointsManager Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<WaypointsManager>();
            return instance;
        }
    }
    private static WaypointsManager instance;

    public int totalWaypoints = 0;
    public List<Transform> waypoints = new List<Transform>();

    void Awake()
    {
        totalWaypoints = this.transform.childCount;

        for(int i = 0; i < this.transform.childCount; ++i)
        {
            waypoints.Add(this.transform.GetChild(i).transform);
        }
    }

	private void Start()
	{
		
	}
}
