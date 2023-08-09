using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class SimpleConnectionMgr : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        //Photon ȯ�漳���� ������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //������ ���� ���� �Ϸ�
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        print(nameof(OnConnectedToMaster));

        //�κ�����
        JoinLobby();
    }

    //�κ�����
    void JoinLobby()
    {
        //�г��� ����
        PhotonNetwork.NickName = "������" + Random.Range(0, 100);
        //�⺻ Lobby ����
        PhotonNetwork.JoinLobby();
    }

    //�κ����� �Ϸ�
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(nameof(OnJoinedLobby));
        //�� ���� or ����
        RoomOptions roomOptioin = new RoomOptions();

        //�濡 ���� �� �ִ� �ִ� �ο�
        roomOptioin.MaxPlayers = 3;

        PhotonNetwork.JoinOrCreateRoom("meta_unity_room", roomOptioin, TypedLobby.Default);
    }

    //�� ���� �Ϸ�� ȣ�� �Ǵ� �Լ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(nameof(OnCreatedRoom));
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(nameof(OnCreateRoomFailed));

        //�� ���� ���� ������ �����ִ� �˾� ������ ����?
    }

    //�� ���� ������ ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));
        //Game Scene ���� �̵�
        PhotonNetwork.LoadLevel("GameScene");
    }


}
