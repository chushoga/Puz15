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
    public GameObject snapPoint; // snap point for prefab
    public GameObject backgroundImage; // backgrouind image

    // PRIVATE -- Variables
    private Shader unlit; // unlit texture for the peices
    private float cameraPadding = 1.15f; // How much padding for the camera
    private List<GameObject> spawnPoints = new List<GameObject>(); // need to initialize fields when they are private
    private List<GameObject> gamePeices = new List<GameObject>(); // need to initialize fields when they are private

    // Start is called before the first frame update
    void Start()
    {
        CreateBoard();
        LoadPeices();
        UpdateCamera();
        //AddBackground();
    }

    // Update the camera scale and center on puzzle
    void UpdateCamera()
    {
        Camera.main.orthographicSize = puzzleSize * cameraPadding;
    }

    // Add the background
    void AddBackground()
    {
        GameObject bg = Instantiate(backgroundImage, new Vector3(0f, 0f, backgroundImage.transform.position.z), Quaternion.identity);
    }

    // Create the play board
    void CreateBoard()
    {
        GameObject hk_SNAP = new GameObject("hk_SNAP");// Housekeeping parent for the peices

        int counter = 1;
        // Create peices from the loop
        for (int i = 0; i <= puzzleSize - 1; i++)
        {
            for (int j = 0; j <= puzzleSize - 1; j++)
            {
                // create a new snap point.
                GameObject sp = Instantiate(snapPoint, new Vector3(i, j, 0f), snapPoint.transform.rotation);
                sp.transform.SetParent(hk_SNAP.transform); // set to a parent for housekeeping
                sp.GetComponent<SpawnPoint>().spawnIndex = counter;
                spawnPoints.Add(sp);
                counter++;
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
        int counter = 1;
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

                // Set the id of the game peice here so we can do things with them.
                gm.GetComponent<GamePeice>().peiceIndex = counter;
                               
                Shader shader1 = Shader.Find("Unlit/Texture");
                gm.GetComponent<Renderer>().material.shader = shader1;

                // position the camera positon in order of width and height of the puzzle.
                newCam.transform.Translate((newOrthoScale / 2), (newOrthoScale / 2), -1f); // move half a square up and over to start

                float newPosX = (newOrthoScale) * i;
                float newPosY = (newOrthoScale) * j;
                newCam.transform.Translate(newPosX, newPosY, 0f); // move to correct position

                newCam.transform.SetParent(hk_CAMERA.transform); // set parent of the newCam for housekeeping
                gm.transform.SetParent(hk_PEICES.transform); // set the parent of the peices for housekeeping

                gamePeices.Add(gm); // add game peice to a list for shuffling and checking if game is finsished

                counter++;
            }
        }
        Renderer[] rends = hk_PEICES.GetComponentsInChildren<Renderer>();
        
        float x = 0f;
        float y = 0f;
        foreach (Renderer rend in rends)
        {
            x += rend.bounds.center.x;
            y += rend.bounds.center.y;           
        }

        x = x / (puzzleSize * puzzleSize);
        y = y / (puzzleSize * puzzleSize);
        
        // hide last bottom right peice if last peice.
        foreach(Transform child in hk_PEICES.transform)
        {
            if(child.GetComponent<GamePeice>().peiceIndex == ((puzzleSize * puzzleSize) - (puzzleSize - 1)))
            {
                child.gameObject.SetActive(false);
            }
        }
  
        Camera.main.transform.position = new Vector3(x, y, Camera.main.transform.position.z);

    }


    // Shuffle the board up
    public void ShuffleBoard()
    {
        int snapOffset = (puzzleSize * puzzleSize) - (puzzleSize - 1);

        // Randomize the spawnpoints and put into a temp snap point location
        List<GameObject> tempGM = spawnPoints;
        
        for (int i = 0; i < tempGM.Count; i++)
        {
            GameObject temp = tempGM[i];
            int randomIndex = Random.Range(i, tempGM.Count);
            tempGM[i] = tempGM[randomIndex];
            tempGM[randomIndex] = temp;
        }

        // search through and find the temp position for the item that will
        // replace the last peice
        Vector3 tempPos = new Vector3();
        // go thre new temp array and find the spawn point that is in the snap offset position
        foreach(GameObject gm in tempGM)
        {
            if(gm.GetComponent<SpawnPoint>().spawnIndex == snapOffset)
            {
                tempPos = gm.transform.position;
            }
        }

        // Move the game peice to the new postion        
        int j = 0;
        foreach (GameObject t in gamePeices)
        {
            
            if(t.GetComponent<GamePeice>().peiceIndex == snapOffset)
            {
                print(t.GetComponent<GamePeice>().peiceIndex);
                print(snapOffset);
                // dont move the peice
            } else
            {
                // move the peice and if index of the snap point is the snap offset then move there
                if(tempGM[j].GetComponent<SpawnPoint>().spawnIndex == snapOffset)
                {
                    print("MOVE TO TEMP" + tempPos);
                    // move the the 
                    t.transform.position = new Vector3(tempPos.x, tempPos.y , tempPos.z);
                } else
                {
                    t.transform.position = new Vector3(tempGM[j].transform.position.x, tempGM[j].transform.position.y, tempGM[j].transform.position.z);
                }

            }
            
            j++;
        }

    }

    // Check if the peices are all in their correct spots
    // Check after each move.
    public void CheckPositions()
    {
        // update the GamePeice correctPos = true if the peice location matches the 
    }

}
