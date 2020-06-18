using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTurret : MonoBehaviour
{
    public float health;

    public float radius;
    public LayerMask layerMask;

    public bool foundTarget;

    Animator animator;
    public bool dead = false;
    void Start()
    {
        animator = transform.GetComponent<Animator>();
    }

    public float damping = 2f;
    public float damping2 = 2f;
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radius, layerMask);

        if (hitColliders.Length == 0) return;

        Vector3 lookPos = hitColliders[0].transform.position - transform.position - Vector3.one * damping2;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

        if (dead && !animator.applyRootMotion) { StopAllCoroutines(); animator.SetBool("Dead", true); animator.applyRootMotion = true; }

        if (hitColliders.Length == 0)
        {
            foundTarget = false;

            animator.ResetTrigger("FoundTarget");
            animator.SetBool("Shoot", false);

            StopAllCoroutines();
            return;
        }

        if (foundTarget) return;

        StartCoroutine(Shoot(hitColliders[0].transform));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    [Range(0, 10)]
    public float shootPercentage = 5f;
    bool shoot;

    IEnumerator Shoot(Transform target)
    {
        Debug.Log("Found Human");
        foundTarget = true;
        shoot = false;
        animator.SetTrigger("FoundTarget");

        while (true)
        {
            float rate = Random.Range(0, 10f);
            if (rate > shootPercentage)
            {
                Debug.Log("Shoot" + rate);
                animator.SetBool("Shoot", true);
            }

            else
            {
                Debug.Log("Idle" + rate);
                animator.SetBool("Shoot", false);
                yield return new WaitForSeconds(Random.Range(0f, 2f));
            }

            if (!foundTarget) break;
            yield return new WaitForSeconds(.5f);
        }
    }

    public void ApplyDamage(float _damageRate)
    {
        if (health <= 0) { Die(); return; }
        health -= _damageRate;
    }

    void Die()
    {
        dead = true;
        Debug.Log("I am Dead");
    }
}
