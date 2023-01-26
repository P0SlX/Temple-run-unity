using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public GameObject player;

    private GameObject terrain;
    private List<GameObject> terrains;
    private int triggerCoordTerrain = 50;
    private int lastTerrainCoord = 200;

    private GameObject decor;
    private List<GameObject> decors;
    private int triggerCoordDecor = 60;
    private int lastDecorCoord = 240;
    private Quaternion decorRotationLeft = Quaternion.Euler(0, 180, 0);
    private Quaternion decorRotationRight = Quaternion.identity;

    // Start is called before the first frame update
    private void Start()
    {
        terrain = Resources.Load("Terrain") as GameObject;
        terrains = new List<GameObject>()
        {
            Instantiate(terrain, new Vector3(0, 0.27f, 0), Quaternion.identity),
            Instantiate(terrain, new Vector3(50, 0.27f, 0), Quaternion.identity),
            Instantiate(terrain, new Vector3(100, 0.27f, 0), Quaternion.identity),
            Instantiate(terrain, new Vector3(150, 0.27f, 0), Quaternion.identity),
            Instantiate(terrain, new Vector3(200, 0.27f, 0), Quaternion.identity),
        };

        decor = Resources.Load("Decor") as GameObject;
        decors = new List<GameObject>()
        {
            Instantiate(decor, new Vector3(0, 0, 19.5f), decorRotationLeft),
            Instantiate(decor, new Vector3(0, 0, -19.5f), decorRotationRight),
            Instantiate(decor, new Vector3(60, 0, 19.5f), decorRotationLeft),
            Instantiate(decor, new Vector3(60, 0, -19.5f), decorRotationRight),
            Instantiate(decor, new Vector3(120, 0, 19.5f), decorRotationLeft),
            Instantiate(decor, new Vector3(120, 0, -19.5f), decorRotationRight),
            Instantiate(decor, new Vector3(180, 0, 19.5f), decorRotationLeft),
            Instantiate(decor, new Vector3(240, 0, -19.5f), decorRotationRight),
            Instantiate(decor, new Vector3(240, 0, 19.5f), decorRotationLeft),
            Instantiate(decor, new Vector3(180, 0, -19.5f), decorRotationRight),
        };
    }

    // Update is called once per frame
    private void Update()
    {
        var playerX = (int)player.transform.position.x;

        // Generate a new plateau if the player is near the end of the current one
        if (playerX >= triggerCoordTerrain)
        {
            terrains.Add(Instantiate(terrain, new Vector3(lastTerrainCoord + 50, 0.27f, 0), Quaternion.identity));
            RemovePlateauBehindPlayer(terrains);
            triggerCoordTerrain += 50;
            lastTerrainCoord += 50;
        }
        else if (playerX >= triggerCoordDecor)
        {
            decors.Add(Instantiate(decor, new Vector3(lastDecorCoord + 60, 0, 19.5f), decorRotationLeft));
            decors.Add(Instantiate(decor, new Vector3(lastDecorCoord + 60, 0, -19.5f), decorRotationRight));
            RemovePlateauBehindPlayer(decors);
            RemovePlateauBehindPlayer(decors);
            triggerCoordDecor += 60;
            lastDecorCoord += 60;
        }
    }

    private void RemovePlateauBehindPlayer(List<GameObject> gameObjects)
    {
        // Remove the first plateau if the player position (int) % 100 == 0
        var firstGo = gameObjects[0];
        gameObjects.RemoveAt(0);
        Destroy(firstGo);
    }
}