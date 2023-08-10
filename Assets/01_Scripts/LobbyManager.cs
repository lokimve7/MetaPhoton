using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
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

    void OnValueChangedRoomName(string room)
    {

    }

    void OnValueChangedMaxPlayer(string max)
    {

    }

    void Update()
    {
        
    }
}
