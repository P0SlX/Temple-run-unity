using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ObstacleGeneration : MonoBehaviour
{
    public GameObject player;

    [FormerlySerializedAs("Obstacle du haut")]
    public GameObject obstacleOver;

    [FormerlySerializedAs("Obstacle du bas")]
    public GameObject obstacleUnder;

    [FormerlySerializedAs("Obstacle une voie")]
    public GameObject obstacleOneWay;

    [FormerlySerializedAs("Obstacle deux voie")]
    public GameObject obstacleTwoWay;

    [FormerlySerializedAs("Séparation des obstacles")]
    public int triggerCoordObstacle = 20;

    private List<GameObject> _obstacles;
    private int _lastObstacleCoord = 20;

    // Start is called before the first frame update
    void Start()
    {
        _obstacles = new List<GameObject>();
        
        // Ajoute 10 obstacles au début
        for (var i = 0; i < 10; i++)
        {
            AddObstacle(GetRandomObstacle());
        }
    }

    // Update is called once per frame
    void Update()
    {
        var playerX = player.transform.position.x;

        // Ajoute un obstacle
        if (playerX >= triggerCoordObstacle)
        {
            AddObstacle(GetRandomObstacle());
            triggerCoordObstacle += 20;
        }

        // Supprime un obstacle si le joueur l'a passé depuis 20 unités
        if (playerX >= _obstacles.First().transform.position.x + 20)
        {
            RemoveObstacle();
        }
    }

    private void AddObstacle(GameObject go)
    {
        // Récup la position de l'obstacle et l'instancie à la bonne position
        var goPosition = go.transform.position;
        var newObstacle = Instantiate(go, new Vector3(_lastObstacleCoord + 20, goPosition.y, goPosition.z), go.transform.rotation);
        
        // Ajoute l'obstacle à la liste pour le détruire quand il sera passé
        _obstacles.Add(newObstacle);
        _lastObstacleCoord += 20;
    }

    private void RemoveObstacle()
    {
        var obstacle = _obstacles.First();
        _obstacles.Remove(obstacle);
        Destroy(obstacle);
    }

    private GameObject GetRandomObstacle()
    {
        return Random.Range(0, 4) switch
        {
            0 => obstacleOver,
            1 => obstacleUnder,
            // 2 => obstacleOneWay,
            // 3 => obstacleTwoWay,
            _ => obstacleOver
        };
    }
}    
