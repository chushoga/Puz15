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

    // Start is called before the first frame update
    void Start()
    {
        
        // Find and set the LevelManager object and set it to the reference.
        lm = GameObject.Find("LevelManager").gameObject.GetComponent<LevelManager>();

        // Instantiate the Gamebord first then the peices.
        GameObject board = Instantiate(GameBoard, new Vector3(0f, 0f, 0f), Quaternion.identity);
        // Create a parent for the puzzle peices.
        GameObject puzzleCenter = new GameObject("Peices");

        // Instantiate a game peice for each spawn point in the board object.
        foreach (Transform t in board.transform)
        {
            if(t.tag == "PeiceSpawn")
            {
                //GameObject p = Instantiate(puzzlePeices[0], new Vector3(t.transform.position.x, t.transform.position.y, t.transform.position.z), Quaternion.identity);
                //p.transform.parent = puzzleCenter.transform; // For keeping the inspector clutter free. Just housekeeping.
            }
        }
    }

    // Game update
    private void Update()
    {
        float speed = 100f;
        Camera.main.transform.Rotate(new Vector3(0f,0f,1f) * Time.deltaTime * speed);
    }
}
