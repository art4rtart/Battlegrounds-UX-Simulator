using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDissove : MonoBehaviour
{
    Projector projector;

    public float dissolveSpeed = 2.5f;
    public bool dissolve;

    private void Awake()
    {
        projector = GetComponent<Projector>();
    }

    void Start()
    {
        StopAllCoroutines();
        if (dissolve) StartCoroutine(ChangeDissolveRate());
    }

    public IEnumerator ChangeDissolveRate()
    {
        Material newMaterial = new Material(projector.material);
        if (newMaterial == null) Debug.Log("OK");

        float rate = 1;

        while (rate > 0.2)
        {
            dissolveSpeed += Time.deltaTime * 1.5f;

            rate -= Time.deltaTime * dissolveSpeed;
            newMaterial.SetFloat("_Dissolved", rate);
            projector.material = newMaterial;
            yield return null;
        }
    }
}
