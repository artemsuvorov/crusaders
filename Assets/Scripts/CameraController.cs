using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(1.0f, 50.0f)]
    private float panSpeed = 10.0f;

    //[SerializeField, Range(1.0f, 100.0f)]
    //private float panBorderThickness = 20.0f;

    void Update()
    {
        var vertical = Input.GetAxisRaw("Vertical");
        var horizontal = Input.GetAxisRaw("Horizontal");
        var direction = new Vector3(horizontal, vertical, 0);
        transform.position += panSpeed * Time.deltaTime * direction;
    }
}
