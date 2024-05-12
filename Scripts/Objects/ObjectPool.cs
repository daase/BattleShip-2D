using System.Collections.Generic;
using UnityEngine;

class ObjectPool // ������Ʈ Ǯ
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

    public static void InitPool() // ������Ʈ Ǯ�� ������Ʈ���� �����ϴ� �޼ҵ�
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

    public static GameObject GetPoolObject(int count) // ������ƮǮ���� ������Ʈ���� �������� �޼ҵ�
    {
        return objectPool[count];
    }
}
