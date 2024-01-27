using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollector : MonoBehaviour
{
    int currentPoints = 0;



    public void CollectTrash(int points)
    {
        currentPoints += points;
        GameEvents.PickupCollected(points, currentPoints);
    }
}
