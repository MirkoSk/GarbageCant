using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnFloorChanger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            if (player.transform.position.y > transform.position.y) SpawnManager.Instance.ChangeSpawnFloor(true);
            else SpawnManager.Instance.ChangeSpawnFloor(false);
        }
    }
}
