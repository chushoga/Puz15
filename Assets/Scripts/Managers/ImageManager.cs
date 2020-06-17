using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public List<Sprite> imageList; // holds a list of all the images to be used for the puzzles
    public List<Sprite> numberOverlayList; // holds a list of all number overlay sizes for puzzle
    public GameObject btnPuzzleImage;

    private GameObject guiCont;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            guiCont = GameObject.Find("Content");
            // Load up the images from the image List into the scrolling content.
            //print("load up the images");
            LoadImages();
        }
    }

    // Load the images from the List
    private void LoadImages()
    {
        int i = 0;
        foreach(Sprite sp in imageList)
        {
            // load em here.
            GameObject gm = Instantiate(btnPuzzleImage, guiCont.transform.position, Quaternion.identity);

            // Parent to the content
            gm.transform.SetParent(guiCont.transform);

            // Set the proper image to the index.
            gm.GetComponent<Image>().sprite = imageList[i];

            // Set the correct index here
            gm.GetComponent<ImageButton>().imageIndex = i;

            // Increment the index.
            i++;
        }
    }

}
