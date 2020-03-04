using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    // -----------------------------------------------------------------
    /* References */
    // -----------------------------------------------------------------

    private LevelManager lm; // Level manager reference.
    private BoardManager bm; // Board Manager - does all the board/peice spawning
    public int totalMoves;
    public GameObject totalMovesTxt;

    // Start is called before the first frame update
    void Start()
    {
        // Find and set the LevelManager object and set it to the reference.
        lm = GameObject.Find("LevelManager").gameObject.GetComponent<LevelManager>();
         
    }
    
}
