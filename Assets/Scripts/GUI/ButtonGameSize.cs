using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonGameSize : MonoBehaviour
{
    // PUBLIC
    public int boardSize = 3;
    public Transform content; // Parent content.

    // PRIVATE
    private TextMesh txt;

    public void SetPuzzleSize()
    {
        DataManager.PuzzleSize = boardSize;
        HighlightMe();
    }

    private void Start()
    {
        // Set the parent here.
        content = this.transform.parent.GetComponent<Transform>();
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = boardSize + "x" + boardSize;
    }

    // Highlight the selected image
    private void HighlightMe()
    {
        // Search all images and clear out all red images with white.
        Transform go = content.GetComponentInChildren<Transform>();

        foreach (Transform gm in go)
        {
            // Reset all the highlighted colors.
            gm.GetComponent<Image>().color = Color.white;
        }

        // Set the new selected color.
        gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);


    }
}
