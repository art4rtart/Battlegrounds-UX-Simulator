using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parasite : MonoBehaviour
{
    public Character character = new Character();

    public bool startDissolve;
    public float damagedRate = 0;

    ParasiteController parasiteController;
    bool isStopingAnimator;

    Material material;

    [HideInInspector] public RePoolObjectH repoolObject;
	public bool isGenerated;
    private void OnEnable()
    {
        repoolObject = this.transform.parent.GetComponent<RePoolObjectH>();
        if (repoolObject) repoolObject.repoolAfterTime = false;
    }

    private void Start()
    {
        parasiteController = GetComponent<ParasiteController>();
        if(isGenerated) parasiteController.zombieIndex = (int)character.zombieType + 1;
        material = parasiteController.material;
    }

    private void Update()
    {
        if(GameTimeController.isPaused && !isStopingAnimator) StopAnimator(); 
        else if (!GameTimeController.isPaused && isStopingAnimator) PlayAnimator(); 
    }

    public void ApplyDamage(float weaponDamage)
    {
        Debug.Log("ApplyDamage");
        if (character.health <= 0) return;
        if(!parasiteController.foundTarget) parasiteController.foundTarget = true;

        character.health -= weaponDamage;
        damagedRate = Mathf.Clamp(.5f - (character.health * 0.002f), 0f, .5f);
        material.SetFloat("_Dirtiness", damagedRate);

        if (character.health <= 0)
        {
            StartCoroutine(parasiteController.Dissolve(material, 3f, material.GetFloat("_Dissolved"), 1f, true));

            character.health = 100f;
        }
    }

    void PlayAnimator()
    {
        parasiteController.StopCoroutine();
        StartCoroutine( parasiteController.PauseAnimation(true));
        isStopingAnimator = false;
    }

    void StopAnimator()
    {
        parasiteController.StopCoroutine();
        StartCoroutine(parasiteController.PauseAnimation(false));
        isStopingAnimator = true;
    }
}
