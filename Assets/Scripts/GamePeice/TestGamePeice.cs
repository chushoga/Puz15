using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGamePeice : MonoBehaviour
{

    private float origX; // original x position [ right, left ]
    private float origZ; // original z position [ up, down ]
    private float mZCoord;
    private bool XorZ; // which direction to move

    // Start is called before the first frame update
    void Start()
    {
        // set the original x an z position of this gameobject
        origX = gameObject.transform.position.x;
        origZ = gameObject.transform.position.y;
    }

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        origX = gameObject.transform.position.x - GetMouseWorldPos().x;
        origZ = gameObject.transform.position.z - GetMouseWorldPos().z;

        print(gameObject.transform.position - GetMouseWorldPos());
    }

    private void OnMouseUp()
    {

       
        print(gameObject.transform.position - GetMouseWorldPos());
    }

    private void OnMouseDrag()
    {
        gameObject.transform.Translate(GetMouseWorldPos());
        if (origX > origZ)
        {
           
            //transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            print("test");
        }
        else
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
            print("hmm");
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

}
