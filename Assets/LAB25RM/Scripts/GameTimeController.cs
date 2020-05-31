using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeController : MonoBehaviour
{
    public static bool isPaused = false;
    public static float deltaTime {  get { return isPaused ? 0 : Time.deltaTime; } }
}
