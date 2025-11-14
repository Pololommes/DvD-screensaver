using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public float speed = 3f;                // Geschwindigkeit
    private Vector2 velocity;               // Bewegungsrichtung
    private SpriteRenderer sr;
    private Camera cam;
    private Vector2 halfSize;               // halbe Sprite-Größe

    void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();

        // Initiale Richtung: zufällig diagonal
        velocity = new Vector2(Random.value > 0.5f ? 1 : -1, Random.value > 0.5f ? 1 : -1).normalized * speed;

        // Sprite-Größe bestimmen
        if (sr != null)
            halfSize = sr.bounds.extents;
        else
            halfSize = Vector2.one * 0.5f;

        // Optional: zufällige Startposition innerhalb der Kamera
        Vector3 min = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 max = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        transform.position = new Vector3(
            Random.Range(min.x + halfSize.x, max.x - halfSize.x),
            Random.Range(min.y + halfSize.y, max.y - halfSize.y),
            0);
    }

    void Update()
    {
        // Bewegung
        Vector3 pos = transform.position;
        pos += (Vector3)velocity * Time.deltaTime;
        transform.position = pos;

        // Kamera-Grenzen live berechnen (bei Resize automatisch)
        Vector3 min = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 max = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        bool bounced = false;

        // X-Richtung prüfen
        if (pos.x - halfSize.x <= min.x)
        {
            pos.x = min.x + halfSize.x;
            velocity.x = Mathf.Abs(velocity.x);
            bounced = true;
        }
        else if (pos.x + halfSize.x >= max.x)
        {
            pos.x = max.x - halfSize.x;
            velocity.x = -Mathf.Abs(velocity.x);
            bounced = true;
        }

        // Y-Richtung prüfen
        if (pos.y - halfSize.y <= min.y)
        {
            pos.y = min.y + halfSize.y;
            velocity.y = Mathf.Abs(velocity.y);
            bounced = true;
        }
        else if (pos.y + halfSize.y >= max.y)
        {
            pos.y = max.y - halfSize.y;
            velocity.y = -Mathf.Abs(velocity.y);
            bounced = true;
        }

        // Position korrigieren und Farbe ändern, falls gebounced
        if (bounced)
        {
            transform.position = pos;

            if (sr != null)
            {
                // Zufällige Farbe bei jedem Bounce
                sr.color = Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.7f, 1f);
            }
        }
    }
}
