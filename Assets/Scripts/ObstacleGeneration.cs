using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleGeneration : MonoBehaviour
{
    public GameObject player;
    public GameObject obstacleOver;
    public GameObject obstacleUnder;
    public GameObject bushObstacle;
    public GameObject stumpObstacle;
    public int triggerCoordObstacle = 20;

    private List<List<GameObject>> _obstacles;
    private int _lastObstacleCoord = 20;
    private int[] _zValues = { -2, 0, 2 };

    private void Start()
    {
        _obstacles = new List<List<GameObject>>();

        // Add 10 obstacles at the start
        for (var i = 0; i < 10; i++)
        {
            AddObstacle();
        }
    }

    private void Update()
    {
        var playerX = player.transform.position.x;

        // Add an obstacle
        if (playerX >= triggerCoordObstacle)
        {
            AddObstacle();
            triggerCoordObstacle += 20;
        }

        // Delete obstacle if it's behind the player (20 units)
        if (playerX >= _obstacles.First().First().transform.position.x + 20)
        {
            RemoveObstacle();
        }
    }

    private void AddObstacle()
    {
        // Obstacle type
        // 0 = 1 way obstacle
        // 1 = 3 way obstacle
        var type = Random.Range(0, 2);
        var list = new List<GameObject>();

        if (type == 0)
        {
            // Spawn 1 obstacle
            var obstacle = GetRandomObstacleOneWay();
            var obstaclePosition = obstacle.transform.position;
            var zCoord = _zValues[Random.Range(0, 3)];
            var newObstacle = Instantiate(obstacle, new Vector3(_lastObstacleCoord + 20, obstaclePosition.y, zCoord),
                obstacle.transform.rotation);
            list.Add(newObstacle);

            // Need to spawn a second one just for fun ?
            if (Random.Range(1, 3) == 1)
            {
                var go = GetRandomObstacleOneWay();
                obstaclePosition = obstacle.transform.position;
                
                // Do not spawn at the same position as the first one
                int zCoord2;
                do
                {
                    zCoord2 = _zValues[Random.Range(0, 3)];
                } while (zCoord == zCoord2);

                var newObstacle2 = Instantiate(go,
                    new Vector3(_lastObstacleCoord + 20, obstaclePosition.y, zCoord2),
                    obstacle.transform.rotation);
                list.Add(newObstacle2);
            }
        }
        else
        {
            // Spawn a 3 way obstacle 
            var obstacle = GetRandomObstacleThreeWay();
            var obstaclePosition = obstacle.transform.position;
            var newObstacle = Instantiate(obstacle,
                new Vector3(_lastObstacleCoord + 20, obstaclePosition.y, obstaclePosition.z),
                obstacle.transform.rotation);
            list.Add(newObstacle);
        }

        _lastObstacleCoord += 20;
        _obstacles.Add(list);
    }

    private void RemoveObstacle()
    {
        // Remove the first obstacle in the list
        var obstacle = _obstacles.First();
        _obstacles.RemoveAt(0);

        foreach (var go in obstacle)
        {
            Destroy(go);
        }
    }

    private GameObject GetRandomObstacleThreeWay()
    {
        // 0 = Tree laid over the road
        // 1 = Log
        return Random.Range(0, 2) switch
        {
            0 => obstacleOver,
            1 => obstacleUnder,
            _ => obstacleOver
        };
    }

    private GameObject GetRandomObstacleOneWay()
    {
        // 0 = Stump
        // 1 = Bush
        return Random.Range(0, 2) switch
        {
            0 => stumpObstacle,
            1 => bushObstacle,
            _ => bushObstacle
        };
    }
}