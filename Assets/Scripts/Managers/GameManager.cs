using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // -----------------------------------------------------------------
    /* References */
    // -----------------------------------------------------------------

    LevelManager lm; // Level manager reference.

    [Tooltip("Game Board")]
    public GameObject GameBoard;
    private GameObject board;

    [Tooltip("Puzzle Peices")]
    public List<GameObject> puzzlePeices;

    [SerializeField]
    private List<GameObject> spawnPoints;

    private GameObject puzzleContainer;

    // Start is called before the first frame update
    void Start()
    {

        // Find and set the LevelManager object and set it to the reference.
        lm = GameObject.Find("LevelManager").gameObject.GetComponent<LevelManager>();
                
        // create the list here for the snap points
        spawnPoints = new List<GameObject>();

        LoadPeices();
        RandomizeBoard();
    }

    // Game update
    private void Update()
    {
        
    }

    public void LoadPeices()
    {
        // Instantiate the Gamebord first then the peices.
        board = Instantiate(GameBoard, new Vector3(0f, 0f, 0f), Quaternion.identity);

        // Create a parent for the puzzle peices.
        puzzleContainer = new GameObject("Peices");

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

        int GamePeice_i = 0;
        // Instantiate a game peice for each spawn point in the board object.
        foreach (Transform t in board.transform)
        {
            if (t.tag == "PeiceSpawn" && GamePeice_i != (spawnPoints.Count - 1))
            {
                GameObject p = Instantiate(puzzlePeices[0], new Vector3(t.transform.position.x, t.transform.position.y, t.transform.position.z), Quaternion.identity);
                p.GetComponent<GamePeice>().location = GamePeice_i; // Give the peices an index
                p.GetComponentInChildren<TextMesh>().text = p.GetComponent<GamePeice>().location.ToString(); // set the text for the peice
                p.transform.parent = puzzleContainer.transform; // For keeping the inspector clutter free. Just housekeeping.
                GamePeice_i++;
            }
        }
    }

    public void RandomizeBoard()
    {

        // Shuffle the peices
        
        List<GameObject> tempGM = spawnPoints;
        for (int i = 0; i < tempGM.Count; i++)
        {
            GameObject temp = tempGM[i];
            int randomIndex = Random.Range(i, tempGM.Count);
            tempGM[i] = tempGM[randomIndex];
            tempGM[randomIndex] = temp;
        }
        

        // Instantiate a game peice for each spawn point in the board object.
        int j = 0;
        foreach (Transform t in puzzleContainer.transform)
        {
           
            if (t.tag == "GamePeice")
            {
                //print(spawnPoints[j].transform.position.x);
                print(j);
                t.position = new Vector3(tempGM[j].transform.position.x, tempGM[j].transform.position.y, tempGM[j].transform.position.z);
                j++; 
            }
        }

        
        // put the peices in the correct spot
    }
}
