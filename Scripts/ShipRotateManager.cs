using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotateManager : MonoBehaviour
{
    private bool isRotatable = false;
    private Vector2 mousePos;

    [SerializeField]
    private GameObject go;

    [SerializeField]
    private String objectName; 

    public static bool hasRotate = false;

    private void Start()
    {
       objectName = this.name;
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(1)) // 마우스를 우클릭 했을 때
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(mousePos, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction); // 마우스 포지션에 ray를 쏘고 

            if(hit.collider != null)  // 거기에 맞은 오브젝트의 tag가 ship이었을 경우
            {
                if (hit.collider.CompareTag("Ship") && isRotatable == false) // 회전 가능한 상태로 바꾼다.
                {
                    go = hit.collider.gameObject;                   
                    isRotatable = true;
                }

                else if (hit.collider.tag == "Ship" && isRotatable == true) // 만약 우클릭을 또 했으면 회전 가능한 상태를 비활성화 시킨다.
                {
                    isRotatable = false;
                    if(hasRotate) hasRotate = false;
                    go = null;
                }                
            }

            else
            {
                if(isRotatable == true)
                {
                    isRotatable = false;
                    if(hasRotate) hasRotate = false;
                    go = null;
                }
            }
        }       

    }

    private void OnTriggerEnter2D(Collider2D collision) // 만약 회전 도중 충돌한다면 다시 각도를 원위치 시킨다.
    {
        if (hasRotate) return;

        if (go != null && go.name == objectName)
        {            
            RotateShip();
            hasRotate = true;
        }
    }

    public void RotateShip()
    {
        float zAngle = go.transform.rotation.eulerAngles.z;

        if (Mathf.Approximately(zAngle, 0f))
        {
            go.transform.Rotate(0, 0, 90);
        }
        else
        {
            go.transform.Rotate(0, 0, -90);
        }
    }

}
