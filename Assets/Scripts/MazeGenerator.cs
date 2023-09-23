using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public enum eDirections
{
    RightDirections = 1, // PosX
    LeftDirections = 2, // NegX
    UpDirections = 3, // PosZ
    DownDirections = 4  // NegZ
}

public enum eWall
{
    RightWall = 0,
    LeftWall = 1,
    UpWall = 2,
    DownWall = 3
}

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
        for(int x = 0; x < i_Size.x; x += 5)
        {
            for(int y = 0; y < i_Size.y; y += 5)
            {
                Vector3 nodePos = new Vector3(x - (i_Size.x / 2f), 0, y - (i_Size.y / 2f));
                MazeNode newNode = Instantiate(m_NodePrefab, nodePos, Quaternion.identity, transform);
                nodes.Add(newNode);

                yield return null;
            }
        }

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        // Choose starting node
        currentPath.Add(nodes[Random.Range(0,nodes.Count)]);
        currentPath[0].SetState(NodeState.Current);

        // While there are uncompleted nodes left, Check nodes next to current node
        while (completedNodes.Count < nodes.Count)
        {
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            Debug.Log($"i_Size.x: {i_Size.x}");
            Debug.Log($"i_Size.y: {i_Size.y}");

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            Debug.Log($"currentNodeIndex: {currentNodeIndex}");
            int currentNodeX = currentNodeIndex / m_Rows;
            Debug.Log($"currentNodeX: {currentNodeX}");
            int currentNodeY = currentNodeIndex % m_Rows;
            Debug.Log($"currentNodeY: {currentNodeY}");

            // Check if the current node is not on the right wall
            if (currentNodeX < m_Cols - 1)
            {
                // Check node to the right of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + m_Rows])
                   && !currentPath.Contains(nodes[currentNodeIndex + m_Rows]))
                {
                    possibleDirections.Add((int)eDirections.RightDirections);
                    possibleNextNodes.Add(currentNodeIndex + m_Rows);
                }
            }

            // Check if the current node is not on the left wall
            if (currentNodeX > 0)
            {
                // Check node to the left of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - m_Rows])
                    && !currentPath.Contains(nodes[currentNodeIndex - m_Rows]))
                {
                    possibleDirections.Add((int)eDirections.LeftDirections);
                    possibleNextNodes.Add(currentNodeIndex - m_Rows);
                }
            }

            // Check if the current node is not on the top wall
            if (currentNodeY < m_Rows - 1)
            {
                // Check node above the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1])
                    && !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add((int)eDirections.UpDirections);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }

            // Check if the current node is not on the below wall
            if (currentNodeY > 0)
            {
                // Check node below the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1])
                    && !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add((int)eDirections.DownDirections);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Check if there is a possible direction to move to
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case (int)eDirections.RightDirections:
                        chosenNode.RemoveWall((int)eWall.LeftWall); // Remove the left wall of the chosen node
                        currentPath[currentPath.Count - 1].RemoveWall((int)eWall.RightWall); // Remove the right wall of the current node
                        break;
                    case (int)eDirections.LeftDirections:
                        chosenNode.RemoveWall((int)eWall.RightWall); // Remove the right wall of the chosen node
                        currentPath[currentPath.Count - 1].RemoveWall((int)eWall.LeftWall); // Remove the left wall of the current node
                        break;
                    case (int)eDirections.UpDirections:
                        chosenNode.RemoveWall((int)eWall.DownWall); // Remove the down wall of the chosen node
                        currentPath[currentPath.Count - 1].RemoveWall((int)eWall.UpWall); // Remove the up wall of the current node
                        break;
                    case (int)eDirections.DownDirections:
                        chosenNode.RemoveWall((int)eWall.UpWall); // Remove the up wall of the chosen node
                        currentPath[currentPath.Count - 1].RemoveWall((int)eWall.DownWall); // Remove the down wall of the current node
                        break;
                }
                
                currentPath.Add(chosenNode);
                chosenNode.SetState(NodeState.Current);
            }
            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);
                currentPath[currentPath.Count - 1].SetState(NodeState.Completed);
                currentPath.RemoveAt(currentPath.Count - 1);
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}
