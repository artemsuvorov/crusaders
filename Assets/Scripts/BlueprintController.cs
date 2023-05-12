using UnityEngine;

public class BlueprintController : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private readonly Color availableColor = new Color(50, 255, 0, 180);
    private readonly Color unavailableColor = new Color(255, 3, 0, 180);

    void Update()
    {
        var position = GetMousePositionInWorld();
        transform.position = position;

        if (Input.GetMouseButtonDown(0))
        {
            var rotation = Quaternion.identity;
            Instantiate(prefab, position, rotation);
            Destroy(gameObject);
        }
    }

    private Vector2 GetMousePositionInWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
