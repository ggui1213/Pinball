using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public float launchForce = 600f;
    public AudioClip slingshotSound;
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
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddScore(50);
            }
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (ballRb != null && collision.contacts.Length > 0)
            {
                Vector2 launchDirection = collision.contacts[0].normal;
                print(launchDirection);

                ballRb.AddForce(-launchDirection * launchForce, ForceMode2D.Impulse);
            }

            if (slingshotSound != null)
            {
                audioSource.PlayOneShot(slingshotSound);
            }
        }
    }
}
