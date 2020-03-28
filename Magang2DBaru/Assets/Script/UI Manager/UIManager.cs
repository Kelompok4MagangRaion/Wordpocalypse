using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static bool isPaused = false;
    public static bool isDeath = false;
    public static bool isFinish = false;
    public PlayerInventory playerInventory;
    public FloatValue durabilitySword;
    public FloatValue bullets;

    public GameObject pausePanel;

    void Update()
    {
        if (pausePanel != null && !isDeath && !isFinish)
        {
            Paused();
        }
    }

    public void Paused()
    {
        if(!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Resume()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Play(string nama)
    {
        if (playerInventory.myInventory.Count > 0)
        {
            for (int i = 0; i < playerInventory.myInventory.Count; i++)
            {
                playerInventory.myInventory[i].numberHeld = 0;
            }
        }
        durabilitySword.RuntimeValue = 0;
        bullets.RuntimeValue = 0;
        isDeath = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(nama);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        Play("SampleScene");
    }
}
