using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardManager : MonoBehaviour
{
    // -----------------------------------------------------------------
    /* References */
    // -----------------------------------------------------------------
    
    // PUBLIC -- Puzzle peice variables
    public int puzzleSize; // How many tiles ie: 4x4.
    
    public GameObject puzzleProjectorCam; // Prefab of camera that projects the texture.
    public GameObject gamePeice; // the game peice
    public GameObject snapPoint; // snap point for prefab
    public GameObject backgroundImage; // backgrouind image
    public GameObject buttonOverlayTexture; // button overlay texture
    public GameObject lm;
    public GameObject im;

    // PRIVATE -- Variables
    private GameObject puzzleImage;
    private GameObject borderImage;
    private Shader unlit; // unlit texture for the peices
    private float cameraPadding = 1.10f; // How much padding for the camera
    private List<GameObject> spawnPoints = new List<GameObject>(); // need to initialize fields when they are private
    private Vector3 lastPeicePos = new Vector3(); // position of last peice
    private List<GameObject> gamePeices = new List<GameObject>(); // need to initialize fields when they are private
    private List<Vector3> origPos = new List<Vector3>();

    // HOUSEKEEPING
    private GameObject hk_SNAP; // Housekeeping parent for the peices
    private GameObject hk_CAMERA; // Housekeeping parent for the render cameras
    private GameObject hk_PEICES; // Housekeeping parent for the peices
    private GameObject hk_OVERLAY; // Housekeeping parent for the overlay textures

    private void Awake()
    {
        puzzleSize = DataManager.PuzzleSize;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Level manager reference
        lm = GameObject.Find("LevelManager");
        im = GameObject.Find("ImageManager");

        // Get reference to the puzzleImage
        puzzleImage = GameObject.Find("puzzleImage");

        // Get reference to the borderImage
        borderImage = GameObject.Find("borderImage");

        hk_SNAP = new GameObject("hk_SNAP");// Housekeeping parent for the peices
        hk_CAMERA = new GameObject("hk_CAMERA");// Housekeeping parent for the render cameras
        hk_PEICES = new GameObject("hk_PEICES"); // Housekeeping parent for the peices
        hk_OVERLAY = new GameObject("hk_OVERLAY"); // Housekeeping parent for the overlay textures

        // Set the image with the user choice.
        puzzleImage.GetComponentInChildren<SpriteRenderer>().sprite = im.GetComponent<ImageManager>().imageList[DataManager.ImageIndex];

        // Set the borderImage with the user choice
        // -3 is the offset for the images in the list
        borderImage.GetComponentInChildren<SpriteRenderer>().sprite = im.GetComponent<ImageManager>().numberOverlayList[(DataManager.PuzzleSize - 3)];

        //print(DataManager.ImageIndex);
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

        // Instantiate puzzleSize x puzzleSize count of puzleProjector Cameras.
        int counter = 1;
        for (int i = 0; i <= puzzleSize - 1; i++)
        {
            for (int j = 0; j <= puzzleSize - 1; j++)
            {
                // -----------------
                // CAMERA
                // -----------------
                // Create a new camera for the projected texture.
                GameObject newCam = Instantiate(puzzleProjectorCam, puzzleImage.transform.position, Quaternion.identity);

                // Set the orthographic camera size.
                newCam.GetComponent<Camera>().orthographicSize = newOrthoSize;

                // Move half a square up and over to start.
                newCam.transform.Translate((newOrthoScale / 2), (newOrthoScale / 2), -1f);

                // Calculate the new x and y position of the projector cameras.
                float newPosX = (newOrthoScale) * i;
                float newPosY = (newOrthoScale) * j;

                // Move the camera to the correct position.
                newCam.transform.Translate(newPosX, newPosY, 0f);

                // set parent of the newCam for housekeeping
                newCam.transform.SetParent(hk_CAMERA.transform);

                // -----------------
                // OVERLAY TEXTURE
                // -----------------
                Vector3 newOverlayPos = new Vector3(newCam.transform.position.x, newCam.transform.position.y, -0.1f);
                GameObject overlayTexture = Instantiate(buttonOverlayTexture, newOverlayPos, Quaternion.identity);
                float newSize = 1 / (float)puzzleSize;
                overlayTexture.transform.localScale = new Vector3(newSize, newSize, 1f);
                overlayTexture.SetActive(false);

                // add to housekeeping parent
                overlayTexture.transform.SetParent(hk_OVERLAY.transform);

                // -----------------
                // RENDER TEXTURE
                // -----------------
                // create a new render texture
                RenderTexture rt = new RenderTexture(256, 256, 24, RenderTextureFormat.ARGB32);
                rt.Create(); // create the texture.
                // set the target texture.
                newCam.GetComponent<Camera>().targetTexture = rt;
                // check if i need to create a new render texture for this.
                // match the puzzle peice to the render texture/camera pair

                // -----------------
                // GAME PEICE
                // -----------------
                // Instantiate the game peice.
                GameObject gm = Instantiate(gamePeice, new Vector3(i, j, 0f), gamePeice.transform.rotation);

                // Set the render texture created for it.
                gm.GetComponent<Renderer>().material.mainTexture = rt;

                // Set the id of the game peice here so we can do things with them.
                gm.GetComponent<GamePeice>().peiceIndex = counter;
                               
                // Find the unlit shader in the project and set it to the texture of the game peice.
                Shader shader1 = Shader.Find("Unlit/Texture");

                // Set the shader to the game peice.
                gm.GetComponent<Renderer>().material.shader = shader1;

                // Set the parent of the peices for housekeeping.
                gm.transform.SetParent(hk_PEICES.transform);

                // Add game peice to a list for shuffling and checking if game is finsished.
                gamePeices.Add(gm);

                // Add the original position to a list for checking final positions.
                origPos.Add(new Vector3(gm.transform.position.x, gm.transform.position.y, gm.transform.position.z));

                counter++;
            }
        }

        // Make an array of render Peices
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
  
        // Move the main camera position to the center of the puzzle.
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
       
        }
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
                switchPos = t.transform.position;
            }

            j++;
        }
       
         int z = 0;
         foreach (GameObject t in gamePeices)
         {
            
            // move the bottom right to the switch pos
            if (t.transform.position.x == puzzleSize - 1 && t.transform.position.y == 0f)
            {
                t.transform.position = new Vector3(switchPos.x, switchPos.y, switchPos.z);
            }

             // move empty peice to the bottom right.
             if(t.GetComponent<GamePeice>().peiceIndex == snapOffset)
            {   
                t.transform.position = new Vector3(lastPeicePos.x, lastPeicePos.y, lastPeicePos.z);
            }
             z++;
         }

        CheckPositions();
    }

    // Check if the peices are all in their correct spots
    // Check after each move.
    public void CheckPositions()
    {

        // boolean to use as reference when checking if the peice is in the correct position.
        bool isComplete = true;

        // update the GamePeice correctPos = true if the peice location matches the 
        for(int i = 0; i < gamePeices.Count; i++)
        {
            if (gamePeices[i].transform.position != origPos[i])
            {
                // show the overlay texture because it is not in the correct postion naymore.
                gamePeices[i].GetComponent<GamePeice>().correctPos = true;
                hk_OVERLAY.transform.GetChild(i).gameObject.SetActive(true);
                isComplete = false;
            } else
            {
                // Hide the overlay because it is in the correct postion now.
                gamePeices[i].GetComponent<GamePeice>().correctPos = false;
                hk_OVERLAY.transform.GetChild(i).gameObject.SetActive(false);
            }
           
        }

        // Check if the whole puzzle is complete and show the game clear screen with stats.4
        if (isComplete)
        {
            int snapOffset = (puzzleSize * puzzleSize) - (puzzleSize);
            gamePeices[snapOffset].SetActive(true);
            GameManager gm = GameObject.Find("GameManager").gameObject.GetComponent<GameManager>();
            gm.totalMovesTxt.GetComponentInChildren<TextMeshProUGUI>().text = "CLEAR!! "; // + gm.totalMoves;
        }
        
    }

    // Fade out the start button.
    public void FadeButton()
    {
        GameObject startButton = GameObject.Find("startButton").gameObject;
        Animator animator = startButton.GetComponent<Animator>(); // Animator for the fade screen
        animator.SetTrigger("FadeOut"); // trigger the fade out animation
    }

}
