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
    //채팅에 접속하기 위한 옵션
    ChatAppSettings chatAppSettings;

    //채팅을 관리하는 Class
    ChatClient chatClient;

    //채널 목록
    public string[] channelNames;

    //채팅 입력 UI
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
            "김현진" + "</color>" + " : " + s;

            chatClient.PublishMessage(currChannel, chat);
        });

        //포톤 설정을 가져와서 CahtAppSettings 에 설정하자
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
            // 계속 호출해줘야 이벤트 등을 받을 수 있다.
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
            chatClient.SendPrivateMessage("강한나", "111");
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            string[] channel = new string[1];
            channel[0] = "채널" + chatClient.PublicChannels.Count;
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

        //백그라운드 상태로 갔을 때도 채팅 서버와의 연결을 유지 한다.
        chatClient.UseBackgroundWorkerForSending = true;
        //채팅할 때 NickName 을 설정한다.
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues("Joe");
        
        //설정을 이용하여 연결 시도
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

        //채널 추가, 단독으로 하나씩 채널을 추가 할 수 도 있다.
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
            //만들어진 item 의 부모를 content 로 한다.
            //ci.transform.SetParent(rtContent);

            //만들어진 item 에서 ChatItem 컴포넌트를 가져온다.
            ChatItem item = ci.GetComponent<ChatItem>();
            //가져온 컴포넌트에서 SetText 함수를 실행
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
    //채널 추가 성공시 호출 되는 함수
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
                print("선택 된 채팅 채널 : " + channelName);

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
                        //만들어진 item 의 부모를 content 로 한다.
                        //ci.transform.SetParent(rtContent);

                        //만들어진 item 에서 ChatItem 컴포넌트를 가져온다.
                        ChatItem item = ci.GetComponent<ChatItem>();
                        //가져온 컴포넌트에서 SetText 함수를 실행
                        item.SetText(channel.Messages[i].ToString());
                    }
                }

            });
        }
        
    }
    

    //채널 삭제 성공시 호출 되는 함수
    public void OnUnsubscribed(string[] channels)
    {
        print("----- " + nameof(OnUnsubscribed) + " -----");
        for (int i = 0; i < channels.Length; i++)
            print(" channels : " + channels[i]);
        print("----- " + nameof(OnUnsubscribed) + " -----");
    }

    //유저가 Online, Offline 등 상태 변경시 호출되는 함수 
    //내 상태를 변경하면 호출되지 않고 접속한 사람들의 상태가 변경될 때만 호출
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
