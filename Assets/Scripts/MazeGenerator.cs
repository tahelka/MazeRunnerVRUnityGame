using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeNode m_NodePrefab;
    [SerializeField] private int m_Rows;
    [SerializeField] private int m_Cols;
    private readonly int r_NodeDiameter = 5;
    private Vector2Int m_MazeSize;

    private void Start()
    {
        m_MazeSize = new Vector2Int(m_Cols * r_NodeDiameter, m_Rows * r_NodeDiameter);
        StartCoroutine(generateMaze(m_MazeSize));
    }

    private IEnumerator generateMaze(Vector2Int i_Size)
    {
        List<MazeNode> nodes = new List<MazeNode>();

        // Create nodes
        for(int x = 0; x < i_Size.x; x+=5)
        {
            for(int y = 0; y < i_Size.y; y+=5)
            {
                Vector3 nodePos = new Vector3(x - (i_Size.x / 2f), 0, y - (i_Size.y / 2f));
                MazeNode newNode = Instantiate(m_NodePrefab, nodePos, Quaternion.identity, transform);
                nodes.Add(newNode);

                yield return null;
            }
        }

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> complitedNodes = new List<MazeNode>();

        // Choose starting node
        currentPath.Add(nodes[Random.Range(0,nodes.Count)]);
        currentPath[0].SetState(NodeState.Current);

        // While there are uncompleted nodes left

    }
}
