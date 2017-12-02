using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject PauseUI;

    private bool _Paused = false;

    private void Start()
    {
        PauseUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Delete))
        {
            _Paused = !_Paused;
        }

        if (_Paused)
        {
            PauseUI.SetActive(true);
        }

        else if (!_Paused)
        {
            PauseUI.SetActive(false);
        }
    }

    public void Resume()
    {
        _Paused = false;
    }

    public void MainMenu()
    {
        DestroyOnLoad();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void DestroyOnLoad()
    {
        if (SceneManager.GetSceneByName("Multiplayer") == SceneManager.GetActiveScene())
        {
            Destroy(GameObject.Find("NetworkManager"));
        }
    }
}
