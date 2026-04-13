using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Stretch : MonoBehaviour
{
    [SerializeField]
    private float stretchMultiplier = 1.25f;

    [SerializeField]
    private bool destroyAfterUse = true;

    private void Reset()
    {
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Ball ball))
        {
            return;
        }

        if (ball.LastHitPaddle == null)
        {
            return;
        }

        ball.LastHitPaddle.Stretch(stretchMultiplier);

        if (destroyAfterUse)
        {
            Destroy(gameObject);
        }
    }
}
