using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField]
    GameObject _player, _camera;

    public PlayerInputs PlayerInputs{get; private set;}

    private void Awake()
    {
        PlayerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        PlayerInputs.Enable();
    }
    private void OnDisable()
    {
        PlayerInputs.Disable();
    }

    private void Update()
    {
        if (PlayerInputs.Menu.Accept.ReadValue<float>() == 1) {
            GetComponentInChildren<Canvas>().gameObject.SetActive(false);
            _player.SetActive(true);
            _camera.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
