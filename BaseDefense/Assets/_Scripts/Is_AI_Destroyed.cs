using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Is_AI_Destroyed : MonoBehaviour
{
    private void OnDestroy()
    {
        SpawnManager_B_Adam.enemiesAlive -= 1;
    }
}
