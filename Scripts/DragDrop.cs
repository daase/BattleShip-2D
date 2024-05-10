using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class DragDrop : MonoBehaviour
{

    private Tilemap tileMap; 

    private Vector2 originPos;
    private Vector3 offset;
    private Vector2 mousePos;
   
    private Rigidbody2D rb;

    private bool isDrag = false;  
    private bool isTriggered = false;

    private GlobalDataSingleton instance;

    private void Start()
    {
        instance = GlobalDataSingleton.GetSingletonInstance();
        originPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        if (tileMap == null) tileMap = instance.MyTile;
        originPos = transform.position;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDrag = true;
        rb.freezeRotation = true;
    }

    private void OnMouseUp()
    {
        if(isDrag)
        {
            isDrag = false;
            SnapToTile();

            if (isTriggered) // 마우스를 놓았을 때 충돌 상태일 때 드래그하기 전의 위치로 오브젝트를 되돌린다.
            {
                transform.position = originPos;
            }
            rb.freezeRotation = false;
        }

        else
        {
            if (isTriggered) // 마우스를 놓았을 때 충돌 상태일 때 드래그하기 전의 위치로 오브젝트를 되돌린다.
            {
                transform.position = originPos;
            }
            rb.freezeRotation = false;
        }

    }

    void OnMouseDrag() // 드래그에 따라 오브젝트의 위치를 변경한다.
    {
        if (isDrag)
        {          
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            rb.MovePosition(mousePosition);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "CollisionTile" && isDrag == true) // 타일맵 밖으로 드래그하려면 즉시 드래그를 비활성화
        {            
            isDrag = false;            
            SnapToTile();
            rb.freezeRotation = false;                       
        }
        
        if(collision.gameObject.tag == "Ship") // 드래그 도중 다른 Ship에 닿으면 충돌 상태를 활성화
        {
            
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // 드래그를 하다 Ship 오브젝트에서 벗어나면 충돌 상태를 해제한다.
    {
        if(collision.gameObject.tag == "Ship" )
        {
           
            isTriggered = false;
        }
    }

    void SnapToTile() // 가까운 타일맵의 중앙으로 오브젝트를 이동시키는 메소드
    {               
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = tileMap.GetCellCenterWorld(tileMap.WorldToCell(mousePos));                          
    }
}
