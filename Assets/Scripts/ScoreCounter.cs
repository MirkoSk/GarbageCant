using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTextmesh;



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
        scoreTextmesh.text = pointsTotal.ToString();
    }
}
