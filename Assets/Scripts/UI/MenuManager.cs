using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject Buttons;
    [SerializeField]
    GameObject Authors;


    private void Start()
    {
        MainMenu();
    }

    public void MainMenu()
    {
        for (int i = 0; i < 4; i++)
        {
            Buttons.transform.GetChild(i).gameObject.SetActive(true);
        }

        Buttons.transform.GetChild(4).gameObject.SetActive(false);
        Buttons.transform.GetChild(5).gameObject.SetActive(false);
        Buttons.transform.GetChild(6).gameObject.SetActive(false);
    }


    public void StartGame()
    {
        for (int i = 0; i < 4; i++)
        {
            Buttons.transform.GetChild(i).gameObject.SetActive(false);
        }
        Buttons.transform.GetChild(4).gameObject.SetActive(true);
        Buttons.transform.GetChild(5).gameObject.SetActive(true);
        Buttons.transform.GetChild(6).gameObject.SetActive(true);
    }

    public void SetAuthors()
    {
        Authors.SetActive(true);
        StartCoroutine(Enumerator());
    }

    public void QuitGame()
    {
        Debug.Log("Quit the game!");
        Application.Quit();
    }

    public void StartGameSingleplayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void StartGameMultiplayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    IEnumerator Enumerator()
    {
        yield return new WaitForSeconds(10.0F);
        Authors.SetActive(false);

    }
}
