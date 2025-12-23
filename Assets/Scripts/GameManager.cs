using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winPanel, losePanel;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void WinGame()
    {
        if (gameEnded)
            return;
        gameEnded = true;
        Time.timeScale = 0f;
        winPanel.SetActive(true);
    }

    public void LoseGame()
    {
        if (gameEnded)
            return;
        gameEnded = true;
        Time.timeScale = 0f;
        losePanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
