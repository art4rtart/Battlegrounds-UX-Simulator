using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CyberPunkUIController : MonoBehaviour
{
    public static CyberPunkUIController Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<CyberPunkUIController>();
            return instance;
        }
    }
    private static CyberPunkUIController instance;

    public Transform Handler;
    public Image[] image;

    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponCurrentBullet;
    public TextMeshProUGUI weaponTotalBullet;
    public TextMeshProUGUI weaponBulletName;
    public Image weaponImage;


    public PartsScrollController [] partsScrollController;

    public Transform layout;

    public CyberPunkItemContent currentContent;

    public bool isItemPicked;


    private void Awake()
    {
        // UI Edit Mode
        //if (this.gameObject.activeSelf) this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StopAllCoroutines();

        weaponName.text = WeaponController.Instance.equippedGun.weaponName + "<size=15><color=#ABABAB>     " +
            WeaponController.Instance.equippedGun.weaponShootType + "</size></color>";

        weaponBulletName.text = WeaponController.Instance.equippedGun.weaponAmmoName;
        weaponCurrentBullet.text = WeaponController.Instance.equippedGun.currentAmo.ToString();
        weaponTotalBullet.text = WeaponController.Instance.equippedGun.totalAmo.ToString();

        weaponImage.sprite = WeaponController.Instance.equippedGun.weaponImageInfoPanel;

        StartCoroutine(PingPongAlpha());
    }

    private void OnDisable()
    {
        for (int i = 0; i < partsScrollController.Length; i++)
        {
            //partsScrollController[i].InitializeScrollViewParent();
            //partsScrollController[i].isPined = false;
            //partsScrollController[i].hoverText.SetActive(true);
            //partsScrollController[i].scrollview.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    IEnumerator PingPongAlpha()
    {
        float alpha = 1f;

        Color imageColor = image[0].color;

        while (true)
        {
            for (int i = 0; i < image.Length; i++)
            {
                image[i].color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);
            }
            alpha = Mathf.PingPong(Time.time * 1f, 1f);
            yield return null;
        }
    }
}
