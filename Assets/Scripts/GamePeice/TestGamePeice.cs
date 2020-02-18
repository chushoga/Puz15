using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestGamePeice : MonoBehaviour
{
    private bool moveToTarget = false;
    private Vector3 mouseDelta;
    private Vector3 lastMouseCoordinate;
    private Vector3 mOffset;
    private float origX; // original x position [ right, left ]
    private float origZ; // original z position [ up, down ]
    private float mZCoord;
    private bool XorZ; // which direction to move

    private GameObject console;

    // Start is called before the first frame update
    void Start()
    {
        console = GameObject.Find("InfoBox");
        
    }

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();

        lastMouseCoordinate = Input.mousePosition;
    }

    private void OnMouseUp()
    {

        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50f, Color.red, 5.0f);

        // Raycast from 
        RaycastHit hitPosition;
        if (Physics.Raycast(transform.position, Vector3.forward, out hitPosition, 50))
        {
            print("HIT");
            print(hitPosition.transform.position);
            //transform.position = hitPosition.transform.position;
            transform.position = Vector3.Lerp(transform.position, hitPosition.transform.position, 2.0f * Time.deltaTime);
        }
        
    }

    private void OnMouseDrag()
    {
        
       
        mouseDelta = Input.mousePosition - lastMouseCoordinate;
        

        Vector3 direction = mouseDelta.normalized;

        float dot = Vector3.Dot(direction, Vector3.up);
        if (dot > 0.5)
        { //can be >= for sideways
            
            print((GetMouseWorldPos() + mOffset).x);
            print(console.GetComponentInChildren<TextMeshProUGUI>().text = "UP");
            //transform.position += new Vector3((GetMouseWorldPos() + mOffset).x, transform.position.y, transform.position.z);
        }
        else if (dot < -0.5)
        { //can be <= for sideways
            print(console.GetComponentInChildren<TextMeshProUGUI>().text = "DOWN");
            //transform.position = GetMouseWorldPos() + mOffset;
        }
        else
        {
            dot = Vector3.Dot(direction, Vector3.right);
            if (dot > 0.5)
            { //can be >= for sideways
                print(console.GetComponentInChildren<TextMeshProUGUI>().text = "RIGHT");
            }
            else if (dot < -0.5)
            { //can be <= for sideways
                print(console.GetComponentInChildren<TextMeshProUGUI>().text = "LEFT");
            }
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

    private void OnTriggerEnter(Collider col)
    {

        print("TEST");

        if (col.gameObject.tag == "PeiceSpawn")
        {
            print("TEST");
        }
    }

    void Update()
    {

       
    }

}
