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

    // Start is called before the first frame update
    void Start()
    {
        _obstacles = new List<List<GameObject>>();

        // Ajoute 10 obstacles au début
        for (var i = 0; i < 10; i++)
        {
            AddObstacle();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var playerX = player.transform.position.x;

        // Ajoute un obstacle
        if (playerX >= triggerCoordObstacle)
        {
            AddObstacle();
            triggerCoordObstacle += 20;
        }

        // Supprime un obstacle si le joueur l'a passé depuis 20 unités
        if (playerX >= _obstacles.First().First().transform.position.x + 20)
        {
            RemoveObstacle();
        }
    }

    private void AddObstacle()
    {
        var type = Random.Range(0, 2);
        var list = new List<GameObject>();

        // Obstacle simple
        if (type == 0)
        {
            var obstacle = GetRandomObstacleOneWay();
            var obstaclePosition = obstacle.transform.position;
            var zCoord = _zValues[Random.Range(0, 3)];
            var newObstacle = Instantiate(obstacle, new Vector3(_lastObstacleCoord + 20, obstaclePosition.y, zCoord),
                obstacle.transform.rotation);
            list.Add(newObstacle);

            // Génére un deuxième obstacle ?
            if (Random.Range(1, 3) == 1)
            {
                var go = GetRandomObstacleOneWay();
                obstaclePosition = obstacle.transform.position;
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
        // Obstacle à 3 voies
        else
        {
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
        var obstacle = _obstacles.First();
        _obstacles.RemoveAt(0);
        foreach (var go in obstacle)
        {
            Destroy(go);
        }
    }

    private GameObject GetRandomObstacleThreeWay()
    {
        return Random.Range(0, 2) switch
        {
            0 => obstacleOver,
            1 => obstacleUnder,
            _ => obstacleOver
        };
    }

    private GameObject GetRandomObstacleOneWay()
    {
        return Random.Range(0, 2) switch
        {
            0 => stumpObstacle,
            1 => bushObstacle,
            _ => bushObstacle
        };
    }
}