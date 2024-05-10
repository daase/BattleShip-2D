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

            if (isTriggered) // ���콺�� ������ �� �浹 ������ �� �巡���ϱ� ���� ��ġ�� ������Ʈ�� �ǵ�����.
            {
                transform.position = originPos;
            }
            rb.freezeRotation = false;
        }

        else
        {
            if (isTriggered) // ���콺�� ������ �� �浹 ������ �� �巡���ϱ� ���� ��ġ�� ������Ʈ�� �ǵ�����.
            {
                transform.position = originPos;
            }
            rb.freezeRotation = false;
        }

    }

    void OnMouseDrag() // �巡�׿� ���� ������Ʈ�� ��ġ�� �����Ѵ�.
    {
        if (isDrag)
        {          
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            rb.MovePosition(mousePosition);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "CollisionTile" && isDrag == true) // Ÿ�ϸ� ������ �巡���Ϸ��� ��� �巡�׸� ��Ȱ��ȭ
        {            
            isDrag = false;            
            SnapToTile();
            rb.freezeRotation = false;                       
        }
        
        if(collision.gameObject.tag == "Ship") // �巡�� ���� �ٸ� Ship�� ������ �浹 ���¸� Ȱ��ȭ
        {
            
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // �巡�׸� �ϴ� Ship ������Ʈ���� ����� �浹 ���¸� �����Ѵ�.
    {
        if(collision.gameObject.tag == "Ship" )
        {
           
            isTriggered = false;
        }
    }

    void SnapToTile() // ����� Ÿ�ϸ��� �߾����� ������Ʈ�� �̵���Ű�� �޼ҵ�
    {               
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = tileMap.GetCellCenterWorld(tileMap.WorldToCell(mousePos));                          
    }
}
