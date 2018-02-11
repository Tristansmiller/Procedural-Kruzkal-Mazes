using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Coord
{
    public int row;
    public int col;
    public Coord(int r, int c)
    {
        row = r;
        col = c;
    }
    public bool Equals(Coord other)
    {
        if (this.row == other.row && this.col == other.col)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
public class Edge
{
    public Coord cellA;
    public Coord cellB;

    public bool solid;
    public Edge(Coord a, Coord b)
    {
        cellA = a;
        cellB = b;
        solid = true;
    }

    public bool Equals(Edge other)
    {
        if ((this.cellA.Equals(other.cellA) && this.cellB.Equals(other.cellB)) || (this.cellA.Equals(other.cellB) && this.cellB.Equals(other.cellA)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
public class gridCell
{
    public bool northWall = true;
    public bool southWall = true;
    public bool eastWall = true;
    public bool westWall = true;
    public bool hasObject = false;
    public int objectScore = 0;
    public Coord index;
    public gridCell(Coord i)
    {
        index = i;

    }
}
public class TreeNode
{
    public TreeNode parent;
    public Coord self;
    public List<TreeNode> children;

}
public struct Maze<T>
{
    public T[,] cellMatrix;
    public int rows;
    public int cols;
    public Maze(int row, int col)
    {
        cellMatrix = new T[row, col];
        rows = row;
        cols = col;
    }
}

public class MazeGen : MonoBehaviour {

    GameObject player;
    float sizeOfCells;
    int sizeMaze;
    bool waitingForRestart = false;
    public static bool containsEdge(List<Edge> listOfEdges, Edge edge)
    {
        for (int i = 0; i < listOfEdges.Count; i++)
        {
            if (listOfEdges[i].Equals(edge))
            {
                return true;
            }
        }
        return false;
    }
    public static List<Edge> initEdges(int rows, int cols)
    {
        List<Edge> edgeList = new List<Edge>();
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (r - 1 >= 0)
                {
                    Edge newEdge = new Edge(new Coord(r, c), new Coord(r - 1, c));
                    if (!containsEdge(edgeList, newEdge))
                    {
                        edgeList.Add(newEdge);
                    }
                }
                if (r + 1 < rows)
                {
                    Edge newEdge = new Edge(new Coord(r, c), new Coord(r + 1, c));
                    if (!containsEdge(edgeList, newEdge))
                    {
                        edgeList.Add(newEdge);
                    }
                }
                if (c - 1 >= 0)
                {
                    Edge newEdge = new Edge(new Coord(r, c), new Coord(r, c - 1));
                    if (!containsEdge(edgeList, newEdge))
                    {
                        edgeList.Add(newEdge);
                    }
                }
                if (c + 1 < cols)
                {
                    Edge newEdge = new Edge(new Coord(r, c), new Coord(r, c + 1));
                    if (!containsEdge(edgeList, newEdge))
                    {
                        edgeList.Add(newEdge);
                    }
                }
            }
        }
        return edgeList;
    }

    public static Maze<TreeNode> initTreeNodeMaze(int rows, int cols)
    {
        Maze<TreeNode> maze = new Maze<TreeNode>(rows, cols);
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                maze.cellMatrix[r, c] = new TreeNode();
                maze.cellMatrix[r, c].self = new Coord(r, c);
            }
        }
        return maze;
    }

    public static Maze<gridCell> initGridCellMaze(int rows, int cols)
    {
        Maze<gridCell> maze = new Maze<gridCell>(rows, cols);
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                maze.cellMatrix[r, c] = new gridCell(new Coord(r, c));
            }
        }
        return maze;
    }

    public static Maze<gridCell> mapWallsToMaze(Maze<gridCell> maze, List<Edge> walls)
    {
        for (int i = 0; i < walls.Count; i++)
        {
            if (!walls[i].solid)
            {
                if (walls[i].cellA.row < walls[i].cellB.row)
                {
                    maze.cellMatrix[walls[i].cellA.row, walls[i].cellA.col].northWall = false;
                    maze.cellMatrix[walls[i].cellB.row, walls[i].cellB.col].southWall = false;
                }
                if (walls[i].cellA.row > walls[i].cellB.row)
                {
                    maze.cellMatrix[walls[i].cellA.row, walls[i].cellA.col].southWall = false;
                    maze.cellMatrix[walls[i].cellB.row, walls[i].cellB.col].northWall = false;
                }
                if (walls[i].cellA.col < walls[i].cellB.col)
                {
                    maze.cellMatrix[walls[i].cellA.row, walls[i].cellA.col].eastWall = false;
                    maze.cellMatrix[walls[i].cellB.row, walls[i].cellB.col].westWall = false;
                }
                if (walls[i].cellA.col > walls[i].cellB.col)
                {
                    maze.cellMatrix[walls[i].cellA.row, walls[i].cellA.col].westWall = false;
                    maze.cellMatrix[walls[i].cellB.row, walls[i].cellB.col].eastWall = false;
                }
            }
        }
        return maze;
    }
    private static System.Random rng = new System.Random();

    public static IList<T> RandomSort<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    public static TreeNode getRoot(TreeNode node)
    {
        while (node.parent != null)
        {
            node = node.parent;
        }
        return node;
    }
    public static List<Edge> generateListMazeWalls(int rows, int cols)
    {
        Maze<TreeNode> maze = initTreeNodeMaze(rows, cols);
        List<Edge> edges = initEdges(rows, cols);
        edges = (List<Edge>)RandomSort<Edge>(edges);
        Edge currEdge = null;
        TreeNode cellA, cellB, cellARoot, cellBRoot = null;
        for (int i = 0; i < edges.Count; i++)
        {
            currEdge = edges[i];
            cellA = maze.cellMatrix[currEdge.cellA.row, currEdge.cellA.col];
            cellB = maze.cellMatrix[currEdge.cellB.row, currEdge.cellB.col];
            cellARoot = getRoot(cellA);
            cellBRoot = getRoot(cellB);
            if (!cellARoot.self.Equals(cellBRoot.self))
            {
                edges[i].solid = false;
                cellBRoot.parent = cellA;
            }
            maze.cellMatrix[cellA.self.row, cellA.self.col] = cellA;
            maze.cellMatrix[cellB.self.row, cellB.self.col] = cellB;
            maze.cellMatrix[cellARoot.self.row, cellARoot.self.col] = cellARoot;
            maze.cellMatrix[cellBRoot.self.row, cellBRoot.self.col] = cellBRoot;
        }
        return edges;
    }
    public static void printMaze(List<Edge> walls, int size)
    {
        Maze<gridCell> maze = new Maze<gridCell>(size, size);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                maze.cellMatrix[i, j] = new gridCell(new Coord(i, j));
            }
        }
        maze = mapWallsToMaze(maze, walls);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                // if(maze.cellMatrix[i,j].westWall){
                //     Console.Write("|");
                // }
                // else{
                //     Console.Write(" ");
                // }
                if (maze.cellMatrix[i, j].southWall)
                {
                    Debug.Log("_");
                    Console.Write("_");
                }
                else
                {
                    Debug.Log(" ");
                    Console.Write(" ");
                }
                if (maze.cellMatrix[i, j].eastWall)
                {
                    Debug.Log("|");
                    Console.Write("|");
                }
                else
                {
                    Debug.Log(" ");
                    Console.Write(" ");
                }
            }
            Debug.Log("\n");
            Console.WriteLine();
        }
    }

    public void mapMazeToWallObjects(Maze<gridCell> maze)
    {
        float xSize = sizeOfCells;
        float ySize = sizeOfCells/5;
        float cellSize = sizeOfCells;
        GameObject wall = new GameObject("wall");
        wall.AddComponent<SpriteRenderer>();
        SpriteRenderer renderer = wall.GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("Wall");
        renderer.color = Color.black;
        renderer.sortingOrder = 1;
        wall.transform.localScale = new Vector3(xSize, ySize, 1);
       // wall.AddComponent<Rigidbody2D>();
        wall.AddComponent<BoxCollider2D>();
      //  Rigidbody2D rigidBody = wall.GetComponent<Rigidbody2D>();
        BoxCollider2D collider = wall.GetComponent<BoxCollider2D>();
       // rigidBody.mass = 1000000f;
      //  rigidBody.gravityScale = 0f;
        float offset = 4;
        Physics2D.IgnoreCollision(collider, collider);
        wall.AddComponent<StormWall>();
        for (int row = 0; row < maze.rows; row++)
        {
            for(int col = 0; col < maze.cols; col++)
            {
                if (maze.cellMatrix[row, col].northWall)
                {
                    Vector3 topLeftPosition = new Vector3(col*cellSize-offset,row*cellSize-offset,0);
                    wall.transform.position = topLeftPosition;
                    Quaternion rot = Quaternion.Euler(0, 0, 0);
                    wall.transform.rotation = rot;
                    Instantiate(wall);


                }
                if (maze.cellMatrix[row, col].eastWall)
                {
                    Vector3 topLeftPosition = new Vector3(col*cellSize-offset+(cellSize/2),row*cellSize-offset-(cellSize/2), 0);
                    wall.transform.position = topLeftPosition;
                    Quaternion rot = Quaternion.Euler(0, 0, 90);
                    wall.transform.rotation = rot;
                    Instantiate(wall);
                }
                if (maze.cellMatrix[row, col].southWall && row==0)
                { 
                   Vector3 topLeftPosition = new Vector3(col*cellSize-offset,(row-1)*cellSize-offset, 0);
                    wall.transform.position = topLeftPosition;
                    Quaternion rot = Quaternion.Euler(0, 0, 0);
                    wall.transform.rotation = rot;
                    Instantiate(wall);
                }
                if (maze.cellMatrix[row, col].westWall && col==0)
                {
                    Vector3 topLeftPosition = new Vector3(col * cellSize - offset - (cellSize / 2), row * cellSize - offset - (cellSize / 2), 0);
                    wall.transform.position = topLeftPosition;
                    Quaternion rot = Quaternion.Euler(0, 0, 90);
                    wall.transform.rotation = rot;
                    Instantiate(wall);
                }
            }
        }
    }
    Maze<gridCell> setExitCell(Maze<gridCell> maze)
    {
        int exitRow = rng.Next(0, maze.cols - 1);
        maze.cellMatrix[exitRow, maze.cols-1].eastWall = false;
        createStormForMaze(maze,exitRow,maze.cols-1);
        createExitWall(exitRow, maze.cols - 1);
        return maze;
    }
    void createExitWall(int row,int col)
    {
        GameObject wall = new GameObject("ExitWall");
        wall.AddComponent<SpriteRenderer>();
        SpriteRenderer renderer = wall.GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("Wall");
        renderer.color = new Color(.6f,0,0,81f/255f);
        renderer.sortingOrder = 1;
        wall.transform.localScale = new Vector3(sizeOfCells/5, sizeOfCells, 1);
        wall.AddComponent<LevelCompletion>();
        wall.AddComponent<BoxCollider2D>();
        BoxCollider2D collider = wall.GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
        float offset = 4;
        Physics2D.IgnoreCollision(collider, collider);
        Vector3 topLeftPosition = new Vector3(col * sizeOfCells - offset + (sizeOfCells / 2), row * sizeOfCells - offset - (sizeOfCells / 2), 0);
        wall.transform.position = topLeftPosition;

        
        //Instantiate(wall);
    }
    Maze<gridCell> addPointObjectsToMaze(Maze<gridCell> maze, int numObjects)
    {
        int numGenerated = 0;
        if (numObjects < maze.rows * maze.cols)
        {
            while (numGenerated < numObjects)
            {
                int randCol = rng.Next(0, maze.cols - 1);
                int randRow = rng.Next(0, maze.rows - 1);
                if (!maze.cellMatrix[randRow, randCol].hasObject)
                {
                    maze.cellMatrix[randRow, randCol].hasObject = true;
                    maze.cellMatrix[randRow, randCol].objectScore = rng.Next(1, 100);
                    numGenerated++;
                }
            }
        }
        return maze;
    }

    void mapMazeToPointObjects(Maze<gridCell> maze,float cellSize)
    {
        float radius = sizeOfCells / 3;
        GameObject point = new GameObject("point");
        point.AddComponent<SpriteRenderer>();
        SpriteRenderer renderer = point.GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("Point");
        renderer.sortingOrder = 1;
        point.transform.localScale = new Vector3(radius, radius, 1);
        // wall.AddComponent<Rigidbody2D>();
        point.AddComponent<BoxCollider2D>();
        //  Rigidbody2D rigidBody = wall.GetComponent<Rigidbody2D>();
        BoxCollider2D collider = point.GetComponent<BoxCollider2D>();
        // rigidBody.mass = 1000000f;
        //  rigidBody.gravityScale = 0f;
        float offset = 4;
        Physics2D.IgnoreCollision(collider, collider);
        point.AddComponent<StormWall>();
        for(int row = 0; row < maze.rows; row++)
        {
            for(int col = 0; col < maze.cols; col++)
            {
                if (maze.cellMatrix[row, col].hasObject)
                {
                    point.transform.position = new Vector3(sizeOfCells * col - offset, sizeOfCells * row - (sizeOfCells / 2) - offset);
                    renderer.color = Color.blue;
                    Instantiate(point);
                }
            }
        }
    }
    void createStormForMaze(Maze<gridCell> maze,int exitCellRow,int exitCellCol)
    {
        GameObject storm = new GameObject("Storm");
        storm.AddComponent<SpriteRenderer>();
        SpriteRenderer renderer = storm.GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("Storm");
        renderer.sortingOrder = 0;
        renderer.color = new Color(1, 1, 1, 1);
        storm.transform.position = new Vector3(exitCellCol * sizeOfCells - 4+(sizeOfCells/2), exitCellRow * sizeOfCells - 4-(sizeOfCells/2),0);
        storm.transform.localScale = new Vector3(maze.cols * 2, maze.rows * 2, 1);
        storm.AddComponent<CircleCollider2D>();
        storm.GetComponent<CircleCollider2D>().isTrigger = true;
        storm.AddComponent<StormShrink>();

    }
    void placePlayerAtMazeStart(Maze<gridCell> maze, float offset, GameObject player)
    {
        int randRow = rng.Next(0, maze.cols - 1);
        player.transform.position = new Vector3(0 - offset, randRow * sizeOfCells - offset - (sizeOfCells / 2), 0);

    }
    GameObject findPlayer()
    {
        var gameObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject gameObj in gameObjects)
        {
            if(gameObj.name == "Player")
            {
                return gameObj;
            }
        }
        return null;
    }
    void CreateInitialMaze(int mazeSize)
    {
        int size = mazeSize;
        Maze<gridCell> maze = initGridCellMaze(size, size);
        List<Edge> walls = generateListMazeWalls(size, size);
        maze = mapWallsToMaze(maze, walls);
        maze = setExitCell(maze);
        maze = addPointObjectsToMaze(maze, 10);
        mapMazeToWallObjects(maze);
        mapMazeToPointObjects(maze, .5f);
        placePlayerAtMazeStart(maze, 4, player);
    }
    void wipeCurrentScene()
    {
        var gameobjects = FindObjectsOfType<GameObject>();
        foreach(GameObject obj in gameobjects)
        {
            if(obj.name!="Player"&&obj.name!="Main Camera" && obj.name != "Backdrop" && obj.name!="Score Text" && obj.name!="Time Text" && obj.name!="ScoreCanvas" && obj.name!="EventSystem")
            {
                Destroy(obj);
            }
        }
    }
    GameObject findTime()
    {
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject gameobj in gameObjects)
        {
            if (gameobj.name == "Time Text")
            {
                return gameobj;
            }
        }
        return null;
    }
    void WaitForRestart()
    {
        GameObject timeText = findTime();
        timeText.GetComponent<Text>().text = "Press Space to Start";
        waitingForRestart = true;
        wipeCurrentScene();
        player.GetComponent<PlayerAnimation>().Invoke("freeze", 0);
    }
    void CreateNewMaze()
    {
        wipeCurrentScene();
        sizeMaze++;
        sizeOfCells = (1 / (float)sizeMaze) * 10;
        CreateInitialMaze(sizeMaze);
        player.AddComponent<StormWall>();
    }
    void startGame()
    {
        sizeMaze = 15;
        sizeOfCells = (1 / (float)sizeMaze) * 10;
        CreateInitialMaze(sizeMaze);
    }
	// Use this for initialization
	void Start () {
        player = findPlayer();
        WaitForRestart();
    }
	
	// Update is called once per frame
	void Update () {
        if (waitingForRestart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startGame();
                waitingForRestart = false;
                player.GetComponent<PlayerAnimation>().Invoke("unfreeze", 0);
            }
        }
	}
}
