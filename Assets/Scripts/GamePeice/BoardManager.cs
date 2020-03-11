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
    private float cameraPadding = 1.10f; // How much padding for the camera
    public List<GameObject> spawnPoints = new List<GameObject>(); // need to initialize fields when they are private
    private Vector3 lastPeicePos = new Vector3(); // position of last peice
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
                sp.name = sp.GetComponent<SpawnPoint>().spawnIndex + "_spawnPoint";
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
                lastPeicePos = child.transform.position;
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
        List<GameObject> tempGM = new List<GameObject>(spawnPoints);

        List<Vector3> tempGMpos = new List<Vector3>();
        List<int> tempGMindex = new List<int>();


        // create the new lists so they dont modifiy the origional list
        for (int i = 0; i < spawnPoints.Count; i++)
        {   
            tempGMpos.Add(spawnPoints[i].transform.position);
            tempGMindex.Add(spawnPoints[i].GetComponent<SpawnPoint>().spawnIndex);
        }
        
        // --------------------------------------------------------------------------------------------
        // RESET POSTION FIRST FOR ALL PEICES
        // Move the game peice to the new postion        
        int a = 0;
        foreach (GameObject t in gamePeices)
        {

            if (t.GetComponent<GamePeice>().peiceIndex == tempGM[a].GetComponent<SpawnPoint>().spawnIndex)
            {
                // move to 
                t.transform.position = new Vector3(tempGM[a].transform.position.x, tempGM[a].transform.position.y, tempGM[a].transform.position.z);

            }

            a++;
        }

// --------------------------------------------------------------------------------------------
        // SHUFFLE
        
        for (int i = 0; i < tempGM.Count; i++)
        {
            Vector3 tempPos = tempGMpos[i];
            int tempIndex = tempGMindex[i];

            int randomIndex = Random.Range(i, tempGM.Count);

            tempGMpos[i] = tempGMpos[randomIndex];
            tempGMpos[randomIndex] = tempPos;

            tempGMindex[i] = tempGMindex[randomIndex];
            tempGMindex[randomIndex] = tempIndex;
            /*
            GameObject temp = tempGM[i];
            temp.name = i + "_tempSnap";
            int randomIndex = Random.Range(i, tempGM.Count);
            tempGM[i] = tempGM[randomIndex];
            tempGM[randomIndex] = temp;
            */
            
        }


        // --------------------------------------------------------------------------------------------



        // --------------------------------------------------------------------------------------------
        // Move the game peices to the new random postion        
        Vector3 switchPos = new Vector3();
        int j = 0;
        foreach (GameObject t in gamePeices)
        {
            // move to the random position created in the tempGM
            t.transform.position = new Vector3(
                tempGMpos[j].x,
                tempGMpos[j].y,
                tempGMpos[j].z
            );

            if(j == (snapOffset-1))
            {
               // t.transform.position = new Vector3(t.transform.position.x, t.transform.position.y, -2.0f);
                switchPos = t.transform.position;
                
            }

            j++;
        }

        //gamePeices[snapOffset].transform.position = new Vector3(switchPeicePos.x, switchPeicePos.y, switchPeicePos.z + 1.0f);
       
         int z = 0;
         foreach (GameObject t in gamePeices)
         {
             if (t.GetComponent<GamePeice>().peiceIndex == snapOffset)
             {
                print(gamePeices[z].transform.position);
                 //t.transform.position = new Vector3(switchPeicePos.x, switchPeicePos.y, switchPeicePos.z - 2f);
                //print(t.GetComponent<GamePeice>().peiceIndex);
                //switchPos = t.GetComponent<GamePeice>().transform.position;
                //t.transform.position = switchPos;
                //t.transform.position = lastPeicePos;
            }
            // move the bottom right to the switch pos
            
            if (t.transform.position.x == puzzleSize - 1 && t.transform.position.y == 0f)
            {
                print(t.transform.position);
                t.transform.position = new Vector3(switchPos.x, switchPos.y, switchPos.z);
            }

             // move empty peice to the bottom right.
             if(t.GetComponent<GamePeice>().peiceIndex == snapOffset)
            {   
                t.transform.position = new Vector3(lastPeicePos.x, lastPeicePos.y, lastPeicePos.z);
            }
             z++;
         }

         // go through the spawn grid and 

        /*

        // Trade the switchPeice with the last peice   
        foreach (GameObject t in gamePeices)
        {
            // 1. Move last peice to last peice position
            if (t.GetComponent<GamePeice>().peiceIndex == snapOffset)
            {
                t.transform.position = lastPeicePos;
            }
        }
        */

        /*
         * if (t.GetComponent<GamePeice>().peiceIndex == switchPeicePos)
        {
            t.transform.position = tempGM[snapOffset].transform.position;
        }
        */


        // 2. Move the switch places peice with the switchPeicePos
        /*
        if (t.GetComponent<GamePeice>().peiceIndex == switchPeicePos)
        {
            t.transform.position = tempGM[z].transform.position;
        }
        */
        /*
        t.transform.position = new Vector3(
            tempGM[z].transform.position.x,
            tempGM[z].transform.position.y,
            tempGM[z].transform.position.z
        );
        */


    }

    // Check if the peices are all in their correct spots
    // Check after each move.
    public void CheckPositions()
    {
        // update the GamePeice correctPos = true if the peice location matches the 
    }

}
