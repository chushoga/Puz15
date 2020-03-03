using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    // -----------------------------------------------------------------
    /* References */
    // -----------------------------------------------------------------
    
    // PUBLIC -- Puzzle peice variables
    public int puzzleSize; // How many tiles ie: 4x4.
    public GameObject puzzleImage; // The image to project.
    public GameObject puzzleProjectorCam; // Prefab of camera that projects the texture.
    public GameObject gamePeice; // the game peice
    public GameObject snapPoint;// snap point for prefab

    // PRIVATE -- Variables
    private Shader unlit; // unlit texture for the peices

    // Start is called before the first frame update
    void Start()
    {
        CreateBoard();
        LoadPeices();
        UpdateCamera();
    }

    // Update the camera scale and center on puzzle
    void UpdateCamera()
    {
        Camera.main.orthographicSize = puzzleSize;
        
    }

    // Create the play board
    void CreateBoard()
    {
        GameObject hk_SNAP = new GameObject("hk_SNAP");// Housekeeping parent for the peices

        // Create peices from the loop
        for (int i = 0; i <= puzzleSize - 1; i++)
        {
            for (int j = 0; j <= puzzleSize - 1; j++)
            {
                // create a new snap point.
                GameObject sp = Instantiate(snapPoint, new Vector3(i, j, 0f), snapPoint.transform.rotation);
                sp.transform.SetParent(hk_SNAP.transform); // set to a parent for housekeeping

            }
        }
    }

    // LoadPeices
    void LoadPeices()
    {
        // Instantiate the projector image
        // Instantiate the prefab ortho projector cameras at the correct position and count for the size of the puzzle

        // *********************************************************************
        // Load the image and create peices
        // *********************************************************************

        float newOrthoScale = 1.0f / (float)puzzleSize; // set the ortho scale
        float newOrthoSize = newOrthoScale * 0.5f; // get the new ortho camera size based off the scale / puzzle width and height
        GameObject hk_CAMERA = new GameObject("hk_CAMERA");// Housekeeping parent for the render cameras
        GameObject hk_PEICES = new GameObject("hk_PEICES"); // Housekeeping parent for the peices

        // Instantiate puzzleSize x puzzleSize count of puzleProjector Cameras.
        for (int i = 0; i <= puzzleSize - 1; i++)
        {
            for (int j = 0; j <= puzzleSize - 1; j++)
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
                GameObject gm = Instantiate(gamePeice, new Vector3(i, j, 0f), gamePeice.transform.rotation);
                gm.GetComponent<Renderer>().material.mainTexture = rt;

                // hide last bottom right peice if last peice.
                if ((i == puzzleSize - 1) && (j == 0))
                {
                    gm.SetActive(false);
                }

                Shader shader1 = Shader.Find("Unlit/Texture");
                gm.GetComponent<Renderer>().material.shader = shader1;

                // position the camera positon in order of width and height of the puzzle.
                newCam.transform.Translate((newOrthoScale / 2), (newOrthoScale / 2), -1f); // move half a square up and over to start

                float newPosX = (newOrthoScale) * i;
                float newPosY = (newOrthoScale) * j;
                newCam.transform.Translate(newPosX, newPosY, 0f); // move to correct position

                newCam.transform.SetParent(hk_CAMERA.transform); // set parent of the newCam for housekeeping
                gm.transform.SetParent(hk_PEICES.transform); // set the parent of the peices for housekeeping
            }
        }
        Renderer[] rends = hk_PEICES.GetComponentsInChildren<Renderer>();
        Bounds bounds;
        float x = 0f;
        float y = 0f;
        foreach (Renderer rend in rends)
        {
            x += rend.bounds.center.x;
            y += rend.bounds.center.y;           
        }

        x = x / (puzzleSize * puzzleSize);
        y = y / (puzzleSize * puzzleSize);
        print(x + "," + y);
        Camera.main.transform.position = new Vector3(x, y, Camera.main.transform.position.z);
        // Vector3 center = bounds.center;
        //print(bounds.center + "," + bounds.size);
    }


    // Shuffle the board up
    public void ShuffleBoard()
    {

        // Shuffle the peices
        /*
        List<GameObject> tempGM = spawnPoints;
        for (int i = 0; i < tempGM.Count; i++)
        {
            GameObject temp = tempGM[i];
            int randomIndex = Random.Range(i, tempGM.Count);
            tempGM[i] = tempGM[randomIndex];
            tempGM[randomIndex] = temp;
        }


        // Move the game peice to the new postion
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
        */

    }

}
