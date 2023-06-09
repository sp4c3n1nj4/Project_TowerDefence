using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private TileManager manager;
    [SerializeField]
    private Pathfinding pathfinding;
    [SerializeField]
    private EnemySpawner spawner;

    private Vector2Int[] Path;

    private List<int> deadEnemies = new List<int>();
   
    private float percentage;
    private int pathIndex;

    public float enemyOffset;
    public float speed = 1;

    public bool hasPath = false;

    public Vector2Int startTile;
    public Vector2Int endTile;
    public bool moveEnemies;

    private void Start()
    {
        manager = GameObject.FindObjectOfType<TileManager>();
        pathfinding = GameObject.FindObjectOfType<Pathfinding>();

        spawner = gameObject.GetComponent<EnemySpawner>();

        DoPathfinding();

        manager.GridObstacleChange.AddListener(CheckPathfinding);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoPathfinding();
        }
    }

    public void StartWave(float _enemyOffset, float _speed)
    {
        percentage = 0;
        moveEnemies = true;
        enemyOffset = _enemyOffset;
        speed = _speed;
        deadEnemies= new List<int>();
    }

    public void EndWave()
    {
        moveEnemies = false;
    }

    private void CheckPathfinding()
    {
        if (Path == null)
            return;


        for (int i = 0; i < Path.Length; i++)
        {
            if (manager.grid[Path[i].x, Path[i].y].walkable != true)
            {
                DoPathfinding();
                return;
            }
        }

        Vector2Int[] _path = pathfinding.FindPath(startTile, endTile);
        if (_path.Length > 0 && _path.Length < Path.Length)
        {
            Path = _path;
            hasPath = true;
        }
        else if (_path.Length < 0)
        {

        }
    }

    private void DoPathfinding()
    {
        Vector2Int[] _path = pathfinding.FindPath(startTile, endTile);
        if (_path.Length > 0)
        {
            Path = _path;
            hasPath = true;
        }
        else
        {
            Path = new Vector2Int[0];
            hasPath = false;
            PathfindingError();
        }
    }

    private void PathfindingError()
    {
        Debug.LogError("no valid path found");
    }

    private void FollowPath()
    {
        int arraySkip = 0;

        GameObject[] Enemies = spawner.enemies.ToArray();
        for (int i = 0; i < Enemies.Length; i++)
        {
            int j = i + arraySkip;
            while (deadEnemies.Contains(j))
            {
                arraySkip++;
                j = i + arraySkip;
            }
            
            float newP = percentage - j * enemyOffset;

            if (newP >= Path.Length - 1)
            {
                Enemies[i].GetComponent<Enemy>().ReachedEnd();
                deadEnemies.Add(j);
                print(j);

                continue;
            }               
            newP = Mathf.Clamp(newP, 0, Path.Length - 1.001f);

            int s = Mathf.FloorToInt(newP);
            int e = s + 1;
            float p = newP - s;
            Vector3 pos = Vector3.Lerp(manager.GetTile(Path[s]), manager.GetTile(Path[e]), p);

            pos.y = 0.15f;
            Enemies[i].transform.position = pos;
        }
        percentage += speed / 50;
        //print(percentage);
    }

    private void FixedUpdate()
    {
        if (!moveEnemies)
            return;

        FollowPath();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Path != null)
        {
            for (int i = 0; i < Path.Length; i++)
            {
                Gizmos.DrawWireCube(manager.GetTile(Path[i]), new Vector3(.8f, .01f, .8f));

                if (i != 0)
                    Gizmos.DrawLine(manager.GetTile(Path[i - 1]), manager.GetTile(Path[i]));
            }
        }
    }
}
