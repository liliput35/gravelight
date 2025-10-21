using UnityEngine;

public class GemRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; // degrees per second

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
