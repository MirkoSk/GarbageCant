using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] List<Transform> _spawnPoints = new List<Transform>();

    PlayerController _player;
    int _currentFloor = 0;

    public int SpawnPointsCount { get => _spawnPoints.Count; }



    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (PlayerController.PlayerInputs.Standard.Spawn1.triggered) SpawnPlayer(0);
        else if (PlayerController.PlayerInputs.Standard.Spawn2.triggered) SpawnPlayer(1);
        else if (PlayerController.PlayerInputs.Standard.Spawn3.triggered) SpawnPlayer(2);
        else if (PlayerController.PlayerInputs.Standard.Spawn4.triggered) SpawnPlayer(3);
        else if (PlayerController.PlayerInputs.Standard.Spawn5.triggered) SpawnPlayer(4);
        else if (PlayerController.PlayerInputs.Standard.Spawn6.triggered) SpawnPlayer(5);
        else if (PlayerController.PlayerInputs.Standard.Spawn7.triggered) SpawnPlayer(6);
        else if (PlayerController.PlayerInputs.Standard.Spawn8.triggered) SpawnPlayer(7);
        else if (PlayerController.PlayerInputs.Standard.Respawn.triggered) RespawnAtCurrentFloor();
    }



    public void SpawnPlayer(int number)
    {
        if (number < _spawnPoints.Count) _player.Respawn(_spawnPoints[number]);
        else Debug.LogError($"Couldn't find spawn point with number {number}.", gameObject);
    }

    public void RespawnAtCurrentFloor()
    {
        SpawnPlayer(_currentFloor);
    }

    public void ChangeSpawnFloor(bool increment)
    {
        if (increment) _currentFloor = _currentFloor++;
        else _currentFloor = _currentFloor--;

        _currentFloor = Mathf.Clamp(_currentFloor, 0, 4);
    }
}
