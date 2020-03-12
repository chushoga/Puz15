using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePeice : MonoBehaviour
{

    BoardManager bm;

    public int peiceIndex; // index of the peice starting from 1 to max size
    public bool correctPos = false; // Is this the correct final postion?

    // Movement variables.
    public bool moveToTarget = false;
    private RaycastHit hitPosition;
    private Vector3 mouseDelta; // The current mouse position - last mouse position.
    private Vector3 lastMouseCoordinate; // The last mouse position.
    private Vector3 mOffset; // The mouse offset.
    private float mZCoord; // The mouse z position used for mouse position calculations. 
    private float movementSpeed = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        bm = GameObject.Find("BoardManager").GetComponent<BoardManager>();
    }

    // When the mouse is held down on theis peice...
    private void OnMouseDown()
    {

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z; // Set the mouse z coord.
        mOffset = gameObject.transform.position - GetMouseWorldPos(); // Calculate the mouse offset.

        lastMouseCoordinate = Input.mousePosition;// Set the last position of mouse coordinate.

    }

    // When the mouse/finger is released...
    private void OnMouseUp()
    {
        mouseDelta = Input.mousePosition - lastMouseCoordinate; // update the mouse delta.

        Vector3 direction = mouseDelta.normalized; // Get a direction from the mouseDelta

        float dot = Vector3.Dot(direction, Vector3.up); // get a 0-1f vector 3 from the direction to decide which way.
        if (dot > 0.5)
        {
            CheckTargetPosition("UP"); // check if there is a hit up.            
        }
        else if (dot < -0.5)
        {
            CheckTargetPosition("DOWN"); // check if there is a hit down.            
        }
        else
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
            dot = Vector3.Dot(direction, Vector3.right);
            if (dot > 0.5)
            {
                CheckTargetPosition("RIGHT"); // check if there is a hit right.                
            }
            else if (dot < -0.5)
            {
                CheckTargetPosition("LEFT"); // check if there is a hit left.               
            }
        }

    }

    // Update the total moves in the ui
    void UpdateTotalMoves()
    {
        GameManager gm = GameObject.Find("GameManager").gameObject.GetComponent<GameManager>();
        gm.totalMoves += 1;
        gm.totalMovesTxt.GetComponentInChildren<TextMeshProUGUI>().text = gm.totalMoves + "";
    }

    // Get the mouse world position.
    private Vector3 GetMouseWorldPos()
    {
        // pixel cords
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    // Move To position.
    private void MoveToPosition(RaycastHit targetPos)
    {  
        // Move to the new position with a lerp.
        transform.position = Vector3.MoveTowards(transform.position, targetPos.transform.position, movementSpeed * Time.deltaTime);
                
        // If the position is about = to the target then stop moving.
        if (Vector3.Equals(transform.position,targetPos.transform.position) == true)
        {
            moveToTarget = false; // set movement flag to false to stop movement. This is checked in the update function.
            // check if the correct position for all peices
            bm.CheckPositions();
        }
        
    }

    // Raycast to target to check position
    private void CheckTargetPosition(string moveDirection)
    {
        // -------------------------------------------------------------------------------------------------------------
        // Raycast from 
        // -------------------------------------------------------------------------------------------------------------
        Vector3 raycastDirection = Vector3.up;
        
        switch (moveDirection)
        {
            case "UP":
                raycastDirection = Vector3.up;
                break;
            case "DOWN":
                raycastDirection = Vector3.down;
                break;
            case "LEFT":
                raycastDirection = Vector3.left;
                break;
            case "RIGHT":
                raycastDirection = Vector3.right;
                break;
            default:
                break;
        }

        if (Physics.Raycast(transform.position, raycastDirection, out hitPosition, 50))
        {
            if (hitPosition.transform.tag == "PeiceSpawn")
            {
                //GameManager.FindObjectOfType<GameManager>().totalMoves += 1; // increase the move counter
                moveToTarget = true; // Hit! Set move to target to true.
                UpdateTotalMoves();
            }
            else
            {
                moveToTarget = false; // prevent moving to target if not an empty spawn
            }
        }
        else
        {
            moveToTarget = false;   // No hit. Set move to target to false.
        }
        // -------------------------------------------------------------------------------------------------------------

    }

    void Update()
    {
        // If true move to target.
        if (moveToTarget)
        {
            MoveToPosition(hitPosition);
        }
    }
}
