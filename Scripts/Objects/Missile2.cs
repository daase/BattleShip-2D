using UnityEngine;

public class Missile2 : MonoBehaviour
{
    public float speed = 5f; // �̻����� �ӵ�

    void Update()
    {
        // �� �����Ӹ��� �̻����� �Ʒ��� �̵�
        transform.Translate(Vector3.down * speed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AimPoint")
        {
            ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.missile2).SetActive(false);
        }
    }
}
