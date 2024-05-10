using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AimPoint : MonoBehaviour
{
    private GlobalDataSingleton instance = GlobalDataSingleton.GetSingletonInstance();
    
    private void OnTriggerEnter2D(Collider2D collision) // 미사일 오브젝트에 닿았을 때 미사일과 조준점인 본인을 동시에 비활성화 시킨다.
    {
        if (collision.gameObject.tag == "Missile")
        {
            instance.isAttacking = false;
            ObjectPool.GetPoolObject(0).SetActive(false);
        }
    }
}
