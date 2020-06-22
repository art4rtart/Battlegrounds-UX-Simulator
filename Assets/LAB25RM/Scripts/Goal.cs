using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static Goal Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<Goal>();
            return instance;
        }
    }
    private static Goal instance;

    Projector projector;
    SphereCollider sCollider;
    public string nextLevel;
    public bool goal;

    private void Awake()
    {
        projector = GetComponent<Projector>();
        sCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            goal = true;
            sCollider.enabled = false;
            StartCoroutine(CloseCircle());
        }
    }

    IEnumerator CloseCircle()
    {
        float value = projector.orthographicSize;

		PlayerInfoManager.Instance.SceneCompleted();
        DialougeManager.Instance.animator.SetTrigger("FadeOut");
        DialougeManager.Instance.levelName = nextLevel;

        while (value > 0)
        {
            value -= Time.deltaTime * 2f;
            projector.orthographicSize = value;
            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}
