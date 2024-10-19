using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour
{
    public float speed = 2f; 
    public float changeDirectionTime = 2f; 
    public float detectionRadius = 0.5f; 
    public float proximityDistance = 1f; 
    public float idleTime = 5f; 
    public string dialogueMessage = "Hello!"; 

    private Vector2 direction;
    private float changeDirectionTimer;
    private bool isIdle = false;

    void Start()
    {
        ChangeDirection();
        changeDirectionTimer = changeDirectionTime;
    }

    void Update()
    {
        if (isIdle)
        {
            return; 
        }

        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0)
        {
            ChangeDirection();
            changeDirectionTimer = changeDirectionTime;
        }

        if (IsObstacleAhead())
        {
            ChangeDirection();
        }

        CheckProximityToOtherNPCs();

        CheckProximityToPlayer();

        transform.Translate(direction * speed * Time.deltaTime);
        RestrictMovement();
    }

    private void ChangeDirection()
    {
        float angle = Random.Range(0f, 360f);
        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }

    private bool IsObstacleAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius);
        return hit.collider != null; 
    }

    private void RestrictMovement()
    {
        Camera cam = Camera.main;
        Vector3 viewportPosition = cam.WorldToViewportPoint(transform.position);
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            ChangeDirection();
        }
    }

    private void CheckProximityToOtherNPCs()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, proximityDistance);
        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != gameObject && collider.CompareTag("NPC"))
            {
                StartCoroutine(InteractWithNPC(collider.transform));
                break;
            }
        }
    }

    private void CheckProximityToPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, proximityDistance, LayerMask.GetMask("Player"));
        if (playerCollider != null)
        {
            StartCoroutine(InteractWithPlayer(playerCollider.transform));
        }
    }

    private IEnumerator InteractWithNPC(Transform otherNPC)
    {
        isIdle = true; 

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = otherNPC.position;
        float elapsedTime = 0f;
        float duration = 0.5f; 

        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        Debug.Log(dialogueMessage); 
        yield return new WaitForSeconds(idleTime);

        ChangeDirection();
        isIdle = false; 
    }

    private IEnumerator InteractWithPlayer(Transform player)
    {
        isIdle = true; 

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = player.position;
        float elapsedTime = 0f;
        float duration = 0.5f; 

        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        Debug.Log(dialogueMessage); 
        yield return new WaitForSeconds(idleTime);

        ChangeDirection();
        isIdle = false; 
    }
}
