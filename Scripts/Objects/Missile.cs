using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 5f; // �̻����� �ӵ�

    void Update()
    {
        // �� �����Ӹ��� �̻����� ���� �̵�
        transform.Translate(Vector3.up * speed * Time.deltaTime);
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "AimPoint")
        {
            ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.missile).SetActive(false);
        }
    }
}
