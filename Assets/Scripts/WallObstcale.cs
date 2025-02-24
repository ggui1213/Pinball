using UnityEngine;

public class WallScoring : MonoBehaviour
{
    public AudioClip wallSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("pinball"))
        {
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddScore(50);
            }
            
            if (wallSound != null)
            {
                audioSource.PlayOneShot(wallSound);
            }
        }
    }
}