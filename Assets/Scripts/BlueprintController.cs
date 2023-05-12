using UnityEngine;
using UnityEngine.Events;

public class BlueprintController : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

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
