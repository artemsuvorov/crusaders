using NavMeshPlus.Components;
using UnityEngine;

public class NavigationSurfaceRebaker : MonoBehaviour
{
    private NavMeshSurface surface;

    public void Rebake()
    {
        surface.BuildNavMesh();
    }

    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
    }
}
