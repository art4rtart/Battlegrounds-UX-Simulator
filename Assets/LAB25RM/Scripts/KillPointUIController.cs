using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class KillPointUIController : MonoBehaviour
{
    public static KillPointUIController Instance {
        get {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<KillPointUIController>();
            return instance;
        }
    }
    private static KillPointUIController instance;

    [Header("UI Elements")]
    public TextMeshProUGUI enemyNameText;
    public TextMeshProUGUI killPointText;
    public TextMeshProUGUI enemyHitPointText;
    public TextMeshProUGUI offensiveKillText;

    public int killPoint;

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        //if(Input.GetKeyDown(KeyCode.C))
        //{
        //    AddKillPoint("PARASITE RANGER", 100);
        //}
    }

    public void AddKillPoint(string _enemyName, int _addPoint)
    {
        enemyNameText.text = _enemyName + "  +" + _addPoint;

        killPoint += _addPoint;

        enemyHitPointText.text = "Enemy Hit  <color=red>+" + _addPoint.ToString() + "</color>";

        int offensiveKill = Random.Range(80, 100);
        offensiveKillText.text = "Offensive Kill  <color=red>+" + offensiveKill.ToString() + "</color>";

        StopAllCoroutines();
        StartCoroutine(ShowPointAnimation());
        animator.SetBool("Show", true);
    }

    IEnumerator ShowPointAnimation()
    {
        int targetPoint = killPoint;

        float value = 0;
        float lerpSpeed = 0f;

        while (value != targetPoint)
        {
            value = Mathf.Lerp(value, targetPoint, lerpSpeed);
            killPointText.text = value.ToString("N0");
            lerpSpeed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(.5f);
        animator.SetBool("Show", false);
    }

    public void ShowKillPoint()
    {
        // enemy id + killpoint
        enemyNameText.text = 
        // total kill point (174)
        killPointText.text = killPoint.ToString();
        // damage (Enemey Hit)

        // total kill point
    }
}
