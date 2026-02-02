using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody2D body;
    public Vector2 direction;
    public float impulse;
    public Vector2 levelBounds;
    public int spinAmount;

    Vector2 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        Reset();
        body.linearVelocity = direction.normalized * impulse;
        body.angularVelocity = spinAmount;
    }

    // Update is called once per frame
    void Update()
    {
        float ballAngle = Vector2.Angle(transform.position, body.linearVelocity);
        if (ballAngle < 90 &&
        (transform.position.x < -levelBounds.x ||
        transform.position.x > levelBounds.x ||
        transform.position.y < -levelBounds.y ||
        transform.position.y > levelBounds.y 
        ))
        {
            Reset();
        }
    }

    public void Reset()
    {
        transform.position = startPosition;
        body.linearVelocity = direction.normalized * impulse;

        // reset score
        GameManager.instance.score = 0;
    }
}
