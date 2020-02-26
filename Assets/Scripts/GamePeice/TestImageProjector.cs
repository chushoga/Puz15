using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestImageProjector : MonoBehaviour
{

    public int puzzleSize; // How many tiles ie: 4x4.
    public GameObject puzzleImage; // The image to project.
    public GameObject puzzleProjectorCam; // Prefab of camera the projects the texture.
    public GameObject testPeice;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the projector image
        // Instantiate the prefab ortho projector cameras at the correct position and count for the size of the puzzle

        // *********************************************************************
        // TEST START
        // *********************************************************************
        
        float newOrthoScale = 1.0f / (float)puzzleSize; // set the ortho scale
        float newOrthoSize =  newOrthoScale * 0.5f; // get the new ortho camera size based off the scale / puzzle width and height

        // Instantiate puzzleSize x puzzleSize count of puzleProjector Cameras.
        for(int i = 0; i <= puzzleSize - 1; i++)
        {
            for(int j = 0; j <= puzzleSize - 1; j++)
            {
                GameObject newCam = Instantiate(puzzleProjectorCam, puzzleImage.transform.position, Quaternion.identity);
                newCam.GetComponent<Camera>().orthographicSize = newOrthoSize; // Set the orthographic camera size
                
                // create a new render texture
                RenderTexture rt = new RenderTexture(256, 256, 24, RenderTextureFormat.ARGB32);
                rt.Create(); // create the texture.
                // set the target texture.
                newCam.GetComponent<Camera>().targetTexture = rt;
                // check if i need to create a new render texture for this.
                // match the puzzle peice to the render texture/camera pair
                // create a new test board from peices
                GameObject gm = Instantiate(testPeice, new Vector3(i, 0f, j), Quaternion.identity);
                gm.GetComponent<Renderer>().material.mainTexture = rt;
                // position the camera positon in order of width and height of the puzzle.
                newCam.transform.Translate((newOrthoScale / 2), (newOrthoScale / 2), -1f); // move half a square up and over to start

                float newPosX = (newOrthoScale) * i;
                float newPosY = (newOrthoScale) * j;
                newCam.transform.Translate(newPosX, newPosY, 0f); // move to correct position
                
            }
        }
        



        
        // *********************************************************************
        // TEST FINISH
        // *********************************************************************
    }



}
