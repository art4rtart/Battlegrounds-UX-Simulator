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
    public TextMeshProUGUI[] text;

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
        if (this.gameObject.activeSelf) this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StopAllCoroutines();

        weaponName.text = WeaponController.Instance.equippedGun.weaponName + "<size=15><color=#ABABAB>     " +
            WeaponController.Instance.equippedGun.weaponShootType + "</size></color>";

        weaponBulletName.text = WeaponController.Instance.equippedGun.weaponAmmoName;
        weaponCurrentBullet.text = WeaponController.Instance.equippedGun.currentAmo.ToString();
        weaponTotalBullet.text = WeaponController.Instance.equippedGun.totalAmo.ToString();

        weaponImage.sprite = WeaponController.Instance.equippedGun.weaponImage;
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
}
