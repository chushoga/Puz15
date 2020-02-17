using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGamePeice : MonoBehaviour
{
    private Vector3 mouseDelta;
    private Vector3 lastMouseCoordinate;
    private Vector3 mOffset;
    private float origX; // original x position [ right, left ]
    private float origZ; // original z position [ up, down ]
    private float mZCoord;
    private bool XorZ; // which direction to move

    public GameObject infoBox;

    // Start is called before the first frame update
    void Start()
    {
        
        //print(infoBox.GetComponent<Text>().text);
        
    }

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();

        lastMouseCoordinate = Input.mousePosition;

        //print(gameObject.transform.position - GetMouseWorldPos());
    }

    private void OnMouseUp()
    {
        
    }

    private void OnMouseDrag()
    {
        // transform.position = GetMouseWorldPos() + mOffset;
        // update to a new original mouse postion to check for up down left right
        /*
        origX = Mathf.Abs(GetMouseWorldPos().x);
        origZ = Mathf.Abs(GetMouseWorldPos().z);

        if (origX < origZ)
        {
           
            //transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            print("Up and Down" + origX + "<" + origZ);
        }

        if(origX > origZ)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
            print("Left and Right" + origX + ">" + origZ);
        }
        */
        //will get a 0/0 error if mouse does not move
        mouseDelta = Input.mousePosition - lastMouseCoordinate;
        

        Vector3 direction = mouseDelta.normalized;

        float dot = Vector3.Dot(direction, Vector3.up);
        if (dot > 0.5)
        { //can be >= for sideways
            print("UP");
            transform.position = GetMouseWorldPos() + mOffset;
        }
        else if (dot < -0.5)
        { //can be <= for sideways
            print("DOWN");
        }
        else
        {
            dot = Vector3.Dot(direction, Vector3.right);
            if (dot > 0.5)
            { //can be >= for sideways
                print("RIGHT");
            }
            else if (dot < -0.5)
            { //can be <= for sideways
                print("LEFT");
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

    private void Update()
    {
        

    }

}
