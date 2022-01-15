using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;

    public TextMeshProUGUI HighScoreBox;
    public Text HighScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;
    public int score;
    public int highScore;
    public string highScorePlayer;

    public string player;



    // Start is called before the first frame update
    void Start()
    {
        player = MenuManager.Instance.currentPlayer;
        Debug.Log("the player right now be " + player);

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }




    }

    private void Update()
    {
        // CurrentScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoadHighScore();

                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 16.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score: {m_Points}";
        if (m_Points > highScore)
        {

            highScore = m_Points;
            highScorePlayer = player;
            HighScoreText.text = "High Score: " + highScorePlayer + ": " + highScore.ToString();
            SaveHighScore();

            Debug.Log("HighScore set to: " + highScore);

            // highScorePlayer.text = currentPlayer;


        }
    }

    [System.Serializable]
    class SaveData
    {
        public string currentPlayer;
        public int score = 0;
        public int highScore;
        public string highScorePlayer;

    }
    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScore = highScore;
        data.highScorePlayer = highScorePlayer;


        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("SAVED High Score");
    }
    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.highScore;
            highScorePlayer = data.highScorePlayer;
            HighScoreText.text = "High Score: " + highScorePlayer + ": " + highScore.ToString();

            Debug.Log("LOADED High Score");


            return;

        }

        if (highScore < 1)
        {
            highScore = 0;
            highScorePlayer = player;
            HighScoreText.text = "High Score: ";
            Debug.Log("HighScore set to: " + highScore);
        }
        HighScoreText.text = "High Score: " + highScorePlayer + ": " + highScore.ToString();


    }

    public void BackOutToMainMenu()
    {
        SceneManager.LoadScene(0);

    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
