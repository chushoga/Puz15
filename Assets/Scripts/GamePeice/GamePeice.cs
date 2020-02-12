using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePeice : MonoBehaviour
{
    public int location; // final position

    private Vector3 mOffset;
    private float mZCoord;
    private Vector3 origMousePos;
    private Vector3 origPos;

    void OnMouseDown()
    {
        origMousePos = GetMouseWorldPos();
        origPos = gameObject.transform.position;
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store the offset. gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseWorldPos();
        
    }

    private Vector3 GetMouseWorldPos()
    {
        // pixel cords
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    private void OnMouseDrag()
    {
        
    }

    private void OnMouseUp()
    {
        if (GetMouseWorldPos().z >= origMousePos.z)
        {
            // Move up
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.25f);
            
        }
        else if(GetMouseWorldPos().z <= origMousePos.z)
        {
            // Move down
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1.25f);
        } else if(GetMouseWorldPos().y <= origMousePos.y)
        {
            // Move right
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z);
        } else if(GetMouseWorldPos().y >= origMousePos.y)
        {
            // Move left
            transform.position = new Vector3(transform.position.x, transform.position.y - 1.25f, transform.position.z);
        }

        /*
        print(GetMouseWorldPos());
        print(mOffset);
        print("----");
        */

        //transform.position = GetMouseWorldPos() + mOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        //gameObject.transform.position = origPos;
    }

}
