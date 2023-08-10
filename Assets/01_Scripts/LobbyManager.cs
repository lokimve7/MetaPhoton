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
    //방 참여 버튼
    public Button btnJoinRoom;
    //방 생성 버튼
    public Button btnCreateRoom;

    void Start()
    {
        //방 참여, 생성 비활성화
        btnJoinRoom.interactable = btnCreateRoom.interactable = false;
        //inputRoomName 의 내용이 변경될 때 호출되는 함수
        inputRoomName.onValueChanged.AddListener(OnValueChangedRoomName);
        //inputMaxPlayer 의 내용이 변경될 때 호출되는 함수
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
