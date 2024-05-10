using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AimPoint : MonoBehaviour
{
    private GlobalDataSingleton instance = GlobalDataSingleton.GetSingletonInstance();
    
    private void OnTriggerEnter2D(Collider2D collision) // �̻��� ������Ʈ�� ����� �� �̻��ϰ� �������� ������ ���ÿ� ��Ȱ��ȭ ��Ų��.
    {
        if (collision.gameObject.tag == "Missile")
        {
            instance.isAttacking = false;
            ObjectPool.GetPoolObject(0).SetActive(false);
        }
    }
}
