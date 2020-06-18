using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ItemType { None, M4, AK, HandGun, Grenade, FlashBang, Parts, NoSafety };
public enum PartsType { None, Muzzle, Handle, Magazine, Reverse, Scope };
public class Item : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemType itemType;
    public PartsType partType;

    public GameObject item;
    public Sprite itemImage;
    public Sprite itemPartsImage;
    public string name;
    public string itemUseType;
    public string tag;
    public int quantity;
    public string weaponCurrentState;

    public GameObject panel;
    public LayerMask humanLayermask;

    [Header("UI Settings")]
    public Image itemImagePanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemUseTypeText;
    public TextMeshProUGUI weaponCurrentStateText;
    public TextMeshProUGUI itemQuantityText;

    public float itemRadius;
    Animator animator;

    public BGItemContent bgItemContent;
    //Ammo , Throwing

    // battleground
    public bool isFound;

    private void Awake()
    {
        if (itemType == ItemType.None) Debug.Log("Item type is null");
        if(tag != null) this.transform.tag = tag;
        panel = this.gameObject.transform.GetChild(0).gameObject;

        animator = GetComponent<Animator>();

        itemImagePanel.sprite = itemImage;
        itemNameText.text = name;
        itemUseTypeText.text = itemUseType;
        itemQuantityText.text = "x " + quantity.ToString();
        weaponCurrentStateText.text = weaponCurrentState;
    }

    private void Start()
    {
        if (!item) return;
        item.transform.position = this.transform.position + Vector3.up * item.transform.localScale.x * .5f;
        panel.transform.position = this.transform.position + Vector3.up * (item.transform.localScale.y+ .25f);
    }

    public void AddItem(Weapon weapon)
    {
        bool useUIAnimation = false;
        switch(itemType)
        {
            case ItemType.AK:
                WeaponController.Instance.akBulletTotal += quantity;
                if(weapon.weaponType == WeaponType.AK) useUIAnimation = true;
                break;
            case ItemType.HandGun:
                WeaponController.Instance.hgBulletTotal += quantity;
                if (weapon.weaponType == WeaponType.HandGun) useUIAnimation = true;
                break;
            case ItemType.NoSafety:
                WeaponController.Instance.nsBulletTotal += quantity;
                if (weapon.weaponType == WeaponType.NoSafety) useUIAnimation = true;
                break;
            case ItemType.Grenade:
                WeaponController.Instance.grenadeCount += quantity;
                break;
            case ItemType.FlashBang:
                WeaponController.Instance.flashBangCount += quantity;
                break;
        }

        WeaponUIController.Instance.UpdateUI();
        if(useUIAnimation)WeaponUIController.Instance.StartUIAnimation();
    }

    public void DropItem(Weapon weapon)
    {
        bool useUIAnimation = false;
        switch (itemType)
        {
            case ItemType.AK:
                WeaponController.Instance.akBulletTotal -= quantity;
                if (weapon.weaponType == WeaponType.AK) useUIAnimation = true;
                break;
            case ItemType.HandGun:
                WeaponController.Instance.hgBulletTotal -= quantity;
                if (weapon.weaponType == WeaponType.HandGun) useUIAnimation = true;
                break;
            case ItemType.NoSafety:
                WeaponController.Instance.nsBulletTotal -= quantity;
                if (weapon.weaponType == WeaponType.NoSafety) useUIAnimation = true;
                break;
            case ItemType.Grenade:
                WeaponController.Instance.grenadeCount -= quantity;
                break;
            case ItemType.FlashBang:
                WeaponController.Instance.flashBangCount -= quantity;
                break;
        }

        WeaponUIController.Instance.UpdateUI();
        if (useUIAnimation) WeaponUIController.Instance.StartUIAnimation();
    }

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, itemRadius, humanLayermask);
        bool active = hitColliders.Length != 0 ? true : false;

        if (active && !isFound && ItemDetector.Instance) {
            ItemDetector.Instance.groundItems.Add(this.gameObject.GetComponent<Item>());
            ItemDetector.Instance.UpdateGroundItem();
            isFound = true;
        }
        if (!active && ItemDetector.Instance) {
            ItemDetector.Instance.groundItems.Remove(this.gameObject.GetComponent<Item>());
            ItemDetector.Instance.UpdateGroundItem();
            isFound = false;
        }

        if (!animator.enabled) return;
        panel.SetActive(active);
        animator.SetBool("Show", active);

        if (!active) return;
        panel.transform.LookAt(hitColliders[0].transform);
        panel.transform.eulerAngles += Vector3.up * 180f;
    }

    public bool drawGizmos;
    void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, itemRadius);
    }
}
