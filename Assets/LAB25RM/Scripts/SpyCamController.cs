using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpyCamController : MonoBehaviour
{
    [Header("Spy Cam")]
    public Camera spyCam;
    public Vector2 spyCamRangeMinMax;
    public float spyCamRange = 50f;
    public LayerMask spyCamLayerMask;

    [Header("Zoom")]
    public float zoomSpeed = 1.5f;
    public float minZoomValue = 40f;
    public float maxZoomValue = 60f;

    string year;
    string month;
    string day;
    string time;
    string dayOfWeek;

    [Header("UI Info")]
    public TextMeshProUGUI dayTimeText;
    public TextMeshProUGUI currentTimeText;

    public TextMeshProUGUI locationText;
    public TextMeshProUGUI cameraText;

    public TextMeshProUGUI zoomingText;

    DateTime date;
    public string currentLocation = "Hallway 01";
    public string currentCamera = "Camera 02";

    private void Awake()
    {
        date = DateTime.Now;
        year = date.Year.ToString();
        month = date.Month.ToString();
        day = date.Day.ToString();
        dayOfWeek = date.DayOfWeek.ToString();
        Debug.Log(dayOfWeek);

        locationText.text = currentLocation;
        cameraText.text = currentCamera;

        dayTimeText.text = Day(dayOfWeek) + ", " + Month(month) + " " + day + ", " + year;
    }

    void Update()
    {
        if (GameTimeController.isPaused) return;

        currentTimeText.text = DateTime.Now.ToString("hh:mm:ss:ff tt");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, spyCamRange, spyCamLayerMask))
        {
            if (Input.GetMouseButtonDown(0)) Teleport(hit.transform);
        }

        if (Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            StartCoroutine(Zoom(spyCam.fieldOfView, minZoomValue, zoomSpeed, 0f, 1f, true));
        }

        if (Input.GetMouseButtonUp(1))
        {
            StopAllCoroutines();
            StartCoroutine(Zoom(spyCam.fieldOfView, maxZoomValue, zoomSpeed * 2f, 1f, 0f, false));
        }
    }

    void Teleport(Transform _spyCam)
    {
        SpyCam spyCamera = _spyCam.GetComponent<SpyCam>();
        locationText.text = spyCamera.locationName;
        cameraText.text = "CAMERA 0" + spyCamera.cameraNum;

        StopAllCoroutines();
        StartCoroutine(Zoom(spyCam.fieldOfView, maxZoomValue, zoomSpeed * 2f, 1f, 0f, false));
        this.gameObject.transform.SetParent(_spyCam);
        InitializeObjectTrasnform(this.gameObject.transform);
    }

    void InitializeObjectTrasnform(Transform _gameObject)
    {
        _gameObject.localPosition = Vector3.zero;
        _gameObject.localRotation = Quaternion.identity;
        _gameObject.localScale = Vector3.one;
    }

    IEnumerator Zoom(float zoomValue, float targetZoomValue, float _zoomSpeed, float currentAlpha, float targetAlpha, bool zoomin)
    {
        float currentZoomValue = zoomValue;
        float lerpValue = 0f;
        float alpha = 0;

        float dir = (zoomin == true) ? 1 : -1;
        float spyCamValue = spyCamRange;

        while (zoomValue != targetZoomValue)
        {
            zoomValue = Mathf.Lerp(currentZoomValue, targetZoomValue, lerpValue);
            lerpValue += Time.deltaTime * _zoomSpeed;
            spyCam.fieldOfView = zoomValue;

            spyCamRange = Mathf.Clamp(spyCamRange += Time.deltaTime * _zoomSpeed * 20f * dir, spyCamRangeMinMax.x, spyCamRangeMinMax.y);

            alpha = Mathf.Lerp(currentAlpha, targetAlpha, lerpValue);
            zoomingText.color = new Color(zoomingText.color.r, zoomingText.color.g, zoomingText.color.b, alpha);
            yield return null;
        }
    }

    string Day(string value)
    {
        string day = "";
        switch (value)
        {
            case "Monday":
                day = "Mon";
                break;
            case "Tuesday":
                day = "Tue";
                break;
            case "Wednsday":
                day = "Wed";
                break;
            case "Thursday":
                day = "Thr";
                break;
            case "Friday":
                day = "Fri";
                break;
            case "Satday":
                day = "Sat";
                break;
            case "Sunday":
                day = "Sun";
                break;
        }
        return day;
    }

    string Month(string value)
    {
        string month = "";
        switch(value)
        {
            case "1":
                month = "Jan";
                break;
            case "2":
                month = "Feb";
                break;
            case "3":
                month = "Mar";
                break;
            case "4":
                month = "Apr";
                break;
            case "5":
                month = "May";
                break;
            case "6":
                month = "Jun";
                break;
            case "7":
                month = "Jul";
                break;
            case "8":
                month = "Aug";
                break;
            case "9":
                month = "Sep";
                break;
            case "10":
                month = "Oct";
                break;
            case "11":
                month = "Nov";
                break;
            case "12":
                month = "Dec";
                break;
        }
        return month;
    }
}
