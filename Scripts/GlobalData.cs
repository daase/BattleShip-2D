using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

class GlobalDataSingleton
{
    public bool isReady  { set; get; } // 준비 상태인지 체크하는 변수
    public bool isAttacking { set; get; } // 공격 상태인지 판별하는 변수
    public bool isHit { set; get; } // 공격이 명중했는지 판별하는 변수
    public bool isMyTurn { set; get; } // 자신의 턴인지 체크하는 변수
    public bool isDestory { set; get; } // 공격으로 인해 상대의 
    public bool isFinish { set; get; } // 게임이 끝났음을 알리는 변수

    public GameObject[] Ships { set; get; } // 함선들을 저장할 배열

    public Tilemap MyTile { set; get; } // 자신의 타일맵
    public Tilemap EnemyTile { set; get; } // 상대방의 타일맵

    public Vector3 EnemyShipPosition { set; get; }
    public float EnemyShipAngle { set; get; }
    public string EnemyShipName { set; get; }   

    public int Hp {  set; get; }
  
    public GameObject popUp { set; get; }
    public Text popUpText { set; get; }

    private static GlobalDataSingleton instance;

    public static GlobalDataSingleton GetSingletonInstance()
    {
        // 인스턴스가 아직 생성되지 않은 경우에만 생성
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
