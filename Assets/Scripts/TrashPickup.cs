using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPickup : MonoBehaviour
{
    [SerializeField] int points = 1;

    private void OnTriggerEnter(Collider other)
    {
        TrashCollector player = other.GetComponentInParent<TrashCollector>();
        if (player != null)
        {
            player.CollectTrash(points);
            Destroy(gameObject);
        }
    }
}
