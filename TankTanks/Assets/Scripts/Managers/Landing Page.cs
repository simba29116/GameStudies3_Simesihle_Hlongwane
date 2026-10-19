using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LandingPage : MonoBehaviour
{

    // adding pause menu code to save time 
    public GameObject Pausemenu;
    public static bool isPaused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Pausemenu.SetActive(false);
    
    }

    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }

        }*/
    }


    public void PlayGame()
    {

        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {

        Application.Quit();
    }

    public void PauseGame()
    {
        Pausemenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Pausemenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void loadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }


    public void PlayGame1()
    {

        SceneManager.LoadScene("SampleScene");
    }
}