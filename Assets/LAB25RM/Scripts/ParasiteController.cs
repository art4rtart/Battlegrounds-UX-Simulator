using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ParasiteController : MonoBehaviour
{
    [HideInInspector] public SkinnedMeshRenderer skinnedMeshRenderer;
    [HideInInspector] public Material material;
    [HideInInspector] public int zombieIndex;
    [HideInInspector] public ParticleSystem biteBloodParticle;
    [HideInInspector] public Transform target;
    public bool foundTarget;

    NavMeshAgent agent;
    Animator animator;
    CapsuleCollider capsuleCollider;

    FirstPersonController player;
    Transform parent;

    Parasite parasite;
    ParasiteItemDropper parasiteItem;

    private void Awake()
    {
        parasite = FindObjectOfType<Parasite>();
        parasiteItem = GetComponent<ParasiteItemDropper>();
        player = FindObjectOfType<FirstPersonController>();
        parent = this.gameObject.transform.parent;
        target = Camera.main.transform;

        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        material = skinnedMeshRenderer.materials[1];
        material.SetFloat("_Dirtiness", 0);
        animator = this.gameObject.transform.parent.GetComponent<Animator>();
        agent = parent.GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;

        biteBloodParticle.Stop();
    }

    private void Start()
    {
        StartCoroutine(Movement());
    }

    private void OnEnable()
    {
        StopCoroutine();
        StartCoroutine(Movement());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) foundTarget = true;
    }

    public void StopCoroutine()
    {
        StopAllCoroutines();
    }

    IEnumerator Movement()
    {
        float updateRate = 0f, timer = 0, timerRefresh = 3f;
        int randomIndex = Random.Range(0, WaypointsManager.Instance.totalWaypoints);

        yield return new WaitForSeconds(Random.Range(.5f, 1f));
        StartCoroutine(Dissolve(material, 0f, material.GetFloat("_Dissolved"), 0f, false));

        animator.SetInteger("ZombieType", zombieIndex);

        while (true) {
            if(!capsuleCollider.enabled) capsuleCollider.enabled = true;
            timer += Time.deltaTime;

            if (timer > timerRefresh) {
                randomIndex = Random.Range(0, WaypointsManager.Instance.totalWaypoints);
                timer = 0f;
            }

            if (!agent.pathPending && !foundTarget) SetAgentDestination(randomIndex);

            if (foundTarget) break;

            yield return new WaitForSeconds(updateRate);
        }

        // run
        animator.SetTrigger("Run");
        agent.speed = 10f;

        while (!player.isDead && agent.enabled && agent.isOnNavMesh && agent.remainingDistance > agent.stoppingDistance)
        {
            agent.SetDestination(target.position);
            if (WeaponController.Instance.weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                agent.stoppingDistance = 1.35f;
            else agent.stoppingDistance = 1.2f;

            yield return null;
        }

        if (agent.enabled && agent.isOnNavMesh && agent.remainingDistance == 0)
        {
            agent.SetDestination(target.position);
            while (agent.enabled && agent.isOnNavMesh)
            {
                agent.SetDestination(target.position);
                if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
                {
                    break;
                }
                yield return null;
            }
        }

        if (!player.isDead && !animator.GetBool("Die"))
        {
            // attack

            agent.SetDestination(this.transform.position);
            agent.transform.LookAt(target);
            animator.SetTrigger("Bite");
            player.target = this.transform;
            player.Die();

            yield return new WaitForSeconds(.8f);

            // get blood
            float value = 0;
            while (value < .45f)
            {
                material.SetFloat("_Dirtiness", value += Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            if(agent.enabled) agent.SetDestination(this.transform.position);
            animator.SetTrigger("Idle");
        }
    }

    void SetAgentDestination(int randomIndex)
    {
        agent.SetDestination(WaypointsManager.Instance.waypoints[randomIndex].position);
    }

    public IEnumerator PauseAnimation(bool play)
    {
        if (play)
        {
            animator.speed = 1f;
        }
        else
        {
            animator.StartRecording(0);
            while (animator.speed > 0)
            {
                animator.speed = Mathf.Clamp(animator.speed -= Time.deltaTime * 2f, 0f, 1f);
                yield return null;
            }
            animator.StopRecording();
        }
    }

    public IEnumerator Dissolve(Material _material, float waitSeconds, float currentRate, float targetRate, bool dead)
    {
        if (dead) {
            agent.SetDestination(this.gameObject.transform.position);
            capsuleCollider.enabled = false;
            animator.SetBool("Die", true);
            agent.speed = 0f;
            agent.enabled = false;
            parasiteItem.DropItem();
        }
        else {
        }

        yield return new WaitForSeconds(waitSeconds);

        float lerpValue = 0;
        float rate = currentRate;

        while (lerpValue < .1f)
        {
            rate = Mathf.Lerp(rate, targetRate, lerpValue);
            lerpValue += Time.deltaTime * .05f;
            _material.SetFloat("_Dissolved", rate);
            yield return null;
        }

        if (dead) InitParasiteData();
    }

    void InitParasiteData()
    {
        agent.enabled = true;
        parent.GetComponent<RePoolObjectH>().repoolAfterTime = true;
        parent.GetComponent<RePoolObjectH>().BeginDestroy();

        parasite.damagedRate = 0f;
        material.SetFloat("_Dirtiness", parasite.damagedRate);
        material.SetFloat("_Dissolved", 1f);
        parasite.character.health = 100f;
        parasiteItem.isDropped = false;

        agent.speed = 1.5f;

        foundTarget = false;
    }
}
