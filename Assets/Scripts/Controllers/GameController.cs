using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    [SerializeField]
    GameObject introPanel, controlsPanel;

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
        PlayerController.PlayerInputs.Disable();

        PlayerInputs.Enable();
        introPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }
    private void OnDisable()
    {
        PlayerInputs.Disable();
    }

    private void Update()
    {
        if (PlayerInputs.Menu.Accept.triggered)
        {
            if (introPanel.activeInHierarchy)
            {
                introPanel.SetActive(false);
                controlsPanel.SetActive(true);
            }
            else if (!introPanel.activeInHierarchy)
            {
                GetComponentInChildren<Canvas>().gameObject.SetActive(false);
                PlayerController.PlayerInputs.Enable();
                gameObject.SetActive(false);
            }
        }
    }
}
