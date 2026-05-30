using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    float moveSpeed = 5f; 
    Rigidbody2D rb;

    float jumpForce = 10f;

    SpriteRenderer spiritRenderer;

    int maxJumps = 2; 
    int jumpRemaining;

    int coinsCollected;
    bool levelCompleted;
    bool gameEnding;
    bool showStartMessage = true;
    GUIStyle coinCounterStyle;
    GUIStyle goalTextStyle;
    GUIStyle startMessageStyle;
    AudioSource musicSource;
    AudioSource soundEffectSource;

    [SerializeField] TMP_Text coinText;
    [SerializeField] TMP_Text goalText;
    [SerializeField] TMP_Text startMessageText;
    [SerializeField] AudioClip coinCollectedSound;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip gameWonSound;
    [SerializeField] AudioClip gameSong;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spiritRenderer = GetComponent<SpriteRenderer>();
        jumpRemaining = maxJumps;
        SetupAudio();

        UpdateCoinText();
        UpdateGoalText();
        UpdateStartMessageText();
        Invoke(nameof(HideStartMessage), 3f);
    }

    void Update()
    {
        if (gameEnding)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        float moveInput = 0f; 
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            moveInput = -1f; 
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput = 1f; 
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRemaining--;

        }
        
        if (moveInput > 0)
        {
            spiritRenderer.flipX = true; 
        }
        else if (moveInput < 0)
        {
            spiritRenderer.flipX = false; 
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            CollectCoin(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpRemaining = maxJumps; 
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameOver();
        }

  
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            CollectCoin(other.gameObject);
        }

        if (other.CompareTag("Fire"))
        {
            GameOver();
        } 

        if (other.CompareTag("Goal") && coinsCollected >= 2)
        {
            CompleteLevel();
        }


    }

    void OnGUI()
    {
        if (coinText == null)
        {
            if (coinCounterStyle == null)
            {
                coinCounterStyle = new GUIStyle(GUI.skin.label);
                coinCounterStyle.fontSize = 28;
                coinCounterStyle.fontStyle = FontStyle.Bold;
                coinCounterStyle.normal.textColor = Color.white;
            }

            GUI.Label(new Rect(16f, 16f, 240f, 40f), GetCoinText(), coinCounterStyle);
        }

        if (goalText == null && levelCompleted)
        {
            if (goalTextStyle == null)
            {
                goalTextStyle = new GUIStyle(GUI.skin.label);
                goalTextStyle.fontSize = 44;
                goalTextStyle.fontStyle = FontStyle.Bold;
                goalTextStyle.alignment = TextAnchor.MiddleCenter;
                goalTextStyle.normal.textColor = Color.yellow;
            }

            GUI.Label(new Rect(0f, 80f, Screen.width, 60f), GetGoalText(), goalTextStyle);
        }

        if (startMessageText == null && showStartMessage)
        {
            if (startMessageStyle == null)
            {
                startMessageStyle = new GUIStyle(GUI.skin.label);
                startMessageStyle.fontSize = 28;
                startMessageStyle.fontStyle = FontStyle.Bold;
                startMessageStyle.alignment = TextAnchor.MiddleCenter;
                startMessageStyle.normal.textColor = Color.white;
            }

            GUI.Label(new Rect(0f, 24f, Screen.width, 50f), GetStartMessageText(), startMessageStyle);
        }
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = GetCoinText();
        }
    }

    string GetCoinText()
    {
        return "Coins: " + coinsCollected;
    }

    void CollectCoin(GameObject coin)
    {
        if (!coin.activeSelf)
        {
            return;
        }

        coinsCollected++;
        PlaySoundEffect(coinCollectedSound);
        UpdateCoinText();
        coin.SetActive(false);
        Destroy(coin);
    }

    void CompleteLevel()
    {
        if (levelCompleted)
        {
            return;
        }

        levelCompleted = true;
        gameEnding = true;
        PlaySoundEffect(gameWonSound);
        UpdateGoalText();
        Debug.Log(GetGoalText());
    }

    void UpdateGoalText()
    {
        if (goalText != null)
        {
            goalText.text = levelCompleted ? GetGoalText() : "";
        }
    }

    string GetGoalText()
    {
        return "Level Completed!";
    }

    void HideStartMessage()
    {
        showStartMessage = false;
        UpdateStartMessageText();
    }

    void UpdateStartMessageText()
    {
        if (startMessageText != null)
        {
            startMessageText.text = showStartMessage ? GetStartMessageText() : "";
        }
    }

    string GetStartMessageText()
    {
        return "Collect enough coins to clear the level";
    }

    void SetupAudio()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.spatialBlend = 0f;
        musicSource.volume = 0.5f;

        soundEffectSource = gameObject.AddComponent<AudioSource>();
        soundEffectSource.playOnAwake = false;
        soundEffectSource.loop = false;
        soundEffectSource.spatialBlend = 0f;
        soundEffectSource.volume = 1f;

        if (gameSong != null)
        {
            musicSource.clip = gameSong;
            musicSource.Play();
        }
    }

    void PlaySoundEffect(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            soundEffectSource.PlayOneShot(audioClip);
        }
    }

    void GameOver()
    {
        if (gameEnding)
        {
            return;
        }

        gameEnding = true;
        PlaySoundEffect(gameOverSound);
        Invoke(nameof(RestartLevel), GetRestartDelay());
    }

    float GetRestartDelay()
    {
        if (gameOverSound == null)
        {
            return 0.5f;
        }

        return Mathf.Min(gameOverSound.length, 2f);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
