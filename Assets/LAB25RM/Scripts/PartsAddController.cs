using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsAddController : MonoBehaviour
{
    public static PartsAddController Instance  {
        get {
            if (instance != null) return instance;
            instance = FindObjectOfType<PartsAddController>();
            return instance;
        }
    } private static PartsAddController instance;

    [Header("Scope[2x/4x/8x]")]
    public GameObject[] Scopes;
    public RawImage[] rawImage;
    public int rawImageIndex;
    public Color zoomInColor;
    public Color zoomOutColor;

    [Header("Muzzle[Silencer/Surpressor/Compenstaor]")]
    public GameObject[] Muzzles;
    public int muzzleIndex;

    [Header("Magazine[Silencer/Suppressor/Compenstaor]")]
    public int magazineIndex;

    [Header("Stock[Light/Tactical]")]
    public GameObject[] Stocks;
    public int stockIndex;

    [Header("Handle[Vertical/Horizontal]")]
    public int handleIndex;

    public void SetScope(string _name)
    {
        if(rawImageIndex >= 0) Scopes[rawImageIndex].SetActive(false);

        switch (_name)
        {
            case "":
                rawImageIndex = -1;
                break;

            case "2x Scope":
                rawImageIndex = 0;
                break;
            case "4x Scope":
                rawImageIndex = 1;
                break;
            case "8x Scope":
                rawImageIndex = 2;
                break;
        }
        if (rawImageIndex >= 0) Scopes[rawImageIndex].SetActive(true);
    }

    public void SetMuzzle(string _name)
    {
        if (muzzleIndex >= 0) Muzzles[muzzleIndex].SetActive(false);

        switch (_name)
        {
            case "":
                muzzleIndex = -1;
                break;

            case "Silencer":
                muzzleIndex = 0;
                break;
            case "Compensator":
                muzzleIndex = 1;
                break;
            case "Suppressor":
                muzzleIndex = 2;
                break;
        }
        if (muzzleIndex >= 0) Muzzles[muzzleIndex].SetActive(true);
    }

    public void SetHandle(string _name)
    {

    }
    public void SetStock(string _name)
    {
        if (stockIndex >= 0) Stocks[stockIndex].SetActive(false);

        switch (_name)
        {
            case "":
                stockIndex = -1;
                break;

            case "Light Stock":
                stockIndex = 0;
                break;
            case "Tactical Stock":
                stockIndex = 1;
                break;
        }
        if (stockIndex >= 0) Stocks[stockIndex].SetActive(true);
    }

    public void SetMagazine(string _name)
    {

    }



    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && rawImageIndex >= 0)
        {
            StopAllCoroutines();
            StartCoroutine(ColorLerp(rawImage[rawImageIndex].color, zoomInColor));
        }

        else if (Input.GetMouseButtonUp(1) && rawImageIndex >= 0)
        {
            StopAllCoroutines();
            StartCoroutine(ColorLerp(rawImage[rawImageIndex].color, zoomOutColor));
        }
    }

    IEnumerator ColorLerp(Color currentColor, Color targetColor)
    {
        float lerpSpeed = 0f;
        while (rawImage[rawImageIndex].color != targetColor)
        {
            rawImage[rawImageIndex].color = Color.Lerp(rawImage[rawImageIndex].color, targetColor, lerpSpeed);
            lerpSpeed += Time.deltaTime * .75f;
            yield return null;
        }
    }
}
