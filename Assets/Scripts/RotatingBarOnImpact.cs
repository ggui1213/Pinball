using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{

    public float torqueMultiplier = 100f;
 
    public float dampingDuration = 1f;
  
    public AudioClip collisionSound;

    private Rigidbody2D rb;
    private AudioSource audioSource;

 
    private bool isDamping = false;
    private float dampingTimer = 0f;
    private float initialAngularVelocity = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found");
        }
  
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("pinball"))
        {

            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddScore(150);
            }


            float impactMagnitude = collision.relativeVelocity.magnitude;

            float appliedTorque = impactMagnitude * torqueMultiplier;
            
            ContactPoint2D contact = collision.contacts[0];
            Vector2 offset = contact.point - (Vector2)transform.position;
            float direction = offset.x > 0 ? 1f : -1f;
            appliedTorque *= direction;
            
            rb.AddTorque(appliedTorque, ForceMode2D.Impulse);
            
            if (collisionSound != null)
            {
                audioSource.PlayOneShot(collisionSound);
            }
            
            initialAngularVelocity = rb.angularVelocity;
            dampingTimer = 0f;
            isDamping = true;
        }
    }

    void FixedUpdate()
    {

        if (isDamping)
        {
            dampingTimer += Time.fixedDeltaTime;
            float newAngularVelocity = Mathf.Lerp(initialAngularVelocity, 0f, dampingTimer / dampingDuration);
            rb.angularVelocity = newAngularVelocity;
            
            if (dampingTimer >= dampingDuration)
            {
                rb.angularVelocity = 0f;
                isDamping = false;
            }
        }
    }
}
