using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageButton : MonoBehaviour
{
    public int imageIndex; // This index for the image set when loading the images.
    public Transform content; // Parent content.

    public void Start()
    {
        // Set the parent here.
        content = this.transform.parent.GetComponent<Transform>();
    }

    public void SetImageIndex()
    {
        DataManager.ImageIndex = imageIndex;
        //print(imageIndex);
        HighlightMe(); // Highlight the selected image.
    }

    // Highlight the selected image
    private void HighlightMe()
    {
        // Search all images and clear out all red images with white.
        Transform go = content.GetComponentInChildren<Transform>();

        foreach(Transform gm in go)
        {
            // Reset all the highlighted colors.
            gm.GetComponent<Image>().color = Color.white;
        }

        // Set the new selected color.
        gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);

        
    }

}
