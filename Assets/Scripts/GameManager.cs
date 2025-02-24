using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public int lives = 3;
    public Transform spawnPoint;
    public float deathYThreshold = -10f;
    
    public TextMeshProUGUI livesText;

    public TextMeshProUGUI gameOverText;

    [SerializeField] private GameObject pinball;
    private Rigidbody2D pinballRb;
    private bool gameOver = false;

    void Awake()
    {
    
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
            pinballRb = pinball.GetComponent<Rigidbody2D>();

        UpdateLivesUI();

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameOver && pinball != null && pinball.transform.position.y < deathYThreshold)
        {
            lives--;
            Debug.Log("Life lost, remaining lives: " + lives);
            ResetPinball();
            UpdateLivesUI();

            if (lives <= 0)
            {
                GameOver();
            }
        }
        
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }
    
    void ResetPinball()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint is not set!");
            return;
        }

        // Debug log for position checking
        Debug.Log("Resetting pinball from position: " + pinball.transform.position);
        
        pinball.transform.position = spawnPoint.position;
        pinball.transform.rotation = spawnPoint.rotation;

        if (pinballRb != null)
        {
            pinballRb.linearVelocity = Vector2.zero;
            pinballRb.angularVelocity = 0f;
        }

        Debug.Log("Pinball reset to position: " + pinball.transform.position);
    }


    ///UI
    void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
        else
        {
            Debug.LogWarning("livesText is not assigned in the Inspector!");
        }
    }

    //Gameover
    void GameOver()
    {
        gameOver = true;
        Destroy(pinball);
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "Game Over!\nPress R to Restart";
        }
        Debug.Log("Game Over");
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
