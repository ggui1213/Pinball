using UnityEngine;

public class launcher : MonoBehaviour
{
    [SerializeField]
    private float force;

    private Rigidbody2D ball;

    [SerializeField]
    private float maxTime;
    private float timer;
    
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
            ball = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("pinball"))
        {
            ball = null;
        }
    }

    void Launch()
    {
        if (ball)
        {
            ball.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        Vector3 scale = transform.localScale;

        if (Input.GetKey(KeyCode.Space))
        {
            timer += Time.deltaTime;
            scale.y = Mathf.Lerp(scale.y, pressedYScale, Time.deltaTime * scalingSpeed);
        }
        else
        {
            scale.y = Mathf.Lerp(scale.y, restYScale, Time.deltaTime * scalingSpeed);
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Launch();
                timer = 0;
            }
        }
        
        transform.localScale = scale;
    }
}