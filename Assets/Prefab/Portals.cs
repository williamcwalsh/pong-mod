using UnityEngine;

public class Portals : MonoBehaviour
{
    [SerializeField]
    private Transform portal1;

    [SerializeField]
    private Transform portal2;

    [SerializeField]
    private float spawnRangeX = 3f;

    [SerializeField]
    private float spawnRangeY = 4f;

    [SerializeField]
    private float minPortalDistance = 1.5f;

    [SerializeField]
    private float exitOffset = 0.5f;

    private bool isUsed;

    private void Reset()
    {
        portal1 = transform.Find("Portal1");
        portal2 = transform.Find("Portal2");
    }

    private void Awake()
    {
        if (portal1 == null)
        {
            portal1 = transform.Find("Portal1");
        }

        if (portal2 == null)
        {
            portal2 = transform.Find("Portal2");
        }

        SetupPortal(portal1);
        SetupPortal(portal2);
    }

    private void Start()
    {
        PositionPortals();
    }

    public void TeleportFrom(PortalEndpoint enteredPortal, Ball ball)
    {
        if (isUsed || enteredPortal == null || ball == null)
        {
            return;
        }

        Transform destination = enteredPortal.transform == portal1 ? portal2 : portal1;

        if (destination == null)
        {
            return;
        }

        isUsed = true;

        Vector3 exitDirection = (destination == portal1 ? Vector3.left : Vector3.right) * exitOffset;
        ball.transform.position = destination.position + exitDirection;

        Destroy(gameObject);
    }

    private void SetupPortal(Transform portal)
    {
        if (portal == null)
        {
            return;
        }

        CircleCollider2D circleCollider = portal.GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            circleCollider.isTrigger = true;
        }

        PortalEndpoint endpoint = portal.GetComponent<PortalEndpoint>();
        if (endpoint == null)
        {
            endpoint = portal.gameObject.AddComponent<PortalEndpoint>();
        }

        endpoint.Initialize(this);
    }

    private void PositionPortals()
    {
        if (portal1 == null || portal2 == null)
        {
            return;
        }

        Vector2 portal1Position = GetRandomPortalPosition();
        Vector2 portal2Position = GetRandomPortalPosition();

        int attempts = 0;
        while ((portal1Position - portal2Position).sqrMagnitude < minPortalDistance * minPortalDistance && attempts < 25)
        {
            portal2Position = GetRandomPortalPosition();
            attempts++;
        }

        if ((portal1Position - portal2Position).sqrMagnitude < minPortalDistance * minPortalDistance)
        {
            portal2Position = portal1Position + Vector2.up * minPortalDistance;
            portal2Position.x = Mathf.Clamp(portal2Position.x, -spawnRangeX, spawnRangeX);
            portal2Position.y = Mathf.Clamp(portal2Position.y, -spawnRangeY, spawnRangeY);
        }

        portal1.position = portal1Position;
        portal2.position = portal2Position;
    }

    private Vector2 GetRandomPortalPosition()
    {
        return new Vector2(Random.Range(-spawnRangeX, spawnRangeX), Random.Range(-spawnRangeY, spawnRangeY));
    }
}

public class PortalEndpoint : MonoBehaviour
{
    private Portals portals;

    public void Initialize(Portals parentPortals)
    {
        portals = parentPortals;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (portals == null)
        {
            return;
        }

        if (!other.TryGetComponent(out Ball ball))
        {
            return;
        }

        portals.TeleportFrom(this, ball);
    }
}
