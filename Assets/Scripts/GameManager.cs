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
    [SerializeField] AudioClip selectSound;

    [Header("Start Screen")]
    [SerializeField] GameObject startScreen;
    [SerializeField] TMP_Text startHighScoreText;
    [SerializeField] AudioClip startSound;

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

    [HideInInspector] public static GameState state = GameState.WAITING;

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
            case GameState.WAITING:
                startScreen.SetActive(true);
                scoreText.gameObject.SetActive(false);
                startHighScoreText.text = "HI-SCORE: " + highScore.ToString();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    state = GameState.PLAYING;
                    FindObjectOfType<PlatformManager>().SetMoving(true);
                    scoreText.gameObject.SetActive(true);
                    startScreen.SetActive(false);
                    FindObjectOfType<AudioPlayer>().PlayAudio(startSound);
                }
                break;
            case GameState.COUNTING_DOWN:

            break;
            case GameState.GAME_OVER:
                GameOver();
            break;
        }

        // restart
        //if (Input.GetKeyDown(KeyCode.R)) 
        //{ 
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //    state = GameState.WAITING;
        //    score = 0;
        //}
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
            FindObjectOfType<AudioPlayer>().PlayAudio(selectSound);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gameOverSelect == 1) { gameOverSelect = 0; }
            else { gameOverSelect = 1; }
            FindObjectOfType<AudioPlayer>().PlayAudio(selectSound);
        }

        // TODO no magic numbers smh
        if (gameOverSelect == 0) { selectArrow.rectTransform.anchoredPosition = new Vector3(-254, -170.5f); }
        else { selectArrow.rectTransform.anchoredPosition = new Vector3(106, -170.5f); }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (gameOverSelect == 0) 
            { 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                score = 0;
                state = GameState.WAITING;
            }
            else 
            { 
                Application.Quit();
            }
        }
    }
}
