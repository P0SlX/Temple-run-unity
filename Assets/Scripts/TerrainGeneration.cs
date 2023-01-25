using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public GameObject player;
    private GameObject plateau;
    private List<GameObject> plateaux;
    private int triggerCoord = 500;

    // Start is called before the first frame update
    void Start()
    {
        plateau = Resources.Load("Terrain") as GameObject;
        plateaux = new List<GameObject>()
        {
            Instantiate(plateau, new Vector3(0, 0, 0), Quaternion.identity),
            Instantiate(plateau, new Vector3(500, 0, 0), Quaternion.identity)
        };
    }

    // Update is called once per frame
    void Update()
    {
        // Generate a new plateau if the player is near the end of the current one
        if ((int)player.transform.position.x >= triggerCoord)
        {
            AddNewPlateauAtTheEnd();
            RemovePlateauBehindPlayer();
        }
    }

    private void AddNewPlateauAtTheEnd()
    {
        var lastPlateauTransform = plateaux.Last().transform;
        var newPlateau = Instantiate(plateau,
            new Vector3(lastPlateauTransform.position.x + (lastPlateauTransform.localScale.x * 10), 0, 0),
            Quaternion.identity);
        plateaux.Add(newPlateau);
        triggerCoord += 500;
    }

    private void RemovePlateauBehindPlayer()
    {
        // Remove the first plateau if the player position (int) % 100 == 0
        var firstPlateau = plateaux[0];
        plateaux.RemoveAt(0);
        Destroy(firstPlateau);
    }
}