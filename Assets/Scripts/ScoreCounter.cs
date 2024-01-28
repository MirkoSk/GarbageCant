using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTextmesh;

    int totalPickupCount;



    private void Start()
    {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("TrashPickup");
        totalPickupCount = pickups.Length;
        scoreTextmesh.text = "0 / " + totalPickupCount.ToString();
    }

    private void OnEnable()
    {
        GameEvents.OnPickupCollected += UpdateCounter;
    }

    private void OnDisable()
    {
        GameEvents.OnPickupCollected -= UpdateCounter;
    }



    void UpdateCounter(int pointsGained, int pointsTotal)
    {
        scoreTextmesh.text = pointsTotal.ToString() + " / " + totalPickupCount.ToString();
    }
}
