using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlusOne : MonoBehaviour
{
    [SerializeField]
    private bool destroyAfterUse = true;

    private void Reset()
    {
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("plus one");
        if (!other.TryGetComponent(out Ball ball))
        {
            return;
        }

        if (ball.LastHitPaddle == null)
        {
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.AwardPointTo(ball.LastHitPaddle) && destroyAfterUse)
        {
            Destroy(gameObject);
        }
    }
}
