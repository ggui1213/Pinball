using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float bumpForce = 600f;
    public AudioClip bumperSound;
    
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("pinball"))
        {
            
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (ballRb != null && collision.contacts.Length > 0)
            {
                Vector2 bounceDirection = collision.contacts[0].normal;

                ballRb.AddForce(-bounceDirection * bumpForce, ForceMode2D.Impulse);
            }


            if (bumperSound != null)
            {
                audioSource.PlayOneShot(bumperSound);
            }
            
            if(ScoreManager.instance != null)
            {
                ScoreManager.instance.AddScore(150);
            }
            
        }
    }
}