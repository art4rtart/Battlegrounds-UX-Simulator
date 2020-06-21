using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public static DoorController Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<DoorController>();
            return instance;
        }
    }
    private static DoorController instance;

    public List<Transform> doors = new List<Transform>();
    public int doorIndex = -1;

    AudioSource audiosource;
	public bool isOpened;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            doors.Add(this.gameObject.transform.GetChild(i));
        }
    }

    public void OpenDoor()
    {
        if (doorIndex == doors.Count - 1) return;
        doorIndex = Mathf.Clamp(doorIndex += 1, 0, doors.Count - 1);
        StopAllCoroutines();
        StartCoroutine(OpenDoor(doors[doorIndex]));
    }

    IEnumerator OpenDoor(Transform _door)
    {
		isOpened = false;
		audiosource.Play();
        while (_door.position.y > -4.9f)
        {
            _door.position -= Vector3.up * Time.deltaTime * 4f;
            doors[doorIndex].position = _door.position;
            yield return null;
        }
		isOpened = true;
	}
}
