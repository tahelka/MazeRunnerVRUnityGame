using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private int m_MazeYValue;
    [SerializeField] private int m_Rows;
    [SerializeField] private int m_Cols;
    private readonly int r_NodeDiameter = 5;
    private Vector2Int m_MazeSize;

    private void Start()
    {
        m_MazeSize = new Vector2Int(m_Cols * r_NodeDiameter, m_Rows * r_NodeDiameter);

        generateMazeInstant(m_MazeSize);
        // StartCoroutine(generateMaze(m_MazeSize));
    }

    private void generateMazeInstant(Vector2Int i_Size)
    {
        List<MazeNode> nodes = new List<MazeNode>();
        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();
        List<MazeNode> longestPath = new List<MazeNode>();
        int maxPathLength = 0;

        // Create all maze nodes
        for (int x = 0; x < i_Size.x; x += 5)
        {
            for(int y = 0; y < i_Size.y; y += 5)
            {
                Vector3 nodePos = new(x - (i_Size.x / 2f), m_MazeYValue, y - (i_Size.y / 2f));
                MazeNode newNode = Instantiate(m_NodePrefab, nodePos, Quaternion.identity, transform);
                nodes.Add(newNode);
            }
        }

        // Choose starting node
        int firstNodeIndex = Random.Range(0, nodes.Count);
        MazeNode startNode = nodes[firstNodeIndex];
        currentPath.Add(startNode); // Add the first node to the current path
        longestPath.Add(startNode); // Add the first node to the longest path
        startNode.SetState(NodeState.Start); // Mark the Start node
        maxPathLength++;

        Debug.Log($"First node: {firstNodeIndex}");
        Debug.Log($"currentNodeX: {firstNodeIndex / m_Rows}");
        Debug.Log($"currentNodeY: {firstNodeIndex % m_Rows}");

        // Run DFS and create paths in the maze
        while (completedNodes.Count < nodes.Count)
        {
            List<int> possibleNextNodes = new();
            List<int> possibleDirections = new();

            int currentNodeIndex = nodes.IndexOf(currentPath[^1]); // Get the index of the last node in the current path
            Debug.Log($"currentNodeIndex: {currentNodeIndex}");
            int currentNodeX = currentNodeIndex / m_Rows;
            Debug.Log($"currentNodeX: {currentNodeX}");
            int currentNodeY = currentNodeIndex % m_Rows;
            Debug.Log($"currentNodeY: {currentNodeY}");

            // Check if from the current node it's possible to go RIGHT
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

            // Check if from the current node it's possible to go LEFT
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

            // Check if from the current node it's possible to go UP
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

            // Check if from the current node it's possible to go DOWN
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
                        currentPath[^1].RemoveWall((int)eWall.RightWall); // Remove the right wall of the current node
                        break;
                    case (int)eDirections.LeftDirections:
                        chosenNode.RemoveWall((int)eWall.RightWall); // Remove the right wall of the chosen node
                        currentPath[^1].RemoveWall((int)eWall.LeftWall); // Remove the left wall of the current node
                        break;
                    case (int)eDirections.UpDirections:
                        chosenNode.RemoveWall((int)eWall.DownWall); // Remove the down wall of the chosen node
                        currentPath[^1].RemoveWall((int)eWall.UpWall); // Remove the up wall of the current node
                        break;
                    case (int)eDirections.DownDirections:
                        chosenNode.RemoveWall((int)eWall.UpWall); // Remove the up wall of the chosen node
                        currentPath[^1].RemoveWall((int)eWall.DownWall); // Remove the down wall of the current node
                        break;
                }
                
                currentPath.Add(chosenNode);
            }
            else
            {
                // check if the current path is the longest path so far
                if (currentPath.Count > maxPathLength)
                {
                    // Copy the elements from currentPath to longestPath
                    longestPath.Clear(); // Clear longestPath if it has any previous data
                    longestPath.AddRange(currentPath); // Copy elements from currentPath to longestPath
                    maxPathLength = currentPath.Count;
                }

                completedNodes.Add(currentPath[^1]);
                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }

        // Set the farthest node from the Start node to be the End node
        if (longestPath.Count > 0)
        {
            MazeNode endNode = longestPath[^1]; // Get the last node of the longest path
            // MazeNode endNode = FindFarthestEndNode(nodes, startNode);
            endNode.SetState(NodeState.End); // Mark the End node
        }
    }

    // Add this method to your MazeGenerator class to find the farthest end node from the start node
    private MazeNode FindFarthestEndNode(List<MazeNode> allNodes, MazeNode startNode)
    {
        MazeNode farthestNode = null;
        float maxDistance = 0f;

        foreach (MazeNode node in allNodes)
        {
            // Calculate the distance between the start and current node
            float distance = Vector3.Distance(startNode.transform.position, node.transform.position);

            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestNode = node;
            }
        }

        return farthestNode;
    }

    private IEnumerator generateMaze(Vector2Int i_Size)
    {
        List<MazeNode> nodes = new List<MazeNode>();

        // Create nodes
        for (int x = 0; x < i_Size.x; x += 5)
        {
            for (int y = 0; y < i_Size.y; y += 5)
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
        currentPath.Add(nodes[Random.Range(0, nodes.Count)]);
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
