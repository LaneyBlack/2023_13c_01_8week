using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject[] points;
    private int currentPointIndex = 0;
    private int direction = 1;

    [SerializeField] private float speed = 2f;

    private void Update()
    {
        if (Vector2.Distance(points[currentPointIndex].transform.position, transform.position) < .1f)
        {
            currentPointIndex += direction;
            
            if (currentPointIndex >= points.Length || currentPointIndex < 0)
            {
                direction *= -1;
                currentPointIndex += direction;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[currentPointIndex].transform.position,
            Time.deltaTime * speed);
    }
}
