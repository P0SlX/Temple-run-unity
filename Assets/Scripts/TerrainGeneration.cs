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


    private GameObject terrain;
    private List<GameObject> terrains;
    private int triggerCoordTerrain = 50;
    private int lastTerrainCoord = -50;

    private GameObject decor;
    private List<GameObject> decors;
    private int triggerCoordDecor = 60;
    private int lastDecorCoord = -60;
    private Quaternion decorRotationLeft = Quaternion.Euler(0, 180, 0);
    private Quaternion decorRotationRight = Quaternion.identity;

    private Quaternion collectibleRotation = Quaternion.Euler(-90, 0, 0);
    private List<List<GameObject>> collectibles;

    // Start is called before the first frame update
    private void Start()
    {
        collectibles = new List<List<GameObject>>();
        terrain = Resources.Load("Terrain") as GameObject;
        terrains = new List<GameObject>();
        
        for (var i = 0; i < 5; i++)
        {
            AddTerrain();
            lastTerrainCoord += 50;
        }

        decor = Resources.Load("Decor") as GameObject;
        decors = new List<GameObject>();
        
        for (var i = 0; i < 5; i++)
        {
            decors.Add(Instantiate(decor, new Vector3(lastDecorCoord + 60, 0, 19.5f), decorRotationLeft));
            decors.Add(Instantiate(decor, new Vector3(lastDecorCoord + 60, 0, -19.5f), decorRotationRight));
            lastDecorCoord += 60;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        var playerX = player.transform.position.x;

        // Generate a new terrain if the player is near the end of the current one
        if (playerX >= triggerCoordTerrain)
        {
            AddTerrain();
            RemoveGoBehindPlayer(terrains);
            triggerCoordTerrain += 50;
            lastTerrainCoord += 50;
        }
        else if (playerX >= triggerCoordDecor)
        {
            decors.Add(Instantiate(decor, new Vector3(lastDecorCoord + 60, 0, 19.5f), decorRotationLeft));
            decors.Add(Instantiate(decor, new Vector3(lastDecorCoord + 60, 0, -19.5f), decorRotationRight));
            RemoveGoBehindPlayer(decors);
            RemoveGoBehindPlayer(decors);
            triggerCoordDecor += 60;
            lastDecorCoord += 60;
        }
        
        // Remove collectibles that are behind the player
        if (collectibles.Count <= 0) return;
        if (!(playerX >= collectibles.First().First().transform.position.x + 50)) return;
        foreach (var collectible in collectibles.First())
        {
            Destroy(collectible);
        }
        collectibles.RemoveAt(0);
    }

    private void AddTerrain()
    {
        var terrainInstance = Instantiate(terrain, new Vector3(lastTerrainCoord + 50, 0.27f, 0), Quaternion.identity);
        terrains.Add(terrainInstance);
        var random = Random.Range(0, 10);

        switch (random)
        {
            // 10% d'avoir 3 rangés de collectables
            // 30% d'avoir 2 rangés ...
            // 50% d'avoir 1 rangée ...
            case < 1:
                // 3 rangées de pièces
                GenerateCollectibles(3, lastTerrainCoord + 50);
                break;
            case < 3:
                // 2 rangées de pièces
                GenerateCollectibles(2, lastTerrainCoord + 50);
                break;
            default:
                // 1 rangée de pièces
                GenerateCollectibles(1, lastTerrainCoord + 50);
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
                tmp.Add(Instantiate(go, pos, collectibleRotation));
            }
            collectibles.Add(tmp);
            offset += increment;
        }
    }

    private void RemoveGoBehindPlayer(List<GameObject> gameObjects)
    {
        // Remove the first plateau if the player position (int) % 100 == 0
        var firstGo = gameObjects[0];
        gameObjects.RemoveAt(0);
        Destroy(firstGo);
    }
}