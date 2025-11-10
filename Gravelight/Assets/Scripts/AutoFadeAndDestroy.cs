using UnityEngine;

public class AutoFadeAndDestroy : MonoBehaviour
{
    public float lifetime = 2.5f;
    private float timer = 0f;
    private Renderer rend;
    private Color startColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / lifetime;

        rend.material.color = new Color(startColor.r, startColor.g, startColor.b, 1 - t);

        if (timer >= lifetime)
            Destroy(gameObject);
    }
}
