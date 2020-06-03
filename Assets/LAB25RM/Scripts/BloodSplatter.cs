using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    public GameObject projector;

    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!other.gameObject.CompareTag("Enviroment")) return;
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        int i = 0;
        while (i < (int)(numCollisionEvents) / 5)
        {
            Vector3 mul = (Vector3.up + Vector3.forward) * 2f;
            Vector3 spawnPos = collisionEvents[i].intersection;
            Vector3 direction = other.transform.position - spawnPos;
                        spawnPos = new Vector3(spawnPos.x, other.gameObject.transform.position.y, spawnPos.z);
            RaycastHit hit;
            Physics.Raycast(spawnPos, direction.normalized, out hit, 1000f);
            projector.transform.rotation *= Quaternion.FromToRotation(projector.transform.up, hit.normal * -1f);
            projector.transform.localEulerAngles = new Vector3(90, projector.transform.localEulerAngles.y, projector.transform.localEulerAngles.z);

            spawnPos = new Vector3(spawnPos.x, other.gameObject.transform.position.y, spawnPos.z);

            GameObject splatterObj = ObjectPoolerH.instance.GetPooledObject("Blood Splatter");
            splatterObj.transform.position = spawnPos;
            splatterObj.transform.rotation = projector.transform.rotation;
            splatterObj.SetActive(true);
            i++;
        }
    }
}
