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

    [Tooltip("Puzzle Peices")]
    public List<GameObject> puzzlePeices;

    // Start is called before the first frame update
    void Start()
    {
        
        // Find and set the LevelManager object and set it to the reference.
        lm = GameObject.Find("LevelManager").gameObject.GetComponent<LevelManager>();
        float padding = 1.15f;
        int pSize = 4;

        // Create a parent for the puzzle peices.
        GameObject puzzleCenter = new GameObject("PuzzleCenter");

        // Instantiate the puzzle pieces.
        for (int i = 0; i < pSize; i++)
        {
            for (int j = 0; j < pSize; j++)
            {
                if(i == 3 && j == 3){
                    print("skip me" + i + "," + j);
                } else
                {   
                    GameObject p = Instantiate(puzzlePeices[0], new Vector3(i * padding, 0f, -j * padding), Quaternion.identity);
                    p.transform.parent = puzzleCenter.transform;
                }
            }

            // Calculate the center position of the puzzle
            float totalX = 0f;
            float totalZ = 0f;
            foreach(Transform go in puzzleCenter.transform)
            {
                totalX += go.transform.position.x;
                totalZ += go.transform.position.z;
            }
            float centerX = totalX / puzzleCenter.transform.childCount;
            float centerZ = totalZ / puzzleCenter.transform.childCount;

            Camera.main.transform.position  = new Vector3(centerX, Camera.main.transform.position.y, centerZ);
        }

    }

}
