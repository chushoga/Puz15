using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePeice : MonoBehaviour
{
    public int location;

    private bool moveToTarget = false;
    private RaycastHit hitPosition;

    private Vector3 mouseDelta;
    private Vector3 lastMouseCoordinate;
    private Vector3 mOffset;
    private float mZCoord;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnMouseDown()
    {

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();

        lastMouseCoordinate = Input.mousePosition;

    }

    private void OnMouseUp()
    {
        mouseDelta = Input.mousePosition - lastMouseCoordinate;

        Vector3 direction = mouseDelta.normalized;

        float dot = Vector3.Dot(direction, Vector3.up);
        if (dot > 0.5)
        {
            CheckTargetPosition("UP"); // check if there is a hit up.
            UpdateTotalMoves();
        }
        else if (dot < -0.5)
        {
            CheckTargetPosition("DOWN"); // check if there is a hit down.
            UpdateTotalMoves();
        }
        else
        {
            dot = Vector3.Dot(direction, Vector3.right);
            if (dot > 0.5)
            {
                CheckTargetPosition("RIGHT"); // check if there is a hit right.
                UpdateTotalMoves();
            }
            else if (dot < -0.5)
            {
                CheckTargetPosition("LEFT"); // check if there is a hit left.
                UpdateTotalMoves();
            }
        }

    }

    // Update the total moves in the ui
    void UpdateTotalMoves()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "PeiceSpawn")
        {
            // print("TEST");
        }
    }

    // get the mouse world position
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
        transform.position = Vector3.Lerp(transform.position, targetPos.transform.position, 15.0f * Time.deltaTime);
        
        
        if (Vector3.Equals(transform.position,targetPos.transform.position) == true)
        {
            moveToTarget = false;
        }
        
        GameManager.FindObjectOfType<GameManager>().totalMoves += 1;
    }

    // Raycast to target
    private void CheckTargetPosition(string moveDirection)
    {
        // -------------------------------------------------------------------------------------------------------------
        // Raycast from 
        // -------------------------------------------------------------------------------------------------------------
        Vector3 raycastDirection = Vector3.forward;
        
        switch (moveDirection)
        {
            case "UP":
                raycastDirection = Vector3.forward;
                break;
            case "DOWN":
                raycastDirection = Vector3.back;
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
                moveToTarget = true; // Hit! Set move to target to true.
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
        if (moveToTarget)
        {
            MoveToPosition(hitPosition);
        }
    }
}
