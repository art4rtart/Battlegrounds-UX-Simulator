using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsController : MonoBehaviour
{
    PartsType partType;

    public void WeaponCustomize(int _partType, string _itemName)
    {
        partType = (PartsType)_partType;

        switch (partType)
        {
            case PartsType.Muzzle:
                switch(_itemName)
                {
                    case "Suppressor (AR)":
                        Debug.Log("Suppressor Equipped");
                        break;
                    case "Silencer (AR)":
                        Debug.Log("Silencer Equipped");
                        break;
                    case "Compensator (AR)":
                        Debug.Log("Compensator Equipped");
                        break;
                }
                break;
            case PartsType.Handle:
                break;
            case PartsType.Magazine:
                break;
            case PartsType.Reverse:
                break;
            case PartsType.Scope:
                break;
        }
    }
}
