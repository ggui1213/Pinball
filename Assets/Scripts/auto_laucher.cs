using UnityEngine;
using System.Collections;

public class AutoLauncher : MonoBehaviour
{
    [SerializeField]
    private float force = 600f;        
    
    private Rigidbody2D ball;          
    private bool isLaunching = false;  
    
    [SerializeField]
    private float restYScale = 1f; 
    [SerializeField]
    private float pressedYScale = 0.5f;  
    [SerializeField]
    private float scalingSpeed = 5f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("pinball"))
        {
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddScore(200);
            }
            ball = collision.gameObject.GetComponent<Rigidbody2D>();
            
            if (!isLaunching)
            {
                StartCoroutine(AutoLaunchAndScale());
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("pinball"))
        {
            ball = null;
        }
    }

    IEnumerator AutoLaunchAndScale()
    {
        isLaunching = true;
        
        while (Mathf.Abs(transform.localScale.y - pressedYScale) > 0.01f)
        {
            float newY = Mathf.Lerp(transform.localScale.y, pressedYScale, Time.deltaTime * scalingSpeed);
            transform.localScale = new Vector3(transform.localScale.x, newY, transform.localScale.z);
            yield return null;
        }
        transform.localScale = new Vector3(transform.localScale.x, pressedYScale, transform.localScale.z);
        
        if (ball != null)
        {
            ball.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }
        
    
        while (Mathf.Abs(transform.localScale.y - restYScale) > 0.01f)
        {
            float newY = Mathf.Lerp(transform.localScale.y, restYScale, Time.deltaTime * scalingSpeed);
            transform.localScale = new Vector3(transform.localScale.x, newY, transform.localScale.z);
            yield return null;
        }
        transform.localScale = new Vector3(transform.localScale.x, restYScale, transform.localScale.z);

        isLaunching = false;
    }
}
