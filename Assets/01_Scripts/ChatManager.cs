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
    //ScrollView �� �ִ� Content �� RectTrasform
    public RectTransform rtContent;

    void Start()
    {
        //����Ű�� ������ InputField �� �ִ� �ؽ�Ʈ ���� �˷��ִ� �Լ� ���
        chatInput.onSubmit.AddListener(OnSubmit);

        //InputField �� ������ ���� �� ������ ȣ�����ִ� �Լ� ���
        chatInput.onValueChanged.AddListener(OnValueChanged);

        //InputField �� Focusing �� ������� �� ȣ�����ִ� �Լ� ���
        chatInput.onEndEdit.AddListener(OnEndEdit);
    }

    void Update()
    {
        
    }

    void OnSubmit(string s)
    {
        //print("OnSubmit : " + s);
        //Chatitem �� �����.
        GameObject ci = Instantiate(chatItemFactory);
        //������� item �� �θ� content �� �Ѵ�.
        ci.transform.SetParent(rtContent);

        //�г����� �ٿ��� ä�ó����� ������
        //"<color=#ffff00> ���ϴ� ���� </color>"
        string chat = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.blue) + ">" +
            PhotonNetwork.NickName + "</color>" + " : " + s;

        //������� item ���� ChatItem ������Ʈ�� �����´�.
        ChatItem item = ci.GetComponent<ChatItem>();        
        //������ ������Ʈ���� SetText �Լ��� ����
        item.SetText(chat);

        //chatInput ���� �ʱ�ȭ
        chatInput.text = "";

        //chatInput �� Ȱ��ȭ ����
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
