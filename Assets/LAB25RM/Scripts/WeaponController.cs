using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<WeaponController>();
            return instance;
        }
    }
    private static WeaponController instance;

    public Transform cam;
    [HideInInspector] public Animator weaponAnimator;
    public Animator[] animators;
    [HideInInspector] public Weapon equippedGun;

    [Header("Gun Settings")]
    public Weapon[] startingGun;
    public int currentGunIndex;
    public int previousGunIndex;

    [Header("Throwing Objects")]
    public float akBulletTotal;
    public float hgBulletTotal;
    public float nsBulletTotal;
    public int grenadeCount;
    public int flashBangCount;

    [SerializeField]
    [Header("Raycast")]
    public float shootRange = 1000f;
    public bool isFiring;
    public LayerMask layersToHit;
    public LayerMask stopLayer;
    public bool isReadyToFire;

    [Header("Zoom")]
    public Material vignette;
    [SerializeField] private float zoomRange = 20f;
    [SerializeField] private float zoomSpeed = 1f;
    public bool isZooming;
    bool aiming;

    [Header("Reload")]
    public bool isReloading;
    public bool isInspecting;
	public Animator reloadAnim;
	public float reloadSpeed = 1f;

	[Header("Weapon Sway")]
    [Tooltip("Toggle weapon sway.")]
    public bool weaponSway;
    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothValue = 4.0f;
    private Vector3 initialSwayPosition;

    [Header("Item Find")]
    public float itemFindRadius;
    public float itemFindRayDistance = 50f;
    public LayerMask itemLayermask;
    public LayerMask pointerLayerMask;

    [Header("Sound")]
    [HideInInspector] public AudioSource weaponAudioSource;
    public AudioClip[] audioClip;
    public AudioClip gainClip;
	public AudioClip[] reloadSound;
    int audioClipIndex;

	[Header("Sound")]
	public AudioClip normalSound;
	public AudioClip silenceSound;

	void Awake()
    {
        if (equippedGun == null) equippedGun = startingGun[currentGunIndex - 1];
        weaponAnimator = this.transform.GetChild(currentGunIndex-1).GetComponent<Animator>();
        weaponAudioSource = GetComponent<AudioSource>();
        cam = Camera.main.transform;

        initialSwayPosition = transform.localPosition;
        InteractUIController.Instance.HideUI();
    }

    void Update()
    {
        if (GameTimeController.isPaused) return;
        if (FirstPersonController.Instance.isDead) return;

        PlayerInput();

        // UI
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, itemFindRadius, itemLayermask);
        bool isItemArround = hitColliders.Length != 0 ? true : false;

        if (!isItemArround) { InteractUIController.Instance.HideUI(); return; }

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, itemFindRayDistance, pointerLayerMask))
        {
            if (!hit.transform.CompareTag("Item"))
            {
                InteractUIController.Instance.HideUI();
                return;
            }

            string name = hit.transform.parent.GetComponent<Item>().name;
            int quantity = hit.transform.parent.GetComponent<Item>().quantity;
            Sprite sprite = hit.transform.parent.GetComponent<Item>().itemImage;
            InteractUIController.Instance.ShowItemUI(name, quantity, sprite);

            if (Input.GetKeyDown(KeyCode.F)) Gain(hit.transform);
        }
    }

    Item item;
    public bool isBattleground;
    public bool isOurGame;
    //by f key
    void Gain(Transform _hit)
    {
        item = _hit.transform.parent.gameObject.GetComponent<Item>();
        if (isBattleground) {
            ItemDetector.Instance.groundItems.Remove(item);
            ItemDetector.Instance.inventoryItems.Add(item);
            ItemDetector.Instance.UpdateGroundItem();
            ItemDetector.Instance.UpdateInventoryItems();
            item.transform.SetParent(ItemDetector.Instance.itemHolder);
        }
        if (isOurGame)
        {
            CyberPunkItemManger.Instance.AddItemToInvenotry(item);
            item.transform.SetParent(CyberPunkItemManger.Instance.itemHolder);
        }
        item.AddItem(equippedGun);
        item.gameObject.SetActive(false);

        weaponAudioSource.PlayOneShot(gainClip);
        InteractUIController.Instance.HideUI();
    }

    // by drag and drop
    public void Gain(Item _item)
    {
        if (isBattleground) {
            ItemDetector.Instance.groundItems.Remove(_item);
            ItemDetector.Instance.inventoryItems.Add(_item);
            ItemDetector.Instance.UpdateGroundItem();
            ItemDetector.Instance.UpdateInventoryItems();
            _item.transform.SetParent(ItemDetector.Instance.itemHolder);
        }
        _item.AddItem(equippedGun);
        _item.gameObject.SetActive(false);

        weaponAudioSource.PlayOneShot(gainClip);
    }

    //temp
    public void Drop(Item _item)
    {
        _item.gameObject.transform.SetParent(null);
        _item.DropItem(equippedGun);
        _item.gameObject.SetActive(true);
		weaponAudioSource.PlayOneShot(gainClip);
	}

    void LateUpdate()
    {
        if (GameTimeController.isPaused) return;

        if (weaponSway)
        {
            float movementX = -Input.GetAxis("Mouse X") * swayAmount;
            float movementY = -Input.GetAxis("Mouse Y") * swayAmount;

            movementX = Mathf.Clamp(movementX, -maxSwayAmount, maxSwayAmount);
            movementY = Mathf.Clamp(movementY, -maxSwayAmount, maxSwayAmount);

            Vector3 finalSwayPosition = new Vector3(movementX, movementY, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalSwayPosition + initialSwayPosition, Time.deltaTime * swaySmoothValue);
        }
    }

    public bool auto = true;
    public bool shootOnce = false;
    void PlayerInput()
    {
        // Fire Gun
        if (Input.GetButtonDown("Fire1") && !isRunning && !isReloading) isFiring = true;
        else if (Input.GetButton("Fire1") && !isRunning && !isReloading) Shoot();
        else if (Input.GetButtonUp("Fire1") && !isRunning && !isReloading) { isFiring = false; if (!aiming && !isFiring && isZooming) Zoom(); shootOnce = false; }

        // Zoom
        if (Input.GetButtonDown("Fire2") && isReadyToFire && !isFiring && !isRunning && !isReloading) { Zoom(); aiming = true; }
        else if (Input.GetButtonUp("Fire2") && isReadyToFire && !isFiring && !isRunning && !isReloading) { Zoom(); if (isZooming) Zoom(); }

        // Relaod
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isZooming && !isInspecting && equippedGun.totalAmo > 0) Reload();

        // Inspect
        if (Input.GetKeyDown(KeyCode.T)) weaponAnimator.SetTrigger("Inspect");

        // Shooting Mode
        if (Input.GetKeyDown(KeyCode.B)) { auto = !auto; WeaponUIController.Instance.UpdateUI(); }

        // Change Weapon
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentGunIndex != 1 && !isFiring) ChangeWeapon(1);
        //else if (Input.GetKeyDown(KeyCode.Alpha2) && currentGunIndex != 2 && !isFiring) ChangeWeapon(2);
        //else if (Input.GetKeyDown(KeyCode.Alpha3) && currentGunIndex != 3 && !isFiring) ChangeWeapon(3);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isMoving = horizontal + vertical != 0 ? true : false;

        if (isMoving && !isRunning) weaponAnimator.SetBool("Walk", true);
        else weaponAnimator.SetBool("Walk", false);

        if (Input.GetKey(KeyCode.LeftShift) && isMoving && !isReloading) isRunning = true;
        else isRunning = false;

        if (isRunning) weaponAnimator.SetBool("Run", true);
        else  weaponAnimator.SetBool("Run", false);
    }
    bool isRunning;

    public void ChangeWeapon(int index)
    {
        weaponAudioSource.clip = audioClip[2];
        weaponAudioSource.Play();
        weaponAnimator.SetBool("Holster", true);
        previousGunIndex = index;
        Invoke("ActiveNextWeapon", 0.35f);
    }

    void ActiveNextWeapon()
    {
        equippedGun = startingGun[previousGunIndex - 1];
        weaponAnimator.gameObject.SetActive(false);

        WeaponUIController.Instance.ChangeWeaponUI();

        weaponAnimator = animators[previousGunIndex - 1];
        weaponAnimator.gameObject.SetActive(true);

        currentGunIndex = previousGunIndex;
        weaponAnimator.SetBool("Holster", false);
    }

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, shootRange)) {
            if (auto)  {
                if (equippedGun != null && isReadyToFire) equippedGun.Shoot(hit, cam, layersToHit, stopLayer, weaponAnimator, isZooming, audioClipIndex);
            }
            else {
                audioClipIndex = 1;
                if (equippedGun != null && isReadyToFire && !shootOnce) equippedGun.Shoot(hit, cam, layersToHit, stopLayer, weaponAnimator, isZooming, audioClipIndex);
                shootOnce = true;
            }
        }
        if (Input.GetButtonUp("Fire2")) aiming = false;
    }

    public void Reload()
    {
		if (auto) WeaponUIController.Instance.autoCrossHair.SetActive(false);
        else WeaponUIController.Instance.boltActionCrossHair.SetActive(false);

        weaponAnimator.Play("Reload Out Of Ammo", 0, 0f);

		ReloadEffectCoroutine = ReloadEffect(.5f, 1f, weaponAnimator.GetCurrentAnimatorClipInfo(0).Length, equippedGun.magazineAmo);
        StartCoroutine(ReloadEffectCoroutine);

        // Sound
        //      mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
        //      mainAudioSource.Play();

        //      //If out of ammo, hide the bullet renderer in the mag
        //      //Do not show if bullet renderer is not assigned in inspector
        //      if (bulletInMagRenderer != null)
        //      {
        //          bulletInMagRenderer.GetComponent
        //          <SkinnedMeshRenderer>().enabled = false;
        //          //Start show bullet delay
        //          StartCoroutine(ShowBulletInMag());
        //      }
        //  } 
        //else 
        //{
        //	//Play diff anim if ammo left
        //	anim.Play("Reload Ammo Left", 0, 0f);

        //	mainAudioSource.clip = SoundClips.reloadSoundAmmoLeft;
        //	mainAudioSource.Play();

        //	//If reloading when ammo left, show bullet in mag
        //	//Do not show if bullet renderer is not assigned in inspector
        //	if (bulletInMagRenderer != null) 
        //	{
        //		bulletInMagRenderer.GetComponent
        //              <SkinnedMeshRenderer>().enabled = true;
        //	}
        //}
        ////Restore ammo when reloading
        //currentAmmo = ammo;
        //outOfAmmo = false;
    }

    public IEnumerator ReloadEffectCoroutine;
    IEnumerator ReloadEffect(float takeOutTime, float reloadTime, float totalTime, float _magazineAmo)
    {
        isReloading = true;
        yield return new WaitForSeconds(takeOutTime);

        float _totalAmo = equippedGun.totalAmo - (equippedGun.magazineAmo - equippedGun.currentAmo);
        float lerpValue = 0;

        float targetAmo = _magazineAmo;

        equippedGun.totalAmo += equippedGun.currentAmo;
        equippedGun.currentAmo = 0;
        WeaponUIController.Instance.UpdateUI();

        yield return new WaitForSeconds(reloadTime);

        if (targetAmo > equippedGun.totalAmo) targetAmo = equippedGun.totalAmo;
        else targetAmo = equippedGun.magazineAmo;

        while (equippedGun.currentAmo < targetAmo) {
            equippedGun.currentAmo  = Mathf.Clamp(equippedGun.currentAmo += Time.deltaTime * 50f, 0f, equippedGun.magazineAmo );

            equippedGun.totalAmo = (int)Mathf.Clamp(Mathf.Lerp(equippedGun.totalAmo, _totalAmo, lerpValue), 0f, Mathf.Infinity);
            lerpValue += Time.deltaTime;

            WeaponUIController.Instance.UpdateUI();
            WeaponUIController.Instance.ChangeWeaponCountTextColor(equippedGun.currentAmo);
            yield return null;
        }

        if (equippedGun.weaponType == WeaponType.AK) akBulletTotal = equippedGun.totalAmo;
        else if (equippedGun.weaponType == WeaponType.HandGun) hgBulletTotal = equippedGun.totalAmo;
        else if (equippedGun.weaponType == WeaponType.NoSafety) nsBulletTotal = equippedGun.totalAmo;

        yield return new WaitForSeconds(totalTime - takeOutTime - reloadTime - .5f);
        isReloading = false;

        yield return new WaitForSeconds(.5f);
        WeaponUIController.Instance.UpdateUI();
    }

    public void Zoom()
    {
        isFiring = false;
        isZooming = !isZooming; StopAllCoroutines();
        weaponAnimator.SetBool("Aim", isZooming);

        if (isZooming) StartCoroutine(ZoomEffect(1.05f, 1.75f, zoomRange, zoomSpeed));
        else StartCoroutine(ZoomEffect(1.2f, 5f, 60f, zoomSpeed));
    }

    IEnumerator ZoomEffect(float _targetRadius, float _targetHardneess, float _targetFieldofView, float _zoomSpeed)
    {
        float lerpValue = 0f;

        float currentRadius = vignette.GetFloat("_Radius");
        float currentHardness = vignette.GetFloat("_Hardness");
        float currentFieldofView = Camera.main.fieldOfView;

        while (vignette.GetFloat("_Radius") != _targetRadius)
        {
            vignette.SetFloat("_Radius", Mathf.Lerp(currentRadius, _targetRadius, lerpValue));
            vignette.SetFloat("_Hardness", Mathf.Lerp(currentHardness, _targetHardneess, lerpValue));
            Camera.main.fieldOfView = Mathf.Lerp(currentFieldofView, _targetFieldofView, lerpValue);

            lerpValue += Time.deltaTime * _zoomSpeed;
            yield return null;
        }
    }
}
