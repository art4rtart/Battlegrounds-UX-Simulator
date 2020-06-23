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
    public int scopeImageIndex;
	public int scopeIndex;
	public Color zoomInColor;
    public Color zoomOutColor;

    [Header("Muzzle[Silencer/Surpressor/Compenstaor]")]
    public GameObject[] Muzzles;
    public int muzzleIndex;

	public ParticleSystem muzzleParticle;
	public CrossHair crossHair;

	[Header("Magazine[Silencer/Suppressor/Compenstaor]")]
    public GameObject[] Magazines;
    public int magazineIndex;

    [Header("Stock[Light/Tactical]")]
    public GameObject[] Stocks;
    public int stockIndex;

    [Header("Handle[Vertical/Horizontal]")]
    public GameObject[] Handles;
    public int handleIndex;

    public void SetScope(string _name)
    {
        if(scopeImageIndex >= 0) Scopes[scopeImageIndex].SetActive(false);

        switch (_name)
        {
            case "Emtpy":
				Scopes[scopeImageIndex].SetActive(false);
				scopeIndex = -1;
				break;
            case "2x Scope":
                scopeImageIndex = 0;
				scopeIndex = 0;
				Scopes[scopeImageIndex].SetActive(true);
				break;
            case "4x Scope":
                scopeImageIndex = 1;
				scopeIndex = 1;
				Scopes[scopeImageIndex].SetActive(true);
				break;
            case "8x Scope":
                scopeImageIndex = 2;
				scopeIndex = 2;
				Scopes[scopeImageIndex].SetActive(true);
				break;
        }
    }

    public void SetMuzzle(string _name)
    {
        if (muzzleIndex >= 0) Muzzles[muzzleIndex].SetActive(false);

		// init
		WeaponController.Instance.audioClip[0] = WeaponController.Instance.normalSound;
		var emission = muzzleParticle.emission;
		emission.enabled = true;
		crossHair.addValue = 1.5f;

		WeaponController.Instance.equippedGun.damageRate = 35f;
		WeaponController.Instance.equippedGun.fireRate = 100f;
		WeaponController.Instance.equippedGun.recoilStrengthMinMax = new Vector2(7, 3);

		switch (_name)
        {
            case "Empty":
				Muzzles[muzzleIndex].SetActive(false);
				muzzleIndex = -1;
				break;

            case "Silencer":
                muzzleIndex = 0;
				WeaponController.Instance.equippedGun.damageRate = 50f;
				WeaponController.Instance.audioClip[0] = WeaponController.Instance.silenceSound;
				break;

            case "Compensator":
                muzzleIndex = 1;
				crossHair.addValue = .25f;
				WeaponController.Instance.equippedGun.recoilStrengthMinMax = new Vector2(2, 1);
				break;

            case "Suppressor":
                muzzleIndex = 2;
				emission = muzzleParticle.emission;
				emission.enabled = false;
				WeaponController.Instance.equippedGun.fireRate = 70f;
				break;
        }
        if (muzzleIndex >= 0) Muzzles[muzzleIndex].SetActive(true);
    }

    public void SetHandle(string _name)
    {
        if (handleIndex >= 0) Handles[handleIndex].SetActive(false);

		WeaponController.Instance.equippedGun.kickMinMax = new Vector2(.1f, .1f);
		WeaponController.Instance.equippedGun.recoilStrengthMinMax = new Vector2(7, 3);

		switch (_name)
        {
            case "Empty":
				Handles[handleIndex].SetActive(false);
				handleIndex = -1;
                break;
            case "Vertical Handle":
                handleIndex = 0;
				WeaponController.Instance.equippedGun.kickMinMax = new Vector2(.25f, .25f);
				break;
            case "Angle Handle":
				WeaponController.Instance.equippedGun.recoilStrengthMinMax = new Vector2(1, .5f);
				handleIndex = 1;
                break;
        }
        if (handleIndex >= 0) Handles[handleIndex].SetActive(true);
    }

    public void SetStock(string _name)
    {
        if (stockIndex >= 0) Stocks[stockIndex].SetActive(false);

		FirstPersonController.Instance.walkSpeed = 5f;
		FirstPersonController.Instance.runSpeed = 10f;
		FirstPersonController.Instance.moveSpeed = 1f;
		WeaponController.Instance.equippedGun.kickMinMax = new Vector2(.1f, .1f);

		switch (_name)
        {
            case "Empty":
				Stocks[stockIndex].SetActive(false);
				stockIndex = -1;
                break;
            case "Light Stock":
                stockIndex = 0;
				FirstPersonController.Instance.walkSpeed = 10f;
				FirstPersonController.Instance.runSpeed = 15f;
				FirstPersonController.Instance.moveSpeed = 2f;
				break;
            case "Tactical Stock":
                stockIndex = 1;
				WeaponController.Instance.equippedGun.kickMinMax = new Vector2(.05f, .05f);
				break;
        }
        if (stockIndex >= 0) Stocks[stockIndex].SetActive(true);
    }

    public void SetMagazine(string _name)
    {
        if (magazineIndex >= 0) Magazines[magazineIndex].SetActive(false);

		WeaponController.Instance.equippedGun.maxAmo = 30f;
		WeaponController.Instance.equippedGun.magazineAmo = 30f;
		WeaponController.Instance.reloadSpeed = 1f;

		switch (_name)
        {
            case "Empty":
                magazineIndex = -1;
                break;
            case "Quick Draw":
                magazineIndex = 0;
				WeaponController.Instance.reloadSpeed = 1.5f;
				break;
            case "Large":
                magazineIndex = 1;
				WeaponController.Instance.equippedGun.maxAmo = 40f;
				WeaponController.Instance.equippedGun.magazineAmo = 40f;
				break;
        }
        if (magazineIndex >= 0) Magazines[magazineIndex].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && scopeImageIndex >= 0)
        {
            StopAllCoroutines();
            StartCoroutine(ColorLerp(rawImage[scopeImageIndex].color, zoomInColor));
        }

        else if (Input.GetMouseButtonUp(1) && scopeImageIndex >= 0)
        {
            StopAllCoroutines();
            StartCoroutine(ColorLerp(rawImage[scopeImageIndex].color, zoomOutColor));
        }
    }

    IEnumerator ColorLerp(Color currentColor, Color targetColor)
    {
        float lerpSpeed = 0f;
        while (rawImage[scopeImageIndex].color != targetColor)
        {
            rawImage[scopeImageIndex].color = Color.Lerp(rawImage[scopeImageIndex].color, targetColor, lerpSpeed);
            lerpSpeed += Time.deltaTime * .75f;
            yield return null;
        }
    }

    public bool CurrentParts(int[] indexes, int _count)
    {
        bool value = false;

        if (_count == 1)
        {
            if (muzzleIndex == indexes[0]
             && handleIndex == indexes[1]
             && stockIndex == indexes[3]
              ) value = true;

            else value = false;
        }

        else  if (_count  == 2)
        {
            if (muzzleIndex == indexes[0]
                && handleIndex == indexes[1]
                && magazineIndex == indexes[2]
                && stockIndex == indexes[3]
                ) value = true;

            else value = false;
        }

        else if (_count == 3)
        {
            if (muzzleIndex == indexes[0]
              && magazineIndex == indexes[2]
              && scopeIndex == indexes[4] - 1
              ) value = true;

            else value = false;
        }

        return value;
    }

    public bool CurrentPartsLevel3(int[] indexes, int _count)
    {
        bool value = false;
        if (_count == 1)
        {
            if (stockIndex == indexes[3]
              && scopeIndex == indexes[4]
              ) value = true;

            else value = false;
        }

        else if (_count == 2)
        {
            if (handleIndex == indexes[1]
                && magazineIndex == indexes[2]
                && scopeIndex == indexes[4]
                ) value = true;

            else value = false;
        }

        else if (_count == 3)
        {
            if (muzzleIndex == indexes[0]
              && scopeIndex == indexes[4]
              ) value = true;

            else value = false;
        }

        //Debug.Log(value);
        return value;
    }

    public void ShowCurrentParts()
    {
        //Debug.Log(muzzleIndex + " " + handleIndex + " " + magazineIndex  + " " + stockIndex  + " " + scopeImageIndex);
    }
}
