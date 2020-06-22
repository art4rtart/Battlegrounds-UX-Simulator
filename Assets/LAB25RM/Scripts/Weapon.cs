using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum WeaponType { None, AK, HandGun, NoSafety };
public class Weapon : MonoBehaviour
{
    [Header("Gun Spec")]
    public WeaponType weaponType;
    public float fireRate = 100;
    float nextShotTime;

    public Sprite weaponImage;
    public Sprite weaponImageInfoPanel;
    public string weaponName;
    public string weaponShootType;
    public float totalAmo = 0;
    public float currentAmo = 0;
    public float maxAmo = 0;
    public float magazineAmo = 0;
    public string weaponAmmoName;


    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2(.05f, .2f);
    public Vector2 recoilStrengthMinMax = new Vector2(5, 1);
    Vector2 recoilAngleMinMax;
    public float recoilMoveSettleTime = .1f;
    public float recoilRotationSettleTime = .1f;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;
    
    [Header("Muzzle")]
    public ParticleSystem muzzleFlash;
    public Transform muzzlePivotNormal;
    public Transform muzzlePivotAim;

    //[Header("Effects")]
    //public Transform shell;
    //public Transform shellEjection;

    //bool triggerReleasedSinceLastShot;
    //int shotsRemainingInBurst;
    //int projectilesRemainingInMag;
    //bool isReloading;

    public float recoilStrength = 1f;

    public float damageRate = 25f;
    public float shootRange = 2000f;

    [Header("Bullet Penetration Settings")]
    public bool bulletPenetration;
    public int numberOfPenetrations;
    public bool leavesExitDecals;

    [Header("Parts")]
    [HideInInspector] public PartsController partsController;

    void OnEnable()
    {
        muzzleFlash.Stop();
    }

    private void Awake()
    {
        magazineAmo = currentAmo;
        partsController = GetComponent<PartsController>();
    }

    private void LateUpdate()
    {
        if (GameTimeController.isPaused) return;
        RecoilThisObject(Camera.main.transform, new Vector3(recoilStrength, recoilStrength, recoilStrength));
    }

    void FireInfo(Animator wepaonAnim, string animNameInfo, Vector2 _recoilAngleMinMax, Transform _muzzleFalsh)
    {
        recoilAngleMinMax = _recoilAngleMinMax;
        muzzleFlash.transform.localPosition = _muzzleFalsh.localPosition;
        wepaonAnim.Play(animNameInfo, 0, 0f);
    }

    public void Shoot(RaycastHit hit, Transform cam, LayerMask layersToHit, LayerMask stopLayer, Animator wepaonAnim, bool isZooming, int audioIndex)
    {
        if (currentAmo >= 1 && Time.time > nextShotTime)
        {
            nextShotTime = Time.time + fireRate / 1000;

            muzzleFlash.Play();

            WeaponController.Instance.weaponAudioSource.PlayOneShot(WeaponController.Instance.audioClip[audioIndex]);

            if (!isZooming) FireInfo(wepaonAnim, "Fire", new Vector2(recoilStrengthMinMax.x, recoilStrengthMinMax.x), muzzlePivotNormal.transform);
            else FireInfo(wepaonAnim, "Aim Fire", new Vector2(recoilStrengthMinMax.y, recoilStrengthMinMax.y), muzzlePivotAim.transform);

            if (WeaponUIController.Instance.isUIAnimPlaying) WeaponUIController.Instance.StopUIAnimation();
            if (WeaponController.Instance.isReloading) { WeaponController.Instance.isReloading = false; StopCoroutine(WeaponController.Instance.ReloadEffectCoroutine); }

            currentAmo -= 1f;

            WeaponUIController.Instance.ChangeWeaponCountTextColor(currentAmo);
            WeaponUIController.Instance.UpdateUI();

            int stoppingIndex = 0;
            int passes = 0;

            RaycastHit[] entryHits = Physics.RaycastAll(cam.position, cam.forward, shootRange, layersToHit).OrderBy(h => h.distance).ToArray();

            Parasite parasite = entryHits[0].transform.GetComponent<Parasite>();
            SniperTurret sniperTurret = entryHits[0].transform.GetComponent<SniperTurret>();
			Debug.Log(entryHits[0].transform.name);
            if (parasite != null) parasite.ApplyDamage(damageRate);
            if (sniperTurret != null) sniperTurret.ApplyDamage(damageRate);

            for (int i = 0; i < entryHits.Length; i++)
            {
                if (1 << entryHits[i].transform.gameObject.layer == stopLayer.value)
                {
                    Vector3 spawnPos = entryHits[i].point;
                    DecalSystem.Instance.LeaveDecal(spawnPos, Quaternion.FromToRotation(Vector3.forward, entryHits[i].normal), entryHits[i], true);
                    stoppingIndex = i;
                    break;
                }
                if (passes == numberOfPenetrations)
                {
                    Vector3 spawnPos = entryHits[i].point;
                    DecalSystem.Instance.LeaveDecal(spawnPos, Quaternion.FromToRotation(Vector3.forward, entryHits[i].normal), entryHits[i], true);
                    stoppingIndex = i;
                    break;
                }
                else
                {
                    Vector3 spawnPos = entryHits[i].point;
                    DecalSystem.Instance.LeaveDecal(spawnPos, Quaternion.FromToRotation(Vector3.forward, entryHits[i].normal), entryHits[i], true);
                    passes++;
                }
            }

            if (entryHits.Length != 0 && leavesExitDecals)
            {
                if (1 << entryHits[stoppingIndex].transform.gameObject.layer == stopLayer.value || passes == numberOfPenetrations)
                {
                    float distance = Vector3.Distance(cam.position, entryHits[stoppingIndex].point);
                    RaycastHit[] exitHits = Physics.RaycastAll(entryHits[stoppingIndex].point, -cam.forward, distance, layersToHit).OrderByDescending(h => h.distance).ToArray();

                    for (int i = 0; i < exitHits.Length; i++)
                    {
                        Vector3 spawnPos = exitHits[i].point;
                        DecalSystem.Instance.LeaveDecal(spawnPos, Quaternion.FromToRotation(Vector3.forward, exitHits[i].normal), exitHits[i], false);
                    }
                }
                else
                {
                    RaycastHit[] exitHits = Physics.RaycastAll(cam.position + (cam.forward * shootRange), -cam.forward, shootRange, layersToHit).OrderByDescending(h => h.distance).ToArray();

                    for (int i = 0; i < exitHits.Length; i++)
                    {
                        Vector3 spawnPos = exitHits[i].point;
                        DecalSystem.Instance.LeaveDecal(spawnPos, Quaternion.FromToRotation(Vector3.forward, exitHits[i].normal), exitHits[i], false);
                    }
                }
            }
            //

            recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);
        }
    }

    void RecoilThisObject(Transform _gameObject, Vector3 _strength)
    {
        Vector3 recoilPower = new Vector3(Random.Range(-1f, 1f) * _strength.x, Random.Range(-1f, 1f) * _strength.y, Random.Range(-1f, 1f) * _strength.z);

        _gameObject.localPosition = Vector3.SmoothDamp(_gameObject.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleTime);
        _gameObject.parent.localEulerAngles = _gameObject.localEulerAngles + recoilPower * recoilAngle;
    }
}