using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    //ä�ÿ� �����ϱ� ���� �ɼ�
    ChatAppSettings chatAppSettings;

    //ä���� �����ϴ� Class
    ChatClient chatClient;

    //ä�� ���
    public string[] channelNames;

    //ä�� �Է� UI
    public InputField inputChat;

    public GameObject chatItemFactory;

    public RectTransform rtContent;


    public RectTransform rtCanvas;
    public GameObject channelTabFactory;

    public string currChannel;

    void Start()
    {

        currChannel = "Region";

        inputChat.onSubmit.AddListener((string s) => {
            string chat = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.blue) + ">" +
            "������" + "</color>" + " : " + s;

            chatClient.PublishMessage(currChannel, chat);
        });

        //���� ������ �����ͼ� CahtAppSettings �� ��������
        AppSettings settings = PhotonNetwork.PhotonServerSettings.AppSettings;
        chatAppSettings = new ChatAppSettings();
        chatAppSettings.AppIdChat = settings.AppIdChat;
        chatAppSettings.AppVersion = settings.AppVersion;
        chatAppSettings.FixedRegion = settings.IsBestRegion ? null : settings.FixedRegion;
        chatAppSettings.NetworkLogging = settings.NetworkLogging;
        chatAppSettings.Protocol = settings.Protocol;
        chatAppSettings.EnableProtocolFallback = settings.EnableProtocolFallback;
        chatAppSettings.Server = settings.IsDefaultNameServer ? null : settings.Server;
        chatAppSettings.Port = (ushort)settings.Port;
        chatAppSettings.ProxyServer = settings.ProxyServer;

        PhotonChatConnect();
    }
    void Update()
    {
        if (chatClient != null)
        {
            // ��� ȣ������� �̺�Ʈ ���� ���� �� �ִ�.
            chatClient.Service();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            string[] channel = new string[1];
            channel[0] = channelNames[0];
            chatClient.Unsubscribe(channel);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            chatClient.SetOnlineStatus(ChatUserStatus.Offline);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            chatClient.SendPrivateMessage("���ѳ�", "111");
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            string[] channel = new string[1];
            channel[0] = "ä��" + chatClient.PublicChannels.Count;
            chatClient.Subscribe(channel);
        }
    }

    private void OnDestroy()
    {
        if (chatClient != null)
        {
            chatClient.Disconnect();
        }
    }

    private void OnApplicationQuit()
    {
        if (chatClient != null)
        {
            chatClient.Disconnect();
        }
    }

    void PhotonChatConnect()
    {
        chatClient = new ChatClient(this);

        //��׶��� ���·� ���� ���� ä�� �������� ������ ���� �Ѵ�.
        chatClient.UseBackgroundWorkerForSending = true;
        //ä���� �� NickName �� �����Ѵ�.
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues("Joe");
        
        //������ �̿��Ͽ� ���� �õ�
        chatClient.ConnectUsingSettings(chatAppSettings);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        print("----- " + nameof(DebugReturn) + " -----");
        print(" level : " + level);
        print(" message : " + message);
        print("----- " + nameof(DebugReturn) + " -----");
    }

    public void OnDisconnected()
    {
        print("----- " + nameof(OnDisconnected) + " -----");
    }

    public void OnConnected()
    {
        print("----- " + nameof(OnConnected) + " -----");

        //ä�� �߰�, �ܵ����� �ϳ��� ä���� �߰� �� �� �� �ִ�.
        if (channelNames.Length > 0)
        {
            chatClient.Subscribe(channelNames);
            
        }

        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnChatStateChange(ChatState state)
    {
        print("----- " + nameof(OnChatStateChange) + " -----");
        print(" state : " + state);
        print("----- " + nameof(OnChatStateChange) + " -----");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        print("----- " + nameof(OnGetMessages) + " -----");
        print(" channelName : " + channelName);
        for (int i = 0; i < senders.Length; i++)
            print(" senders : " + senders[i]);
        for (int i = 0; i < messages.Length; i++)
            print(" messages : " + messages[i]);
        print("----- " + nameof(OnGetMessages) + " -----");

        for(int i = 0; i < senders.Length; i++)
        {
            GameObject ci = Instantiate(chatItemFactory, rtContent);
            //������� item �� �θ� content �� �Ѵ�.
            //ci.transform.SetParent(rtContent);

            //������� item ���� ChatItem ������Ʈ�� �����´�.
            ChatItem item = ci.GetComponent<ChatItem>();
            //������ ������Ʈ���� SetText �Լ��� ����
            item.SetText(messages[i].ToString());
        }
    }

    //
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        print("----- " + nameof(OnPrivateMessage) + " -----");
        print(" channelName : " + channelName);
        print(" sender : " + sender);
        print(" message : " + message);
        print("----- " + nameof(OnPrivateMessage) + " -----");
    }
    //ä�� �߰� ������ ȣ�� �Ǵ� �Լ�
    public void OnSubscribed(string[] channels, bool[] results)
    {
        print("----- " + nameof(OnSubscribed) + " -----");
        for (int i = 0; i < channels.Length; i++)
            print(" channels : " + channels[i]);
        for (int i = 0; i < results.Length; i++)
            print(" results : " + results[i]);
        print("----- " + nameof(OnSubscribed) + " -----");

        //20, 195
        for(int i = 0; i < channels.Length; i++)
        {
            GameObject channelTab = Instantiate(channelTabFactory, rtCanvas);
            RectTransform rt = channelTab.GetComponent<RectTransform>();
            rt.anchoredPosition = 
                new Vector2((chatClient.PublicChannels.Count - 1) * 100 + 20, 195);
            ChannelTab tab = channelTab.GetComponent<ChannelTab>();
            tab.SetInfo(channels[i], (string channelName) => {
                print("���� �� ä�� ä�� : " + channelName);

                currChannel = channelName;

                for(int i = 0; i < rtContent.childCount; i++)
                {
                    Destroy(rtContent.GetChild(i).gameObject);
                }

                ChatChannel channel = null;
                bool found = chatClient.TryGetChannel(channelName, out channel);
                if (found)
                {
                    for (int i = 0; i < channel.Senders.Count; i++)
                    {
                        GameObject ci = Instantiate(chatItemFactory, rtContent);
                        //������� item �� �θ� content �� �Ѵ�.
                        //ci.transform.SetParent(rtContent);

                        //������� item ���� ChatItem ������Ʈ�� �����´�.
                        ChatItem item = ci.GetComponent<ChatItem>();
                        //������ ������Ʈ���� SetText �Լ��� ����
                        item.SetText(channel.Messages[i].ToString());
                    }
                }

            });
        }
        
    }
    

    //ä�� ���� ������ ȣ�� �Ǵ� �Լ�
    public void OnUnsubscribed(string[] channels)
    {
        print("----- " + nameof(OnUnsubscribed) + " -----");
        for (int i = 0; i < channels.Length; i++)
            print(" channels : " + channels[i]);
        print("----- " + nameof(OnUnsubscribed) + " -----");
    }

    //������ Online, Offline �� ���� ����� ȣ��Ǵ� �Լ� 
    //�� ���¸� �����ϸ� ȣ����� �ʰ� ������ ������� ���°� ����� ���� ȣ��
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        print("----- " + nameof(OnStatusUpdate) + " -----");
        print(" user : " + user);
        print(" status : " + status);
        print(" gotMessage : " + gotMessage);
        print(" message : " + message);
        print("----- " + nameof(OnStatusUpdate) + " -----");
    }

    public void OnUserSubscribed(string channel, string user)
    {
        print("----- " + nameof(OnUserSubscribed) + " -----");
        print(" channel : " + channel);
        print(" user : " + user);
        print("----- " + nameof(OnUserSubscribed) + " -----");
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        print("----- " + nameof(OnUserUnsubscribed) + " -----");
        print(" channel : " + channel);
        print(" user : " + user);
        print("----- " + nameof(OnUserUnsubscribed) + " -----");
    }
}
