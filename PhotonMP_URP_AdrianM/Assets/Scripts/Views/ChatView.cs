using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ChatView : MonoBehaviour, IChatClientListener, IView
{
    private const string CHAT_CHANNEL_NAME = "globalChatChannel";
    private const string CHAT_DEFAULT_GREETING = "hello there.. my name is ";

    [SerializeField] private TextMeshProUGUI _entireChatText;
    [SerializeField] private TMP_InputField _inputFieldText;

    public ChatClient _chatClient;

    private string _playerName;
    private string _chatText;

    protected internal ChatAppSettings chatAppSettings;

    public void Initialize(string playerName)
    {
        ShowView();
        _playerName = playerName;
        Connect();
    }

    public void ShowView()
    {
        gameObject.SetActive(true);
    }

    public void HideView()
    {
        gameObject.SetActive(false);
    }

    private void Connect()
    {
        chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();

        _chatClient = new ChatClient(this);
        _chatClient.UseBackgroundWorkerForSending = true;

        _chatClient.ChatRegion = "EU";
        _chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(_playerName));
    }

    public void OnConnected()
    {
        _chatClient.Subscribe(CHAT_CHANNEL_NAME);
    }

    public void OnDisconnected()
    {
        Debug.Log(Time.time + " OnDisconnected() ");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        //Debug.Log(Time.time + " OnGetMessages() " + channelName);

        string msgs = "";
        for (int i = 0; i < senders.Length; i++)
        {
            msgs = string.Format("{0}{1} : {2} ", msgs, senders[i], messages[i]);

            _entireChatText.text += "\n" + msgs;
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log(Time.time + " OnPrivateMessage() " + sender);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log(Time.time + " OnStatusUpdate() " + user);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log(Time.time + " OnSubscribed() ");

        _chatClient.PublishMessage(CHAT_CHANNEL_NAME, CHAT_DEFAULT_GREETING + _playerName);
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log(Time.time + " OnUnsubscribed() ");
    }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.Log(Time.time + " OnUserSubscribed() ");
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log(Time.time + " OnUserUnsubscribed() ");
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(Time.time + " DebugReturn() " + message);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log(Time.time + " OnChatStateChange() " + state);
    }

    private void Update()
    {
        if (_chatClient != null)
        {
            _chatClient.Service();
        }
    }

    public void OnChatTextValueChanged(string chatText)
    {
        _chatText = chatText;
    }

    public void OnButtonSendChatText()
    {
        if (!string.IsNullOrEmpty(_chatText) && _chatText != "")
        {
            _chatClient.PublishMessage(CHAT_CHANNEL_NAME, _chatText);
            _chatText = "";
            _inputFieldText.text = "";
        }
    }

    public void OnDestroy()
    {
        if (_chatClient != null)
        {
            _chatClient.Disconnect();
        }
    }

    public void OnApplicationQuit()
    {
        if (_chatClient != null)
        {
            _chatClient.Disconnect();
        }
    }
}