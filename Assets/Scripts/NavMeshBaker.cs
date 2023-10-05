using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public List<MazeNode> m_NavMeshSurfaces = new List<MazeNode>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildNavMeshSurfaces(List<MazeNode> i_MazeNodes)
    {
        foreach (MazeNode mazeNode in i_MazeNodes)
        {
            //mazeNode.AddComponent<NavMeshSurface>();
            m_NavMeshSurfaces.Add(mazeNode);
            mazeNode.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }
}
