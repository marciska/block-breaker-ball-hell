using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] float minX = 1f, maxX = 15f;
    [SerializeField] float screenWidthInUnits = 16f;

    // cached references
    Ball ball;
    GameSession gameSession;


    private void Start()
    {
        ball = FindObjectOfType<Ball>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 paddlePos = new Vector2(transform.position.x, transform.position.y);
        paddlePos.x = Mathf.Clamp(GetXPos(), minX, maxX);
        transform.position = paddlePos;
    }

    private float GetXPos()
    {
        if (gameSession.IsAutoPlayEnabled() && ball.HasStarted()) // Using Autopilot
            return ball.transform.position.x;
        else // Shift paddle to mouse position
            return Input.mousePosition.x / Screen.width * screenWidthInUnits;
    }
}
