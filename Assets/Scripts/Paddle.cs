using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Paddle : MonoBehaviour
{
    protected Rigidbody2D rb;
    private Vector3 initialScale;

    public float speed = 8f;
    [Tooltip("Changes how the ball bounces off the paddle depending on where it hits the paddle. The further from the center of the paddle, the steeper the bounce angle.")]
    public bool useDynamicBounce = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
    }

    public void ResetPosition()
    {
        rb.linearVelocity = Vector2.zero;
        rb.position = new Vector2(rb.position.x, 0f);
        transform.localScale = initialScale;
    }

    public void Stretch(float scaleMultiplier)
    {
        Vector3 stretchedScale = initialScale;
        stretchedScale.y *= scaleMultiplier;
        transform.localScale = stretchedScale;
    }

    public void Shrink(float scaleMultiplier)
    {
        Vector3 shrunkenScale = initialScale;
        shrunkenScale.y *= scaleMultiplier;
        transform.localScale = shrunkenScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            ball.SetLastHitPaddle(this);
            GameManager.Instance?.RegisterPaddleHit();

            if (useDynamicBounce)
            {
                Rigidbody2D ballRb = collision.rigidbody;
                Collider2D paddle = collision.otherCollider;

                // Gather information about the collision
                Vector2 ballDirection = ballRb.linearVelocity.normalized;
                Vector2 contactDistance = ballRb.transform.position - paddle.bounds.center;
                Vector2 surfaceNormal = collision.GetContact(0).normal;
                Vector3 rotationAxis = Vector3.Cross(Vector3.up, surfaceNormal);

                // Rotate the direction of the ball based on the contact distance
                // to make the gameplay more dynamic and interesting
                float maxBounceAngle = 75f;
                float bounceAngle = contactDistance.y / paddle.bounds.size.y * maxBounceAngle;
                ballDirection = Quaternion.AngleAxis(bounceAngle, rotationAxis) * ballDirection;

                // Re-apply the new direction to the ball
                ballRb.linearVelocity = ballDirection * ballRb.linearVelocity.magnitude;
            }
        }
    }

}
