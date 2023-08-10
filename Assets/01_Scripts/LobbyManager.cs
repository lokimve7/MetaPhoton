using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //Input Room Name
    public InputField inputRoomName;
    //Input Max Player
    public InputField inputMaxPlayer;
    //�� ���� ��ư
    public Button btnJoinRoom;
    //�� ���� ��ư
    public Button btnCreateRoom;

    void Start()
    {
        //�� ����, ���� ��Ȱ��ȭ
        btnJoinRoom.interactable = btnCreateRoom.interactable = false;
        //inputRoomName �� ������ ����� �� ȣ��Ǵ� �Լ�
        inputRoomName.onValueChanged.AddListener(OnValueChangedRoomName);
        //inputMaxPlayer �� ������ ����� �� ȣ��Ǵ� �Լ�
        inputMaxPlayer.onValueChanged.AddListener(OnValueChangedMaxPlayer);
    }

    //���� & ���� ��ư�� ����
    void OnValueChangedRoomName(string room)
    {
        //���� ��ư Ȱ��/ ��Ȱ��
        btnJoinRoom.interactable = room.Length > 0;
        //���� ��ư Ȱ�� / ��Ȱ��
        btnCreateRoom.interactable = room.Length > 0 && inputMaxPlayer.text.Length > 0;
    }

    //���� ��ư�� ����
    void OnValueChangedMaxPlayer(string max)
    {
        btnCreateRoom.interactable = max.Length > 0 && inputRoomName.text.Length > 0;
    }

    public void CreateRoom()
    {
        //�� �ɼ��� ���� (�ִ� �ο�)
        RoomOptions option = new RoomOptions();
        option.MaxPlayers = int.Parse(inputMaxPlayer.text);
        //�� ��Ͽ� ���̰� �ϳ�? ���ϳ�?
        option.IsVisible = true;
        //�濡 ������ �� �ִ�? ����?
        option.IsOpen = true;

        //Ư�� �κ� �� ���� ��û
        //TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);
        //PhotonNetwork.CreateRoom(inputRoomName.text, option, typedLobby);
        
        //�⺻ �κ� �� ���� ��û
        PhotonNetwork.CreateRoom(inputRoomName.text, option);
    }

    //�� ���� �Ϸ�� ȣ�� �Ǵ� �Լ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�� ���� �Ϸ�");
    }

    //�� ���� ���н� ȣ�� �Ǵ� �Լ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("�� ���� ���� : " + message);
    }

    public void JoinRoom()
    {
        //�� ���� ��û
        PhotonNetwork.JoinRoom(inputRoomName.text);
    }

    // �� ���� �Ϸ�� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("�� ���� �Ϸ�");

        //GameScene ���� �̵�
    }

    // �� ���� ���н� ȣ��Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("�� ���� ���� : " + message);
    }

    // ������ ���� ����ų� ���������� ȣ��Ǵ� �Լ�
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
       
        for(int i = 0; i < roomList.Count; i++)
        {
            print(i + "��° �� : " + roomList[i].Name);
        }
    }


    void Update()
    {
      
    }
}
