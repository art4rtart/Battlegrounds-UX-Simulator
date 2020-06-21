using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    Projector projector;
    SphereCollider sCollider;
    public Material material;

    public Color EnterColor;
    public Color ExitColor;

    public EnemyChecker enemyChecker;

    float originSize;
    public bool arrived;

	public bool tutorial;

    private void Awake()
    {
        projector = GetComponent<Projector>();
        sCollider = GetComponent<SphereCollider>();
        material = projector.material;
        originSize = projector.orthographicSize;
        if(!tutorial) projector.orthographicSize = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
			arrived = true;
            material.SetColor("_Color", EnterColor);

			if (tutorial) return;
			for (int i = 0; i < enemyChecker.transform.childCount; i++)
            {
                CapsuleCollider cCollider = enemyChecker.transform.GetChild(i).GetChild(1).GetComponent<CapsuleCollider>();
                cCollider.enabled = true;
                SkinnedMeshRenderer skinnedMesh = enemyChecker.transform.GetChild(i).GetChild(1).GetComponent<SkinnedMeshRenderer>();
                skinnedMesh.materials[1].SetFloat("_Dissolved", 0f);
            }
		}
    }

    private void OnTriggerExit(Collider other)
    {
		if (other.gameObject.CompareTag("Player"))
        {
			material.SetColor("_Color", ExitColor);

			if (tutorial) return;
            for (int i = 0; i < enemyChecker.transform.childCount; i++)
            {
                CapsuleCollider cCollider = enemyChecker.transform.GetChild(i).GetChild(1).GetComponent<CapsuleCollider>();
                cCollider.enabled = false;
                SkinnedMeshRenderer skinnedMesh = enemyChecker.transform.GetChild(i).GetChild(1).GetComponent<SkinnedMeshRenderer>();
                skinnedMesh.materials[1].SetFloat("_Dissolved", 1f);
            }
        }
    }

    public void OpenDestination()
    {
        StartCoroutine(OpenCircle());
    }

    IEnumerator OpenCircle()
    {
        float value = 0f;

        while (value < originSize)
        {
            value += Time.deltaTime * 2f;
            projector.orthographicSize = value;
            yield return null;
        }
    }
}
