using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TMP_Text scoreText;

    // TODO separate class, I don't want all these references in GameManager
    [Header("Game Over Screen")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] TMP_Text endScoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] Image selectArrow;

    static int score = 0;
    static int highScore = 0;

    int gameOverSelect = 0; // select pos

    public enum GameState
    {
        WAITING, 
        COUNTING_DOWN,
        PLAYING,
        PAUSED,
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
                GameOver();
            break;
        }

        if (Input.GetKeyDown(KeyCode.R)) 
        { 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            score = 0;
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

    void GameOver()
    {
        gameOverScreen.SetActive(true);
        endScoreText.text = "SCORE: " + score.ToString();
        highScoreText.text = "HI-SCORE: " + highScore.ToString();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gameOverSelect == 0) { gameOverSelect = 1; }
            else { gameOverSelect = 0; }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gameOverSelect == 1) { gameOverSelect = 0; }
            else { gameOverSelect = 1; }
        }

        // TODO no magic numbers smh
        if (gameOverSelect == 0) { selectArrow.rectTransform.anchoredPosition = new Vector3(-325, -220); }
        else { selectArrow.rectTransform.anchoredPosition = new Vector3(89, -220); }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (gameOverSelect == 0) 
            { 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                state = GameState.PLAYING;
            }
            else 
            { 
                Application.Quit();
            }
        }
    }

    public void PlayAudio(AudioClip sound)
    {

    }
}
