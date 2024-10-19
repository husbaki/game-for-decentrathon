using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public Vector2 spawnPosition;
    private bool isPlayerTouching = false;

    void Start()
    {
        spawnPosition = transform.position; 
    }

    void Update()
    {
        if (IsOutOfCameraView())
        {
            transform.position = spawnPosition; 
        }
        else
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            Debug.Log("Distance to player: " + distanceToPlayer);
            if (isPlayerTouching || distanceToPlayer <= 2.5f)
            {
                Debug.Log("Car stopped due to proximity to player.");
                return;
            }
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerTouching = true;
            Debug.Log("Player entered the trigger.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerTouching = false;
            Debug.Log("Player exited the trigger.");
        }
    }

    bool IsOutOfCameraView()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return (screenPoint.y + 1) < 0; 
    }
}
