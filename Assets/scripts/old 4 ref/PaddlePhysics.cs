using UnityEngine;

public class PaddlePhysics : MonoBehaviour
{
    public float maxReflectAngle;

    public AudioSource audioSource;
    public AudioClip meowClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D ball = other.attachedRigidbody;
        if (ball != null) {
            Vector2 paddleNormal = transform.up;
            float ballAngle = Vector2.Angle(paddleNormal, ball.linearVelocity);
            if (ballAngle > 90)
            {
                // reflect ball velocity
                Vector2 reflectedVelocity = Vector2.Reflect(ball.linearVelocity, paddleNormal);

                // clamp reflection angle
                // which direction to rotate to?
                float reflectAngle = Vector2.SignedAngle(paddleNormal, reflectedVelocity);
                if (Mathf.Abs(reflectAngle) > maxReflectAngle) // check bounce
                {
                    float deltaAngle = (Mathf.Sign(reflectAngle) * maxReflectAngle) - reflectAngle;
                    Quaternion clampRotation = Quaternion.Euler(0, 0, deltaAngle);
                    reflectedVelocity = clampRotation * reflectedVelocity;
                }

                // update ball velocity
                ball.linearVelocity = reflectedVelocity;
                

                GameManager.instance.AddPoint(); // score points
                audioSource.PlayOneShot(meowClip);
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
