using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 5f;

    Vector3 startPosition;

    SpriteRenderer spiritRenderer;

    int direction = 1;

     void Awake()
    {
        startPosition = transform.position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()

    {
        float moveInput = 0f; 
        transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
        float distanceFromStart = Vector3.Distance(transform.position, startPosition);
        if (Mathf.Abs(distanceFromStart) >= moveDistance)
        {
            direction *= -1; // Reverse direction
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
}
