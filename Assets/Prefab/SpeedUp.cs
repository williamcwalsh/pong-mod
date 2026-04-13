using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SpeedUp : MonoBehaviour
{
    [SerializeField]
    private float speedMultiplier = 1.2f;

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

        ball.currentSpeed *= speedMultiplier;
        Debug.Log("added speed");

        if (destroyAfterUse)
        {
            Destroy(gameObject);
        }
    }
}
