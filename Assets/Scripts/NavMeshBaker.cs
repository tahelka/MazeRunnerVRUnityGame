using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public List<NavMeshSurface> m_NavMeshSurfaces = new List<NavMeshSurface>();

    public void BuildNavMeshSurfaces(Transform i_ParentOfNodeMazes)
    {
        for (int i = 0; i < i_ParentOfNodeMazes.childCount; i++)
        {
            Transform floor = i_ParentOfNodeMazes.GetChild(i).transform.Find("Floor");

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

    public void AddNavMeshAgent(List<GameObject> i_Enemies, Transform i_PointToSpawnEnemiesOnSurface)
    {
        foreach (GameObject enemy in i_Enemies)
        { 
            // Place agent on the navMeshSurface and add to it navMeshAgent
            enemy.transform.SetPositionAndRotation(i_PointToSpawnEnemiesOnSurface.position, i_PointToSpawnEnemiesOnSurface.rotation);
            NavMeshAgent navMeshAgentComponent = enemy.AddComponent<NavMeshAgent>();
            //navMeshAgentComponent.autoBraking = false;
            //avMeshAgentComponent.autoRepath = true;
            //navMeshAgentComponent.destination = GameObject.Find("Main Camera").transform.position;
            navMeshAgentComponent.speed = 2.5f;
            //navMeshAgentComponent.radius = 2.5f;
        }
    }
}
