using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUIController : MonoBehaviour
{
    public static WeaponUIController Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<WeaponUIController>();
            return instance;
        }
    }
    private static WeaponUIController instance;

    public Image weaponImage;

    Weapon currentWeapon;

    [Header("UI Elements")]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI currentAmoText;
    public TextMeshProUGUI totalAmoText;
    public TextMeshProUGUI grenadeCountText;
    public TextMeshProUGUI flashBangCountText;
    public TextMeshProUGUI weaponFireModeText;

    [Header("Animator")]
    public Animator weaponNameChangeAnimator;

    private void Start()
    {
        ChangeWeaponUI();
    }

    public void UpdateUI()
    {
        currentWeapon = WeaponController.Instance.equippedGun;
        currentAmoText.text = currentWeapon.currentAmo.ToString("N0");
        totalAmoText.text = currentWeapon.totalAmo.ToString("N0");
        grenadeCountText.text = WeaponController.Instance.grenadeCount.ToString();
        flashBangCountText.text = WeaponController.Instance.flashBangCount.ToString();
        UpdateFireMode();
    }

    public void UpdateFireMode()
    {
        weaponFireModeText.text = WeaponController.Instance.auto == true ? "Auto" : "Bolt Action";
    }

    public Color loadedTextColor;
    public Color lowTextColor;

    public void ChangeWeaponCountTextColor(float _currentAmo)
    {
        if (_currentAmo <= currentWeapon.magazineAmo * 0.3f) currentAmoText.color = lowTextColor;
        else currentAmoText.color = loadedTextColor;
    }

    public void ChangeWeaponUI()
    {
        UpdateUI();
        currentWeapon = WeaponController.Instance.equippedGun;
        weaponImage.sprite = WeaponController.Instance.equippedGun.weaponImage;

        weaponNameText.text = currentWeapon.weaponName;
        weaponNameChangeAnimator.Play("Idle");
        weaponNameChangeAnimator.SetTrigger("Change");
        ChangeWeaponCountTextColor(currentWeapon.currentAmo);

        StopUIAnimation();
        StartUIAnimation();
    }

    public void StopUIAnimation()
    {
        StopAllCoroutines();
    }

    public void StartUIAnimation()
    {
        if (currentWeapon.weaponType == WeaponType.AK) currentWeapon.totalAmo = WeaponController.Instance.akBulletTotal;
        else if (currentWeapon.weaponType == WeaponType.HandGun) currentWeapon.totalAmo = WeaponController.Instance.hgBulletTotal;

        StartCoroutine(UIAnimation(currentWeapon.currentAmo, currentWeapon.totalAmo,
    WeaponController.Instance.grenadeCount, WeaponController.Instance.flashBangCount));
    }

    public bool isUIAnimPlaying;

    IEnumerator UIAnimation(float _targetCurrentAmo, float _targetTotalAmo, float _targetGrenade, float _targetFlashBang)
    {
        isUIAnimPlaying = true;

        float currentAmo = _targetCurrentAmo; float totalAmo = 0; float grenade = 0; float flashBang = 0;

        float lerpValue = 0;

        while (totalAmo != _targetTotalAmo)
        {
            currentAmo = Mathf.Lerp(currentAmo, _targetCurrentAmo, lerpValue);
            totalAmo = Mathf.Lerp(totalAmo, _targetTotalAmo, lerpValue);
            //grenade = Mathf.Lerp(grenade, _targetGrenade, lerpValue);
            //flashBang = Mathf.Lerp(flashBang, _targetFlashBang, lerpValue);
            lerpValue += Time.deltaTime;

            currentAmoText.text = currentAmo.ToString("N0");
            totalAmoText.text = totalAmo.ToString("N0");
            //grenadeCountText.text = grenade.ToString("N0");
            //flashBangCountText.text = flashBang.ToString("N0");
            yield return null;
        }
        isUIAnimPlaying = false;
    }
}
