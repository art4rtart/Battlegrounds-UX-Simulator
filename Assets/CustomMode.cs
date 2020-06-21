using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		WeaponController.Instance.weaponAnimator.SetBool("Holster", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
