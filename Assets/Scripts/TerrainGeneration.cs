using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public GameObject player;
    public GameObject coin;
    public GameObject diamond;
    public GameObject diamondBlack;
    public GameObject emerald;
    public GameObject ruby;
    public GameObject finishLine;

    private GameObject _terrain;
    private List<GameObject> _terrains;
    private int _triggerCoordTerrain = 50;
    private int _lastTerrainCoord = -50;

    private GameObject _decor;
    private List<GameObject> _decors;
    private int _triggerCoordDecor = 60;
    private int _lastDecorCoord = -60;
    private Quaternion _decorRotationLeft = Quaternion.Euler(0, 180, 0);
    private Quaternion _decorRotationRight = Quaternion.identity;

    private Quaternion _collectibleRotation = Quaternion.Euler(-90, 0, 0);
    private List<List<GameObject>> _collectibles;
    
    private int _finishCoord;

    private void Start()
    {
        // Initialize lists and load prefabs
        _collectibles = new List<List<GameObject>>();
        _terrain = Resources.Load("Terrain") as GameObject;
        _terrains = new List<GameObject>();
        _decor = Resources.Load("Decor") as GameObject;
        _decors = new List<GameObject>();
        
        // Add 5 terrains at the start
        for (var i = 0; i < 5; i++)
        {
            AddTerrain();
            _lastTerrainCoord += 50;
        }

        // Add 5 decors each side at the start
        for (var i = 0; i < 5; i++)
        {
            _decors.Add(Instantiate(_decor, new Vector3(_lastDecorCoord + 60, 0, 19.5f), _decorRotationLeft));
            _decors.Add(Instantiate(_decor, new Vector3(_lastDecorCoord + 60, 0, -19.5f), _decorRotationRight));
            _lastDecorCoord += 60;
        }

        // If the difficulty is infinite, don't spawn the finish line
        if (DifficultyHandler.IsInfinit) return;
        _finishCoord = DifficultyHandler.Difficulty * 1000 + 5;
        Instantiate(finishLine, new Vector3(_finishCoord, 0.28f, 3.5f), Quaternion.identity);
    }

    private void Update()
    {
        var playerX = player.transform.position.x;

        // Generate a new terrain if the player is near the end of the current one
        if (playerX >= _triggerCoordTerrain)
        {
            AddTerrain();
            RemoveGoBehindPlayer(_terrains);
            _triggerCoordTerrain += 50;
            _lastTerrainCoord += 50;
        }
        else if (playerX >= _triggerCoordDecor)
        {
            _decors.Add(Instantiate(_decor, new Vector3(_lastDecorCoord + 60, 0, 19.5f), _decorRotationLeft));
            _decors.Add(Instantiate(_decor, new Vector3(_lastDecorCoord + 60, 0, -19.5f), _decorRotationRight));
            RemoveGoBehindPlayer(_decors);
            RemoveGoBehindPlayer(_decors);
            _triggerCoordDecor += 60;
            _lastDecorCoord += 60;
        }
        
        // Remove collectibles that are behind the player
        if (_collectibles.Count <= 0) return;
        if (!(playerX >= _collectibles.First().First().transform.position.x + 50)) return;
        
        foreach (var collectible in _collectibles.First())
        {
            Destroy(collectible);
        }
        
        _collectibles.RemoveAt(0);
    }

    private void AddTerrain()
    {
        var terrainInstance = Instantiate(_terrain, new Vector3(_lastTerrainCoord + 50, 0.27f, 0), Quaternion.identity);
        _terrains.Add(terrainInstance);
        var random = Random.Range(0, 10);

        switch (random)
        {
            // 10% d'avoir 3 rangés de collectables
            // 30% d'avoir 2 rangés ...
            // 50% d'avoir 1 rangée ...
            case < 1:
                // 3 rangées de pièces
                GenerateCollectibles(3, _lastTerrainCoord + 50);
                break;
            case < 3:
                // 2 rangées de pièces
                GenerateCollectibles(2, _lastTerrainCoord + 50);
                break;
            default:
                // 1 rangée de pièces
                GenerateCollectibles(1, _lastTerrainCoord + 50);
                break;
        }
    }

    private void GenerateCollectibles(int nbRows, int x)
    {
        // 80% d'avoir une pièce
        // 14% d'avoir une émeraude
        // 5% d'avoir un rubis
        // 0.5% d'avoir un diamant
        // 0.5% d'avoir un diamant noir

        var increment = nbRows switch
        {
            2 => 50 / 3,
            3 => 50 / 2,
            _ => 0.0f
        };
        var offset = -increment;

        for (var i = 0; i < nbRows; i++)
        {
            var tmp = new List<GameObject>();
            for (var j = -2; j <= 2; j += 2)
            {
                var random = Random.Range(0, 1000);
                var pos = new Vector3(x + offset, 1, j);
                var go = random switch
                {
                    < 800 => coin,
                    < 940 => emerald,
                    < 995 => ruby,
                    < 997 => diamond,
                    _ => diamondBlack
                };

                // Add the collectible to the terrain instance to remove it when the terrain is destroyed
                tmp.Add(Instantiate(go, pos, _collectibleRotation));
            }
            _collectibles.Add(tmp);
            offset += increment;
        }
    }

    private static void RemoveGoBehindPlayer(List<GameObject> gameObjects)
    {
        var firstGo = gameObjects[0];
        gameObjects.RemoveAt(0);
        Destroy(firstGo);
    }
}