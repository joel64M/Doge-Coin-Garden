using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public bool canMove = true;

    [SerializeField] int totalCoins = 0;
    [SerializeField] int currentCoins = 0;


    [Header("UI")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] GameObject gameplayPanel;
    [SerializeField] GameObject gameCompletePanel;
    string levelString;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("LEVELS", 0) != SceneManager.GetActiveScene().buildIndex)
        {
            if (Application.CanStreamedLevelBeLoaded(PlayerPrefs.GetInt("LEVELS", 0)))
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("LEVELS", 0));
            }
            else
            {
                SceneManager.LoadScene("Final");
            }
        }
        instance = this;
    }

    // Start is called before the first frame update

    void Start()
    {
       
        levelString = "level " + (PlayerPrefs.GetInt("LEVELS", 0) + 1).ToString();
        levelText.text = levelString;
        TinySauce.OnGameStarted(levelNumber: levelString);
        Debug.Log("Started "+levelString);
        Invoke("DisplayCoinText", 0.2f);
    }

    void DisplayCoinText()
    {
        coinText.text = currentCoins + "/" + totalCoins;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeCoin()
    {
        totalCoins++;
    }

    public void CoinCollected()
    {
        currentCoins++;
        coinText.text = currentCoins + "/" + totalCoins;
        if (currentCoins >= totalCoins)
        {
            canMove = false;
            gameplayPanel.SetActive(false);
            gameCompletePanel.SetActive(true);
            PlayerPrefs.SetInt("LEVELS", SceneManager.GetActiveScene().buildIndex + 1);
            //Debug.Log("level complete");
            TinySauce.OnGameFinished(true, currentCoins, levelNumber: levelString);
            Debug.Log("Complete " + levelString);

        }
    }

    public void _NextLevel()
    {
        if (Application.CanStreamedLevelBeLoaded(SceneManager.GetActiveScene().buildIndex + 1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            PlayerPrefs.SetInt("LEVELS", 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene(0);
        }
    }
}
