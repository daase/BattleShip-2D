using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RotateBtn : MonoBehaviour
{
    private GameObject rotateBtn;
    private bool isRotatable = false;
    private Vector2 mousePos;
    private GameObject ship;
    private float zAngle = 0;

    // Start is called before the first frame update
    void Start()
    {       
        rotateBtn = GameObject.Find("Canvas/RotationButton");
        rotateBtn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(mousePos, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if(hit.collider != null)
            {
                if (hit.collider.tag == "Ship" && isRotatable == false)
                {
                    rotateBtn.SetActive(true);
                    isRotatable = true;
                    ship = hit.collider.gameObject;

                }

                else if (hit.collider.tag == "Ship" && isRotatable == true)
                {
                    rotateBtn.SetActive(false);
                    isRotatable = false;
                    ship = null;
                }              
            }

            else
            {
                if(isRotatable == true)
                {
                    rotateBtn.SetActive(false);
                    isRotatable = false;
                    ship = null;
                }
            }
            
        }

        if(isRotatable == true) 
        { 
            rotateBtn.transform.position = new Vector3(ship.transform.position.x, ship.transform.position.y + 0.4f, 0); 
        }      
    }

    public void RotateShip()
    {       
        zAngle = ship.transform.rotation.eulerAngles.z;
        Rotate(zAngle);      
    }

    public void Rotate(float angle)
    {
        if(ShipRotateManager.hasRotate) ShipRotateManager.hasRotate = false;

        if (Mathf.Approximately(angle, 0f))
        {
            ship.transform.Rotate(0, 0, 90);
        }

        else
        {
            ship.transform.Rotate(0, 0, -90);
        }       
    }
}
