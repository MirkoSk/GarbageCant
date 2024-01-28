using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    [SerializeField]
    GameObject _player, _camera;

    public PlayerInputs PlayerInputs{get; private set;}

    private void Awake()
    {
        PlayerInputs = new PlayerInputs();

        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
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
