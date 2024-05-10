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
        
        if (Input.GetMouseButtonDown(1)) // ���콺�� ��Ŭ�� ���� ��
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(mousePos, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction); // ���콺 �����ǿ� ray�� ��� 

            if(hit.collider != null)  // �ű⿡ ���� ������Ʈ�� tag�� ship�̾��� ���
            {
                if (hit.collider.CompareTag("Ship") && isRotatable == false) // ȸ�� ������ ���·� �ٲ۴�.
                {
                    go = hit.collider.gameObject;                   
                    isRotatable = true;
                }

                else if (hit.collider.tag == "Ship" && isRotatable == true) // ���� ��Ŭ���� �� ������ ȸ�� ������ ���¸� ��Ȱ��ȭ ��Ų��.
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

    private void OnTriggerEnter2D(Collider2D collision) // ���� ȸ�� ���� �浹�Ѵٸ� �ٽ� ������ ����ġ ��Ų��.
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
