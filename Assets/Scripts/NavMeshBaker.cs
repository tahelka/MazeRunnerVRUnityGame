using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public List<NavMeshSurface> m_NavMeshSurfaces = new List<NavMeshSurface>();

    public void BuildNavMeshSurfaces(List<MazeNode> i_MazeNodes)
    {
        foreach (MazeNode mazeNode in i_MazeNodes)
        {
            Transform floor = mazeNode.transform.Find("Floor");

            // Check if the "Floor" object exists
            if (floor != null)
            {
                NavMeshSurface navMeshSurfaceComponent = floor.GetComponent<NavMeshSurface>();

                // Check if NavMeshSurface component exists
                if (navMeshSurfaceComponent != null)
                {
                    // NavMeshSurface component found
                    navMeshSurfaceComponent.BuildNavMesh();
                    m_NavMeshSurfaces.Add(navMeshSurfaceComponent);
                }
                else
                {
                    Debug.LogWarning("NavMeshSurface component not found on the 'Floor' object.");
                }
            }
            else
            {
                Debug.LogWarning("Child object 'Floor' not found under mazeNode.");
            }
        }
    }

    public void AddNavMeshAgent(List<GameObject> i_Enemies)
    {
        foreach (GameObject enemy in i_Enemies)
        {
            enemy.AddComponent<NavMeshAgent>();
        }
    }
}
