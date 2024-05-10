using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

class GlobalDataSingleton
{
    public bool isReady  { set; get; } // �غ� �������� üũ�ϴ� ����
    public bool isAttacking { set; get; } // ���� �������� �Ǻ��ϴ� ����
    public bool isHit { set; get; } // ������ �����ߴ��� �Ǻ��ϴ� ����
    public bool isMyTurn { set; get; } // �ڽ��� ������ üũ�ϴ� ����
    public bool isDestory { set; get; } // �������� ���� ����� 
    public bool isFinish { set; get; } // ������ �������� �˸��� ����

    public GameObject[] Ships { set; get; } // �Լ����� ������ �迭

    public Tilemap MyTile { set; get; } // �ڽ��� Ÿ�ϸ�
    public Tilemap EnemyTile { set; get; } // ������ Ÿ�ϸ�

    public Vector3 EnemyShipPosition { set; get; }
    public float EnemyShipAngle { set; get; }
    public string EnemyShipName { set; get; }   

    public int Hp {  set; get; }
  
    public GameObject popUp { set; get; }
    public Text popUpText { set; get; }

    private static GlobalDataSingleton instance;

    public static GlobalDataSingleton GetSingletonInstance()
    {
        // �ν��Ͻ��� ���� �������� ���� ��쿡�� ����
        if (instance == null)
        {
            instance = new GlobalDataSingleton();
        }
        return instance;
    }

    public void SetPopUp(string message)
    {
        popUpText.text = message;
        popUp.SetActive(true);
    }

}
