using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject ball;

    public Text scoreTextLeft;
    public Text scoreTextRight;

    public Starter starter;

    private bool started = false;

    private int scoreLeft = 0;
    private int scoreRight = 0;

    private BallController ballController;

    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        ballController = ball.GetComponent<BallController>();
        startingPosition = ball.transform.position;
    }

    public void StartGame()
    {
        ballController.Go();
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
            return;

        if (Input.GetKey(KeyCode.Space))
        {
            {
                started = true;
                starter.StartCountdown();
            }
        }
    }

    public void ScoreGoalLeft()
    {
        scoreRight += 1;
        UpdateUI();
        ResetBall();
    }

    public void ScoreGoalRight()
    {
        scoreLeft += 1;
        UpdateUI();
        ResetBall();
    }

    private void UpdateUI()
    {
        scoreTextLeft.text = scoreLeft.ToString();
        scoreTextRight.text = scoreRight.ToString();
    }

    private void ResetBall()
    {
        ballController.Stop();
        ball.transform.position = startingPosition;
        starter.StartCountdown();
    }
}
