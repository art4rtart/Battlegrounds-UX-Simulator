using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalSystem : MonoBehaviour
{
    public static DecalSystem Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<DecalSystem>();
            return instance;
        }
    }
    private static DecalSystem instance;

    public GameObject decalProjector;

    [Header("Decal Settings")]
    public Decals[] decals;

    [Header("Splatter Settings")]
    public bool splatterEnabled;
    public SplatterDecals[] splatterDecals;
    public GameObject splatterParticleSystem;
    public float splatterRange;
    public LayerMask splatterMask;

    [Header("Bleed Settings")]
    public bool bleedingEnabled;
    public GameObject bleedParticleSystem;
    [Range(0, 100)]
    public float bleedChance;
    public LayerMask layersToSpawnBleedEffects;

    private void SetParentObject(GameObject child, GameObject parent)
    {
        if (parent != null)
            child.transform.SetParent(parent.transform);
    }

    private void DrawDecalProjector(Vector3 position, Quaternion rotation, RaycastHit hit, Decals _decals)
    {
        GameObject projector = ObjectPoolerH.instance.GetPooledObject("Blood Decal");

        if (projector != null && _decals.decals != null)
        {
            projector.transform.position = position;
            projector.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            projector.SetActive(true);

            int randIndex = Random.Range(0, _decals.decals.Length);
            Material randMaterial = _decals.decals[randIndex];

            Projector _projector = projector.GetComponentInChildren<Projector>();
            _projector.material = randMaterial;
            _projector.orthographicSize = _decals.decalSize;
            _projector.farClipPlane = _decals.decalDepth;

            SetParentObject(projector, hit.collider.gameObject);
        }
        // StartCoroutine(projector.transform.GetChild(0).GetComponent<DecalDissove>().ChangeDissolveRate());
    }

    public void DrawSplatterProjector(Vector3 position, Quaternion rotation, GameObject splatterParent, RaycastHit hit, SplatterDecals _splatterDecals)
    {
        for (int i = 0; i < _splatterDecals.numberOfDecals; i++)
        {
            GameObject projector = ObjectPoolerH.instance.GetPooledObject("Blood Decal");
            if (projector != null)
            {
                projector.transform.position = position;
                projector.transform.rotation = Quaternion.FromToRotation(Vector3.forward, -hit.normal);
                projector.SetActive(true);

                SetParentObject(projector, splatterParent);

                projector.transform.localPosition = GenerateRandomSplatterSpread(_splatterDecals.horizontalSpread, _splatterDecals.verticalSpread);

                int randIndex = Random.Range(0, _splatterDecals.decals.Length);
                Material randMaterial = _splatterDecals.decals[randIndex];

                Projector _projector = projector.GetComponentInChildren<Projector>();
                _projector.material = randMaterial;
                _projector.orthographicSize = _splatterDecals.decalSize;
                _projector.farClipPlane = _splatterDecals.decalDepth;
            }
            // StartCoroutine(projector.transform.GetChild(0).GetComponent<DecalDissove>().ChangeDissolveRate());
        }
    }

    public void LeaveExitDecal(Vector3 position, Quaternion rotation, RaycastHit hit)
    {
        DrawDecalProjector(position, rotation, hit, CheckOverrides(hit));

        if (splatterEnabled)
        {
            SplatterFromPoint(hit.point, Camera.main.transform.forward, hit);

            if (splatterParticleSystem != null)
            {
                SpawnSplatterParticle(hit.point, Camera.main.transform.forward, hit.collider.gameObject);
            }
        }
    }

    public void LeaveDecal(Vector3 position, Quaternion rotation, RaycastHit hit, bool isEntry)
    {
        if (isEntry)
        {
            DrawDecalProjector(position, rotation, hit, CheckOverrides(hit));
            if (bleedingEnabled && 1 << hit.transform.gameObject.layer == layersToSpawnBleedEffects.value)
            {
                SpawnBleedParticle(position, rotation, hit.collider.gameObject);
            }
        }
        else if (!isEntry)
        {
            LeaveExitDecal(position, rotation, hit);
        }
    }

    private Decals CheckOverrides(RaycastHit hit)
    {
        Decals defaultDecals = null;
        string tag = hit.collider.tag;

        for (int i = 0; i < decals.Length; i++)
        {
            if (decals[i].overrideTag == "")
            {
                defaultDecals = decals[i];
            }

            if (tag == decals[i].overrideTag)
            {
                return decals[i];
            }
        }

        if (decals.Length > 0)
        {
            defaultDecals = decals[0];
        }

        return defaultDecals;
    }

    private SplatterDecals CheckSplatterOverrides(RaycastHit hit)
    {
        SplatterDecals defaultDecals = null;
        string tag = hit.collider.tag;

        for (int i = 0; i < splatterDecals.Length; i++)
        {
            if (splatterDecals[i].overrideTag == "")
            {
                defaultDecals = splatterDecals[i];
            }

            if (tag == splatterDecals[i].overrideTag)
            {
                return splatterDecals[i];
            }
        }

        if (splatterDecals.Length > 0)
        {
            defaultDecals = splatterDecals[0];
        }

        return defaultDecals;
    }

    public void SpawnBleedParticle(Vector3 position, Quaternion direction, GameObject parent)
    {
        float randomNumber = Random.Range(0, 100);
        if (randomNumber <= bleedChance)
        {
            GameObject bleedParent = new GameObject("BleedParent");
            bleedParent.AddComponent<DestroyObjectH>();
            bleedParent.GetComponent<DestroyObjectH>().SetState(true, 1.5f);
            Instantiate(bleedParticleSystem, position, direction, bleedParent.transform);

            SetParentObject(bleedParent, parent);
        }
    }

    private void SpawnSplatterParticle(Vector3 postion, Vector3 direction, GameObject parent)
    {
        GameObject splatterParent = new GameObject("SplatterParent");
        splatterParent.AddComponent<DestroyObjectH>();
        splatterParent.GetComponent<DestroyObjectH>().SetState(true, 3f);
        //Instantiate(splatterParticleSystem, postion, Quaternion.FromToRotation(Vector3.forward, direction), splatterParent.transform);

        GameObject splatterObj = ObjectPoolerH.instance.GetPooledObject("Blood Splatter Particle");
        splatterObj.transform.position = postion;
        splatterObj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, direction);
        splatterObj.transform.SetParent(splatterParent.transform);
        SetParentObject(splatterParent, parent);
        splatterObj.SetActive(true);
    }

    private void SplatterFromPoint(Vector3 position, Vector3 rotation, RaycastHit _hit)
    {
        RaycastHit hit;

        if (Physics.Raycast(position, rotation, out hit, splatterRange, splatterMask))
        {
            Vector3 spawnPoint = hit.point + hit.normal.normalized * 0.001f;
            GameObject parentDecal = new GameObject("SplatterObject");
            parentDecal.transform.position = spawnPoint;
            parentDecal.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

            DrawSplatterProjector(spawnPoint, Quaternion.FromToRotation(Vector3.forward, hit.normal), parentDecal, hit, CheckSplatterOverrides(_hit));

            SetParentObject(parentDecal, hit.collider.gameObject);
        }
    }

    private Vector3 GenerateRandomSplatterSpread(float horizontal, float vertical)
    {
        Vector3 offsetSpawnPoint;
        offsetSpawnPoint.x = Random.Range(-horizontal, horizontal);
        offsetSpawnPoint.y = Random.Range(-vertical, vertical);
        offsetSpawnPoint.z = 0f;

        return offsetSpawnPoint;
    }
}

