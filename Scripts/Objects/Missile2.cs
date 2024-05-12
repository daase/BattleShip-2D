using UnityEngine;

public class Missile2 : MonoBehaviour
{
    public float speed = 5f; // 미사일의 속도

    void Update()
    {
        // 매 프레임마다 미사일을 아래로 이동
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
