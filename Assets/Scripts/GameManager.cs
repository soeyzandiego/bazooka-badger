using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TMP_Text scoreText;

    [Header("Game Over Screen")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] TMP_Text endScoreText;
    [SerializeField] TMP_Text highScoreText;

    static int score = 0;
    static int highScore = 0;

    public enum GameState
    {
        WAITING, 
        COUNTING_DOWN,
        PLAYING,
        GAME_OVER
    };

    static GameState state = GameState.WAITING;

    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();

        switch (state)
        {
            case GameState.GAME_OVER:
                gameOverScreen.SetActive(true);
                endScoreText.text = score.ToString();
                highScoreText.text = highScore.ToString();
            break;
        }
    }

    public static void ModifyScore(int _value)
    {
        score += _value;
    }

    public static void ChangeState(GameState newState)
    {
        state = newState;

        if (score > highScore) { highScore = score; }
    }
}
