using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Text scoreTxt;
    public GameObject playButton;
    public GameObject gameOver;
    public GameObject getReady;
    private int score;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        gameOver.GetComponent<Image>().enabled = false;

        Pause();
    }

    public void Play()
    {
        score = 0;
        scoreTxt.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);
        getReady.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();
        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        playButton.SetActive(true);
        gameOver.GetComponent<Image>().enabled = true;

        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        scoreTxt.text = score.ToString();
    }
}
