using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents
{
    public delegate void PickUpCollectHandler(int pointsGained, int pointsTotal);
    public static event PickUpCollectHandler OnPickupCollected;
    public static void PickupCollected(int pointsGained, int pointsTotal)
    {
        Debug.Log($"<color=#00FF00>[GameEvent]</color> OnPickupCollected - PointsGained: {pointsGained}, PointsTotal: {pointsTotal}");
        OnPickupCollected?.Invoke(pointsGained, pointsTotal);
    }
}
