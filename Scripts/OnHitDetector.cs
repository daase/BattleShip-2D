using Photon.Pun;
using UnityEngine;

public class OnHitDetector : MonoBehaviourPunCallbacks
{
    private GlobalDataSingleton instance = GlobalDataSingleton.GetSingletonInstance();

    [SerializeField]
    private int hp;

    [SerializeField]
    private string shipName;

    private void Start()
    {
        SetHp(gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AimPoint") // 조준점의 collider에 닿으면 피격 상태가 된다.
        {
            photonView.RPC("OnHit", RpcTarget.Others); // 피격되었을 경우 다른 플레이어에게 맞았다고 rpc로 알린다.
            hp--;
            instance.Hp--;

            if (hp == 0) // 이 컴포넌트가 붙은 함선의 hp가 0이면 상대에게 파괴되었다고 알린다.
            {
                photonView.RPC("ShipDestroy", RpcTarget.Others, transform.position, transform.rotation.eulerAngles.z, gameObject.name);
                gameObject.GetComponent<OnHitDetector>().enabled = false;
            }
        }

        if (instance.Hp == 0) // 체력이 0이 되면 상대에게 졌다고 rpc로 알린다.
        {
            photonView.RPC("NoticeGameFinish", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void OnHit()
    {
        instance.isHit = true;
    }

    [PunRPC]
    public void ShipDestroy(Vector3 position, float angle, string shipName) // 파괴된 함선의 정보를 상대에게 알린다.
    {
        instance.isDestory = true;
        instance.EnemyShipPosition = position;
        instance.EnemyShipAngle = angle;
        instance.EnemyShipName = shipName;
        Debug.Log(gameObject.name + $" {hp} {PhotonNetwork.IsMasterClient}");
    }

    [PunRPC]
    public void NoticeGameFinish()
    {
        instance.isFinish = true;
    }

    public void SetHp(string shipName)
    {
        switch (shipName)
        {
            case "WarShip":
                hp = 5;
                break;

            case "BattleCrusier":
                hp = 4;
                break;

            case "Destroyer":
                hp = 3;
                break;

            case "TorpedoBoat":
                hp = 2;
                break;
        }
    }
}
