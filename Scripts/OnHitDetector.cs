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
        if (collision.gameObject.tag == "AimPoint") // �������� collider�� ������ �ǰ� ���°� �ȴ�.
        {
            photonView.RPC("OnHit", RpcTarget.Others); // �ǰݵǾ��� ��� �ٸ� �÷��̾�� �¾Ҵٰ� rpc�� �˸���.
            hp--;
            instance.Hp--;

            if (hp == 0) // �� ������Ʈ�� ���� �Լ��� hp�� 0�̸� ��뿡�� �ı��Ǿ��ٰ� �˸���.
            {
                photonView.RPC("ShipDestroy", RpcTarget.Others, transform.position, transform.rotation.eulerAngles.z, gameObject.name);
                gameObject.GetComponent<OnHitDetector>().enabled = false;
            }
        }

        if (instance.Hp == 0) // ü���� 0�� �Ǹ� ��뿡�� ���ٰ� rpc�� �˸���.
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
    public void ShipDestroy(Vector3 position, float angle, string shipName) // �ı��� �Լ��� ������ ��뿡�� �˸���.
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
