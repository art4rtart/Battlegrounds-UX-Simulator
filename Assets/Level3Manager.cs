using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level3Manager : MonoBehaviour
{
    [Header("Destination")]
    public Destination[] destination;

    int index;

    public ItemChecker[] itemCheker;
    public EnemyChecker[] enemySet;

	public Animator successMessage;
	public Animator dangerMessage;

	CustomState customState;
	private void Start()
    {
		customState = FindObjectOfType<CustomState>();

		StartCoroutine(Scenario());
    }

	bool isMessageShowed;

	private void Update()
	{
		if (!PartsAddController.Instance.CurrentPartsLevel3(indexes, count)) return;

		if (!isMessageShowed) { isMessageShowed = true; successMessage.SetTrigger("Show"); customState.UpdateCustomState(true); }
	}

	IEnumerator Scenario()
    {
        yield return new WaitForSeconds(10f);
        InstructionController.Instance.animator.SetTrigger("Show");
		InstructionController.Instance.audioSource.Play();

		while (index < destination.Length)
        {
            destination[index].OpenDestination();
            while (!destination[index].arrived) yield return null;
            while (!itemCheker[index].isEmpty) yield return null;

			// 8x Scope를 장착 후 몰려오는 적을 처치해 주세요.
			customState.UpdateCustomState(false);
			InstructionController.Instance.UpdateInstructions();
			MissionRate.Instance.UpdateMissionClearRate();

			// activate enemys
			successMessage.gameObject.SetActive(true);
			SetTargetCustomization();
			PartsScrollView.Instance.ShowTargetCustomization();
			isMessageShowed = false;
			yield return new WaitForSeconds(.75f);
            PartsScrollView.Instance.canvasGroup.alpha = 1f;

            yield return new WaitForSeconds(8f);
			successMessage.gameObject.SetActive(false);
			dangerMessage.SetTrigger("Show");
			enemySet[index].gameObject.SetActive(true);

            for (int i = 0; i < enemySet[index].transform.childCount; i++)
            {
                enemySet[index].transform.GetChild(i).GetComponent<NavMeshAgent>().speed = 1.5f;
                enemySet[index].transform.GetChild(i).GetComponent<Animator>().SetInteger("ZombieType", 1);
                enemySet[index].transform.GetChild(i).GetChild(1).GetComponent<ParasiteController>().foundTarget = true;
            }

            //while (!PartsAddController.Instance.CurrentPartsLevel3(indexes, count)) yield return null;

            float damageRate = WeaponController.Instance.equippedGun.damageRate;
            while (!enemySet[index].isEmtpy)
            {
				TimeMeasureController.Instance.customTime[index] += Time.deltaTime;

				if (!PartsAddController.Instance.CurrentPartsLevel3(indexes, count))
				{
					WeaponController.Instance.equippedGun.damageRate = 0f;
				}
				else
				{
					WeaponController.Instance.equippedGun.damageRate = damageRate;
				}
                yield return null;
            }

			//Temp
			TimeMeasureController.Instance.customTime[index] += 5f;

			PartsScrollView.Instance.canvasGroup.alpha = 0f;
            InstructionController.Instance.UpdateInstructions();
			MissionRate.Instance.UpdateMissionClearRate();
			customState.HideCustomState();
			index++;

			yield return new WaitForSeconds(1f);
        }
    }

    public void ShowCanvasGroup()
    {

    }

    [Header("Muzzle/Handle/Magazine/Stock/Scope")]
    public int[] indexes = new int[5];
    public void GetCurrentCustomization()
    {
        for (int i = 0; i < 5; i++) indexes[i] = -1;

        indexes[0] = PartsAddController.Instance.muzzleIndex;
        indexes[1] = PartsAddController.Instance.handleIndex;
        indexes[2] = PartsAddController.Instance.magazineIndex;
        indexes[3] = PartsAddController.Instance.stockIndex;
        indexes[4] = PartsAddController.Instance.scopeImageIndex;

        Debug.Log("current : " + indexes[0] + " " + indexes[1] + " " + indexes[2] + " " + indexes[3] + " " + indexes[4]);
    }

    int count = 0;
    public void SetTargetCustomization()
    {
        // set 1
        // 8x scope, light stock
        if (count == 0)
        {
            indexes[3] = 0;
            indexes[4] = 2;
        }

        // set 2
        // 4x scope, vertical, large
        else if (count == 1)
        {
            indexes[1] = 0;
            indexes[2] = 1;
            indexes[4] = 1;
        }

        // set 3
        // 2x scope, compensator
        else if (count == 2)
        {
            indexes[0] = 1;         // muzzle
            indexes[4] = 0;         // scope
        }

        // 1 - 0 - 1 0
        count++;
    }
}
