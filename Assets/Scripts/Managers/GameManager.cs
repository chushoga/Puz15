using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{

    // -----------------------------------------------------------------
    /* References */
    // -----------------------------------------------------------------

    private LevelManager lm; // Level manager reference.
    private BoardManager bm; // Board Manager - does all the board/peice spawning
    public int totalMoves;
    public GameObject totalMovesTxt;
    public AudioClip moveSound;
    public bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;

        // Find and set the LevelManager object and set it to the reference.
        lm = GameObject.Find("LevelManager").gameObject.GetComponent<LevelManager>();
         
    }

    public void SetGameStarted()
    {
        gameStarted = true;
    }
    
}
