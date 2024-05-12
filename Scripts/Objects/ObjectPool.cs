using System.Collections.Generic;
using UnityEngine;

class ObjectPool // 오브젝트 풀
{
    private static List<GameObject> objectPool = new List<GameObject>();

    private static int poolSize = 5;
    private static GameObject poolObject;

    public enum ObjectType
    {
        aimpoint,
        missile,
        missile2,
        splash,
        explose
    };

    public static void InitPool() // 오브젝트 풀에 오브젝트들을 세팅하는 메소드
    {
        for(int i = 0; i < poolSize; i++)
        {
            switch (i)
            {
                case 0:
                    poolObject = Resources.Load<GameObject>("Prefabs/AimPoint");
                    SetObject(poolObject);
                    break;

                case 1:
                    poolObject = Resources.Load<GameObject>("Prefabs/Missile");
                    SetObject(poolObject);
                    break;

                case 2:
                    poolObject = Resources.Load<GameObject>("Prefabs/Missile2");
                    SetObject(poolObject);
                    break;

                case 3:
                    poolObject = Resources.Load<GameObject>("Prefabs/Splash");
                    SetObject(poolObject);
                    break;

                case 4:
                    poolObject = Resources.Load<GameObject>("Prefabs/Explose");
                    SetObject(poolObject);
                    break;
            }
        }
    }

    public static void SetObject(GameObject go)
    {
        GameObject obj = Object.Instantiate(go);
        obj.SetActive(false);
        objectPool.Add(obj);
    }

    public static GameObject GetPoolObject(int count) // 오브젝트풀에서 오브젝트들을 가져오는 메소드
    {
        return objectPool[count];
    }
}
