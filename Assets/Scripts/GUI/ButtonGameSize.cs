using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonGameSize : MonoBehaviour
{
    // PUBLIC
    public int boardSize = 3;

    // PRIVATE
    private TextMesh txt;

    public void SetPuzzleSize()
    {
        DataManager.PuzzleSize = boardSize;
        print(boardSize);
    }

    private void Start()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = boardSize + "x" + boardSize;
    }

}
