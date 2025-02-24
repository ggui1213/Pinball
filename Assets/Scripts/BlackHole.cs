using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour
{
    public float holdDuration = 2f;
 
    public float releaseMultiplier = 1f;
    
    public float pullStrength = 5f;

    public AudioClip blackHoleSound;

    public float cooldownDuration = 2f;

    private AudioSource audioSource;

    private Rigidbody2D capturedBall;
    private Vector2 cachedVelocity;
    private float cachedGravityScale;

    private bool isCoolingDown = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (isCoolingDown)
            return;

        if(other.CompareTag("pinball"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if(rb != null && capturedBall == null)
            {
                capturedBall = rb;

                cachedVelocity = rb.linearVelocity;
                cachedGravityScale = rb.gravityScale;

                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0f;
                
                if(blackHoleSound != null)
                    audioSource.PlayOneShot(blackHoleSound);
                
                StartCoroutine(HoldAndRelease(rb));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("pinball"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if(rb != null && rb == capturedBall)
            {
                rb.gravityScale = cachedGravityScale;
                capturedBall = null;
            }
        }
    }

    IEnumerator HoldAndRelease(Rigidbody2D rb)
    {
        float timer = 0f;
        while(timer < holdDuration)
        {
            if(rb == null)
                yield break;
            
            Vector2 pullDir = (transform.position - rb.transform.position).normalized;
            rb.AddForce(pullDir * pullStrength);
            
            timer += Time.deltaTime;
            yield return null;
        }
        
        if(rb != null)
        {
            rb.gravityScale = cachedGravityScale;
            Vector2 releaseVelocity = cachedVelocity * releaseMultiplier;
  
            if(releaseVelocity == Vector2.zero)
                releaseVelocity = (rb.transform.position - transform.position).normalized * releaseMultiplier;
            
            rb.linearVelocity = releaseVelocity;
        }
        capturedBall = null;
        
        if(ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(200);
        }

        // 启动冷却
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCoolingDown = false;
    }
}