[System.Serializable]
public class Decals
{
    [Tooltip("An array of materials that are randomly selected from when a decal is placed. Materials must be using a Projector shader.")]
    public Material[] decals;
    [Tooltip("The size of the projection onto surfaces.")]
    public float decalSize;
    [Tooltip("Leave a blank value to make a set of decals the default. If no blank override string exists then the first decal set in the array is used instead.")]
    public string overrideTag;
    [Tooltip("Adjusts the far clipping plane of the projector. Set higher if decals are fadding out or lower if decals are being placed on multiple sides of an object.")]
    public float decalDepth;
}

[System.Serializable]
public class SplatterDecals
{
    [Tooltip("An array of materials that are randomly selected from when a decal is placed. Materials must be using a Projector shader.")]
    public Material[] decals;
    [Tooltip("The size of the projection onto surfaces.")]
    public float decalSize;
    [Tooltip("Adjusts the far clipping plane of the projector. Set higher if decals are fadding out or lower if decals are being placed on multiple sides of an object.")]
    public float decalDepth;
    [Tooltip("How many splatter decals are created per hit.")]
    public int numberOfDecals;
    [Tooltip("Adjusts the random spread for the placement of the splatter decals.")]
    public float horizontalSpread, verticalSpread;
    [Tooltip("Leave a blank value to make a set of decals the default. If no blank override string exists then the first decal set in the array is used instead.")]
    public string overrideTag;
}

