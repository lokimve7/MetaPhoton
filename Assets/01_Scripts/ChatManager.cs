using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChatManager : MonoBehaviour
{
    //InputField 
    public InputField chatInput;

    //ChatItem Prefab
    public GameObject chatItemFactory;
    //ScrollView 에 있는 Content 의 RectTrasform
    public RectTransform rtContent;

    void Start()
    {
        //엔터키를 누르면 InputField 에 있는 텍스트 내용 알려주는 함수 등록
        chatInput.onSubmit.AddListener(OnSubmit);

        //InputField 의 내용이 변경 될 때마다 호출해주는 함수 등록
        chatInput.onValueChanged.AddListener(OnValueChanged);

        //InputField 의 Focusing 이 사라졌을 때 호출해주는 함수 등록
        chatInput.onEndEdit.AddListener(OnEndEdit);
    }

    void Update()
    {
        
    }

    void OnSubmit(string s)
    {
        //print("OnSubmit : " + s);
        //Chatitem 을 만든다.
        GameObject ci = Instantiate(chatItemFactory);
        //만들어진 item 의 부모를 content 로 한다.
        ci.transform.SetParent(rtContent);

        //닉네임을 붙여서 채팅내용을 만들자
        //"<color=#ffff00> 원하는 내용 </color>"
        string chat = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.blue) + ">" +
            PhotonNetwork.NickName + "</color>" + " : " + s;

        //만들어진 item 에서 ChatItem 컴포넌트를 가져온다.
        ChatItem item = ci.GetComponent<ChatItem>();        
        //가져온 컴포넌트에서 SetText 함수를 실행
        item.SetText(chat);

        //chatInput 값을 초기화
        chatInput.text = "";

        //chatInput 을 활성화 하자
        chatInput.ActivateInputField();
    }

    void OnValueChanged(string s)
    {
        //print("OnValueChanged : " + s);
    }

    void OnEndEdit(string s)
    {
        //print("OnEndEdit : " + s);
    }
}
