using UnityEngine;

public class BlueprintController : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private bool hasCollision = false;

    private SpriteRenderer spriteRenderer;

    private readonly Color availableColor = Color.green;
    private readonly Color unavailableColor = Color.red;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = availableColor;
    }

    void Update()
    {
        var position = GetMousePositionInWorld();
        transform.position = position;

        if (!hasCollision && Input.GetMouseButtonDown(0))
        {
            var rotation = Quaternion.identity;
            Instantiate(prefab, position, rotation);
            Destroy(gameObject);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Building"))
        {
            hasCollision = true;
            spriteRenderer.color = unavailableColor;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Building"))
        {
            hasCollision = false;
            spriteRenderer.color = availableColor;
        }
    }

    private Vector2 GetMousePositionInWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
