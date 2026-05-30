using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float moveSpeed = 5f; 
    Rigidbody2D rb;

    float jumpForce = 10f;

    SpriteRenderer spiritRenderer;

    int maxJumps = 2; 
    int jumpRemaining;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spiritRenderer = GetComponent<SpriteRenderer>();
        jumpRemaining = maxJumps;

        
    }

    // Update is called once per frame
    void Update()
    {
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpRemaining = maxJumps; 
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            RestartLevel();
        }

  
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fire"))
        {
            RestartLevel();
        } 

        if (other.CompareTag("Goal"))
        {
            Debug.Log("Level Completed!");
        }

        // if (other.CompareTag("Enemy"))
        // {
        //     RestartLevel();
        // }
    }



    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
