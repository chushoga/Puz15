using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // -----------------------------------------------------------------
    /* References */
    // -----------------------------------------------------------------

    LevelManager lm; // Level manager reference.
    bool gameOver = false; // Game over flag.

    [Tooltip("Game Board")]
    public GameObject GameBoard;

    [Tooltip("Puzzle Peices")]
    public List<GameObject> puzzlePeices;

    [SerializeField]
    private List<GameObject> spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
        // Find and set the LevelManager object and set it to the reference.
        lm = GameObject.Find("LevelManager").gameObject.GetComponent<LevelManager>();
        // Instantiate the Gamebord first then the peices.
        GameObject board = Instantiate(GameBoard, new Vector3(0f, 0f, 0f), Quaternion.identity);
        // Create a parent for the puzzle peices.
        GameObject puzzleContainer = new GameObject("Peices");
        // create the list here for the snap points
        spawnPoints = new List<GameObject>();

        // Get and load up all the spawn points.
        int pos = 0; // counter for spawn points.
        foreach (Transform t in board.transform)
        {            
            if (t.tag == "PeiceSpawn")
            {
                // add to the peice list
                SpawnPoint spawn = t.GetComponent<SpawnPoint>();
                spawn.location = pos; // Update the position of the spawn point to keep track of array.
                spawnPoints.Add(t.gameObject); // For keeping the inspector clutter free. Just housekeeping.
                pos++; // Increment the position for array.
            }
        }

        print(spawnPoints.Count);

        int GamePeice_i = 0;
        // Instantiate a game peice for each spawn point in the board object.
        foreach (Transform t in board.transform)
        {
            if(t.tag == "PeiceSpawn")
            {
                print(GamePeice_i);
                GameObject p = Instantiate(puzzlePeices[0], new Vector3(t.transform.position.x, t.transform.position.y, t.transform.position.z), Quaternion.identity);
                p.GetComponent<GamePeice>().location = GamePeice_i; // Give the peices an index
                p.GetComponentInChildren<TextMesh>().text = p.GetComponent<GamePeice>().location.ToString(); // set the text for the peice
                p.transform.parent = puzzleContainer.transform; // For keeping the inspector clutter free. Just housekeeping.
                GamePeice_i++;
            }
        }

        // Shuffle the peices
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            GameObject temp = spawnPoints[i];
            int randomIndex = Random.Range(i, spawnPoints.Count);
            spawnPoints[i] = spawnPoints[randomIndex];
            spawnPoints[randomIndex] = temp;
        }

        // put the peices in the correct spot


    }

    // Game update
    private void Update()
    {
        float speed = 100f;
        //Camera.main.transform.Rotate(new Vector3(0f,0f,1f) * Time.deltaTime * speed);
    }
}
