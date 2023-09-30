using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    DownWall = 3,
    Amount = 4
}

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeNode m_NodePrefab;
    [SerializeField] private MazeNode m_StartNodePrefab;
    [SerializeField] private MazeNode m_EndNodePrefab;
    [SerializeField] private MazeNode m_Obstacle1NodePrefab;
    [SerializeField] private int m_MazeYValue;
    private readonly int r_NodeDiameter = 5;
    private List<MazeNode> m_Nodes;
    private List<MazeNode> m_CurrentPath;
    private List<MazeNode> m_CompletedNodes;
    private List<MazeNode> m_LongestPath;
    private int m_MaxPathLength;
    private Vector2Int m_MazeSize;

    public MazeNode StartNode { get; private set; }
    public MazeNode EndNode { get; private set; }

    public void Awake()
    {
        m_Nodes = new List<MazeNode>();
        m_CurrentPath = new List<MazeNode>();
        m_CompletedNodes = new List<MazeNode>();
        m_LongestPath = new List<MazeNode>();
        m_MaxPathLength = 0;
    }

    public void GenerateMazeInstant(GameLevel i_Level, int i_Rows, int i_Cols)
    {
        m_MazeSize = new Vector2Int(i_Cols * r_NodeDiameter, i_Rows * r_NodeDiameter);

        // Create all maze nodes
        for (int x = 0; x < m_MazeSize.x; x += 5)
        {
            for(int y = 0; y < m_MazeSize.y; y += 5)
            {
                Vector3 nodePos = new(x - (m_MazeSize.x / 2f), m_MazeYValue, y - (m_MazeSize.y / 2f));
                MazeNode newNode = Instantiate(m_NodePrefab, nodePos, Quaternion.identity, transform);
                m_Nodes.Add(newNode);
            }
        }

        // Choose starting node
        int firstNodeIndex = Random.Range(0, m_Nodes.Count);
        StartNode = m_Nodes[firstNodeIndex];
        m_CurrentPath.Add(StartNode); // Add the first node to the current path
        m_LongestPath.Add(StartNode); // Add the first node to the longest path
        m_MaxPathLength++;

        Debug.Log($"First node: {firstNodeIndex}");
        Debug.Log($"currentNodeX: {firstNodeIndex / i_Rows}");
        Debug.Log($"currentNodeY: {firstNodeIndex % i_Rows}");

        // Run DFS and create paths in the maze
        while (m_CompletedNodes.Count < m_Nodes.Count)
        {
            List<int> possibleNextNodes = new();
            List<int> possibleDirections = new();

            int currentNodeIndex = m_Nodes.IndexOf(m_CurrentPath[^1]); // Get the index of the last node in the current path
            Debug.Log($"currentNodeIndex: {currentNodeIndex}");
            int currentNodeX = currentNodeIndex / i_Rows;
            Debug.Log($"currentNodeX: {currentNodeX}");
            int currentNodeY = currentNodeIndex % i_Rows;
            Debug.Log($"currentNodeY: {currentNodeY}");

            // Check if from the current node it's possible to go RIGHT
            if (currentNodeX < i_Cols - 1)
            {
                // Check node to the right of the current node
                if (!m_CompletedNodes.Contains(m_Nodes[currentNodeIndex + i_Rows])
                   && !m_CurrentPath.Contains(m_Nodes[currentNodeIndex + i_Rows]))
                {
                    possibleDirections.Add((int)eDirections.RightDirections);
                    possibleNextNodes.Add(currentNodeIndex + i_Rows);
                }
            }

            // Check if from the current node it's possible to go LEFT
            if (currentNodeX > 0)
            {
                // Check node to the left of the current node
                if (!m_CompletedNodes.Contains(m_Nodes[currentNodeIndex - i_Rows])
                    && !m_CurrentPath.Contains(m_Nodes[currentNodeIndex - i_Rows]))
                {
                    possibleDirections.Add((int)eDirections.LeftDirections);
                    possibleNextNodes.Add(currentNodeIndex - i_Rows);
                }
            }

            // Check if from the current node it's possible to go UP
            if (currentNodeY < i_Rows - 1)
            {
                // Check node above the current node
                if (!m_CompletedNodes.Contains(m_Nodes[currentNodeIndex + 1])
                    && !m_CurrentPath.Contains(m_Nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add((int)eDirections.UpDirections);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }

            // Check if from the current node it's possible to go DOWN
            if (currentNodeY > 0)
            {
                // Check node below the current node
                if (!m_CompletedNodes.Contains(m_Nodes[currentNodeIndex - 1])
                    && !m_CurrentPath.Contains(m_Nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add((int)eDirections.DownDirections);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Check if there is a possible direction to move to
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = m_Nodes[possibleNextNodes[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case (int)eDirections.RightDirections:
                        chosenNode.RemoveWall((int)eWall.LeftWall); // Remove the left wall of the chosen node
                        m_CurrentPath[^1].RemoveWall((int)eWall.RightWall); // Remove the right wall of the current node
                        break;
                    case (int)eDirections.LeftDirections:
                        chosenNode.RemoveWall((int)eWall.RightWall); // Remove the right wall of the chosen node
                        m_CurrentPath[^1].RemoveWall((int)eWall.LeftWall); // Remove the left wall of the current node
                        break;
                    case (int)eDirections.UpDirections:
                        chosenNode.RemoveWall((int)eWall.DownWall); // Remove the down wall of the chosen node
                        m_CurrentPath[^1].RemoveWall((int)eWall.UpWall); // Remove the up wall of the current node
                        break;
                    case (int)eDirections.DownDirections:
                        chosenNode.RemoveWall((int)eWall.UpWall); // Remove the up wall of the chosen node
                        m_CurrentPath[^1].RemoveWall((int)eWall.DownWall); // Remove the down wall of the current node
                        break;
                }
                
                m_CurrentPath.Add(chosenNode);
            }
            else
            {
                // check if the current path is the longest path so far
                if (m_CurrentPath.Count > m_MaxPathLength)
                {
                    // Copy the elements from currentPath to longestPath
                    m_LongestPath.Clear(); // Clear longestPath if it has any previous data
                    m_LongestPath.AddRange(m_CurrentPath); // Copy elements from currentPath to longestPath
                    m_MaxPathLength = m_CurrentPath.Count;
                }

                m_CompletedNodes.Add(m_CurrentPath[^1]);
                m_CurrentPath.RemoveAt(m_CurrentPath.Count - 1);
            }
        }

        // Set the farthest node from the Start node to be the End node
        if (m_LongestPath.Count > 0)
        {
            EndNode = m_LongestPath[^1]; // Get the last node of the longest path
        }
        
        // Replace START and END node with the dedicated nodes
        replaceNodeWithStartNodePrefab(m_Nodes);
        replaceNodeWithEndNodePrefab(m_Nodes);

        // Adding obstacles to the maze
        addObstacles(i_Level, m_Nodes);

        // Adding enemies to the maze
        addEnemies(i_Level, m_Nodes);
    }

    private void replaceNodeWithStartNodePrefab(List<MazeNode> i_Nodes)
    {
        if (StartNode != null && m_StartNodePrefab != null)
        {
            // Create a new StartNode using the StartNodePrefab
            Vector3 nodePos = StartNode.transform.position;
            MazeNode newStartNode = Instantiate(m_StartNodePrefab, nodePos, Quaternion.identity, transform);

            // Remove the same walls from the new START node
            bool[] startNodeRemovedWalls = StartNode.GetRemovedWalls();
            for (int wallIndex = 0; wallIndex < (int)eWall.Amount; wallIndex++)
            {
                if(startNodeRemovedWalls[wallIndex])
                {
                    newStartNode.RemoveWall(wallIndex);
                }
            }

            // Remove the current node from the list of nodes and destroy it
            i_Nodes.Remove(StartNode);
            Destroy(StartNode.gameObject);

            // Update the StartNode reference to the new StartNode
            StartNode = newStartNode;

            // Set the state of the new StartNode
            StartNode.SetState(NodeState.Start);
        }
    }

    private void replaceNodeWithEndNodePrefab(List<MazeNode> i_Nodes)
    {
        if (EndNode != null && m_EndNodePrefab != null)
        {
            // Create a new EndNode using the EndNodePrefab
            Vector3 nodePos = EndNode.transform.position;
            MazeNode newEndNode = Instantiate(m_EndNodePrefab, nodePos, Quaternion.identity, transform);

            // Remove the same walls from the new END node
            bool[] endNodeRemovedWalls = EndNode.GetRemovedWalls();
            for (int wallIndex = 0; wallIndex < (int)eWall.Amount; wallIndex++)
            {
                if (endNodeRemovedWalls[wallIndex])
                {
                    newEndNode.RemoveWall(wallIndex);
                }
            }

            // Remove the current node from the list of nodes and destroy it
            i_Nodes.Remove(EndNode);
            Destroy(EndNode.gameObject);

            // Update the EndNode reference to the new EndNode
            EndNode = newEndNode;

            // Set the state of the new EndNode
            EndNode.SetState(NodeState.End);
        }
    }

    private void addObstacles(GameLevel i_Level, List<MazeNode> i_Nodes)
    {
        if (i_Level.Name == "Medium")
        {
            // Add obstacles
        }
        else if (i_Level.Name == "Hard")
        {
            // Add obstacles
        }
        else
        {
            Debug.Log("No obstacles are added at level: " + i_Level);
        }
    }

    private void addEnemies(GameLevel i_Level, List<MazeNode> i_Nodes)
    {
        if (i_Level.Name == "Hard")
        {
            // Add enemies
        }
        else
        {
            Debug.Log("No enemies are added at level: " + i_Level);
        }
    }

    public void DeleteMaze()
    {
        // Destroy all maze nodes
        foreach (MazeNode node in m_Nodes)
        {
            Destroy(node.gameObject);
        }

        // Destroy the START and END nodes
        Destroy(StartNode.gameObject);
        Destroy(EndNode.gameObject);

        // Clear lists and references
        m_Nodes.Clear();
        m_CurrentPath.Clear();
        m_CompletedNodes.Clear();
        m_LongestPath.Clear();
        m_MaxPathLength = 0;

        // Clear StartNode and EndNode references
        StartNode = null;
        EndNode = null;
    }


    /*private IEnumerator generateMaze(Vector2Int i_Size)
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
    }*/
}
