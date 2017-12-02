using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;


    private void Start()
    {
        MainMenu();
    }

    public void MainMenu()
    {
        for (int i = 0; i <= 4; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(true);
        }
        canvas.transform.GetChild(5).gameObject.SetActive(false);
        canvas.transform.GetChild(6).gameObject.SetActive(false);
        canvas.transform.GetChild(7).gameObject.SetActive(false);
        canvas.transform.GetChild(8).gameObject.SetActive(false);
        canvas.transform.GetChild(9).gameObject.SetActive(false);
    }


    public void StartGame()
    {

        canvas.transform.GetChild(7).gameObject.SetActive(false);
        canvas.transform.GetChild(8).gameObject.SetActive(false);
        for (int i = 1; i <= 4; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }
        canvas.transform.GetChild(5).gameObject.SetActive(true);
        canvas.transform.GetChild(6).gameObject.SetActive(true);
        canvas.transform.GetChild(9).gameObject.SetActive(true);
    }

    public void Authors()
    {
        canvas.transform.GetChild(7).gameObject.SetActive(true);
        canvas.transform.GetChild(8).gameObject.SetActive(true);
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
        yield return new WaitForSeconds(15.0F);
        canvas.transform.GetChild(7).gameObject.SetActive(false);
        canvas.transform.GetChild(8).gameObject.SetActive(false);

    }
}
