using UnityEngine;

public class GhostFloat : MonoBehaviour
{
    public float floatAmplitude = 0.5f; // how high it moves up/down
    public float floatSpeed = 2f;       // how fast it oscillates

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
