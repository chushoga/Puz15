using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestGamePeice : MonoBehaviour
{
    private bool moveToTarget = false;
    private RaycastHit hitPosition;

    private Vector3 mouseDelta;
    private Vector3 lastMouseCoordinate;
    private Vector3 mOffset;
    private float mZCoord;

    private AudioClip myMoveSound;

    private GameObject console;

    private void Awake()
    {
        myMoveSound = GameObject.Find("soundManager").GetComponent<testSoundManager>().moveSound;
        gameObject.GetComponent<AudioSource>().clip = myMoveSound;
    }

    // Start is called before the first frame update
    void Start()
    {
        console = GameObject.Find("InfoBox");        
    }

    private void OnMouseDown()
    {
        gameObject.GetComponent<AudioSource>().Play();
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
            //print(console.GetComponentInChildren<TextMeshProUGUI>().text = "UP");
        }
        else if (dot < -0.5)
        {
            CheckTargetPosition("DOWN"); // check if there is a hit down.            
            //print(console.GetComponentInChildren<TextMeshProUGUI>().text = "DOWN");
        }
        else
        {
            dot = Vector3.Dot(direction, Vector3.right);
            if (dot > 0.5)
            {
                CheckTargetPosition("RIGHT"); // check if there is a hit right.            
                //print(console.GetComponentInChildren<TextMeshProUGUI>().text = "RIGHT");
            }
            else if (dot < -0.5)
            {
                CheckTargetPosition("LEFT"); // check if there is a hit left.            
                //print(console.GetComponentInChildren<TextMeshProUGUI>().text = "LEFT");
            }
        }
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
        transform.position = Vector3.Lerp(transform.position, targetPos.transform.position, 10.0f * Time.deltaTime);
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
            } else
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
