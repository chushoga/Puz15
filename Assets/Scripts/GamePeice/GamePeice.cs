using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePeice : MonoBehaviour
{
    public int location; // final position

    private Vector3 mOffset;
    private float mZCoord;
    private Vector3 origMousePos;

    void OnMouseDown()
    {
        origMousePos = GetMouseWorldPos();

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
        if(GetMouseWorldPos().z >= origMousePos.z)
        {
            print("Up");
        } else
        {
            print("Down");
        }

        transform.position = Vector3.up;
        /*
        print(GetMouseWorldPos());
        print(mOffset);
        print("----");
        */

        //transform.position = GetMouseWorldPos() + mOffset;
    }

    private void OnMouseUp()
    {
        
    }

}
