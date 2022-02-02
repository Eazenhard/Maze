using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public Cell CellPrefab;
    public GameObject CellsParent;
    public GameObject Danger;
    public Vector3 CellSize = new Vector3(1, 1, 0);
    public HintRenderer HintRenderer;

    public Maze maze;

    private void Start()
    {
        MazeGenerator generator = new MazeGenerator();
        maze = generator.GenerateMaze();

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                Cell c = Instantiate(CellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity, CellsParent.transform);
                if (Random.Range(0, 10f) <= 2f 
                    && ((y != maze.cells.GetLength(1) - 1 && x != maze.cells.GetLength(0) - 1)
                    && (y != 0 && x != 0) && (y != 1 && x != 0) && (y != 0 && x != 1)))
                {
                    Instantiate(Danger, new Vector3(x * CellSize.x, y * CellSize.y - 13f, y * CellSize.z), Quaternion.identity, CellsParent.transform);
                }
                c.Ground.SetActive(maze.cells[x, y].Ground);
                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
            }
        }

        HintRenderer.DrawPath();
    }
}

