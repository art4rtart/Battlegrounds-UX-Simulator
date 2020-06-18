using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level2Manager : MonoBehaviour
{
    [Header("Mission 1")]
    public ItemChecker[] partsSet;

    [Header("Mission 2")]
    public EnemyChecker[] emenySet;

    [Header("WayPoints")]
    public WaypointsManager[] wayPointsManager;

	public Animator successMessage;

    int index;

    private void Start()
    {
        for(int i = 0; i < wayPointsManager.Length; i++) wayPointsManager[i].enabled = true;
        StartCoroutine(Scenario());
    }

    IEnumerator Scenario()
    {
        yield return new WaitForSeconds(9f);
        DoorController.Instance.OpenDoor();
        InstructionController.Instance.animator.SetTrigger("Show");

        yield return new WaitForSeconds(.5f);

        while (index < partsSet.Length)
        {
            while (!partsSet[index].isEmpty) yield return null;
            InstructionController.Instance.UpdateInstructions();

            // customize
            GetCurrentCustomization();
            PartsScrollView.Instance.ShowTargetCustomization();
            yield return new WaitForSeconds(1.25f);
            PartsScrollView.Instance.canvasGroup.alpha = 1f;
            Debug.Log("Customize Updated");

            // set Target customization
            SetTargetCustomization();

            while (!PartsAddController.Instance.CurrentParts(indexes, count)) {
                PartsAddController.Instance.ShowCurrentParts(); yield return null; }
			successMessage.SetTrigger("Show");
			PartsScrollView.Instance.canvasGroup.alpha = 0f;

            DoorController.Instance.OpenDoor();
            InstructionController.Instance.UpdateInstructions();

            wayPointsManager[index].enabled = true;
            for (int i = 0; i < emenySet[index].transform.childCount; i++)
            {
                emenySet[index].transform.GetChild(i).GetComponent<NavMeshAgent>().speed = 1.5f;
                emenySet[index].transform.GetChild(i).GetComponent<Animator>().SetInteger("ZombieType", 1);
            }

            while (!emenySet[index].isEmtpy) yield return null;
            DoorController.Instance.OpenDoor();
            InstructionController.Instance.UpdateInstructions();
            index++;
        }
    }

    public void ShowCanvasGroup()
    {

    }

    [Header("Muzzle/Handle/Magazine/Stock/Scope")]
    public int[] indexes = new int[5] ;
    public void GetCurrentCustomization()
    {
        for (int i = 0; i < 5; i++) indexes[i] = -1;

        indexes[0] = PartsAddController.Instance.muzzleIndex;
        indexes[1] = PartsAddController.Instance.handleIndex;
        indexes[2] = PartsAddController.Instance.magazineIndex;
        indexes[3] = PartsAddController.Instance.stockIndex;
        indexes[4] = PartsAddController.Instance.scopeIndex;

        Debug.Log("current : " + indexes[0] + " " + indexes[1] + " " + indexes[2] + " " + indexes[3] + " " + indexes[4]);
    }

    int count = 0;
    public void SetTargetCustomization()
    {
        // set 1
        // silencer, vertical, qucik mag
        if (count == 0)
        {
            indexes[0] = 0;
            indexes[1] = 0;
            indexes[3] = 0;
        }

        // set 2
        // suppressor, ankle, quick, tactical
        else if (count == 1)
        {
            indexes[0] = 2;
            indexes[1] = 1;
            indexes[2] = 0;
            indexes[3] = 1;
        }

        // set 3
        // compensator, large, hologram
        else if (count == 2)
        {
            indexes[0] = 1;         // muzzle
            indexes[2] = 1;         // mag
            indexes[4] = 1;         // scope
        }

        // 1 - 0 - 1 0
        count++;
    }
}
