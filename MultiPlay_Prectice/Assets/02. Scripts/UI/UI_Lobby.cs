using MP.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MP.UI
{
    public class UI_Lobby : UIScreenBase, ILobbyCallbacks, IMatchmakingCallbacks
    {
        private const int NOT_SELECTED = -1;
        public int roomListSlotIndexSelected
        {
            get => _roomListSlotIndexSelected;
            set
            {
                _roomListSlotIndexSelected = value;
                _joinRoom.interactable = value > NOT_SELECTED;
            }
        }

        // Main panel
        private int _roomListSlotIndexSelected = NOT_SELECTED;
        private RoomListSlot _roomListSlot;
        private List<RoomListSlot> _roomListSlots = new List<RoomListSlot>(20);
        private RectTransform _roomListContent;
        private Button _joinRoom;
        private Button _createRoom;
        private List<RoomInfo> _localRoomList;

        // RoomOption panel
        private GameObject _roomOptionPanel;
        private TMP_InputField _roomName;
        private Scrollbar _maxPlayer;
        private TMP_Text _maxPlayerValue;
        private Button _confirmRoomOptions;
        private Button _cancelRoomOptions;

        protected override void Awake()
        {
            base.Awake();
            _roomListSlot = Resources.Load<RoomListSlot>("UI/RoomListSlot");
            _roomListContent = transform.Find("Panel/Scroll View - RoomList/Viewport/Content").GetComponent<RectTransform>();
            _joinRoom = transform.Find("Panel/Button - JoinRoom").GetComponent<Button>();
            _createRoom = transform.Find("Panel/Button - CreateRoom").GetComponent<Button>();
            _joinRoom.interactable = false;
            _joinRoom.onClick.AddListener(() =>
            {
                if (PhotonNetwork.JoinRoom(_localRoomList[_roomListSlotIndexSelected].Name))
                {
                    // 입장중.. 같은 메세지 띄우면서 유저한테 기다려달라고 하는 UI 띄우기
                    UIManager.instance.Get<UI_LoadingPanel>()
                                        .Show();
                }
                else
                {
                    UIManager.instance.Get<UIWarningWindow>()
                                        .Show("The room is invalid");
                }
            });
            _createRoom.onClick.AddListener(() =>
            {
                //todo -> 방 생성 옵션 창 띄우기
            });

            _createRoom.onClick.AddListener(() =>
            {
                _roomName.text = string.Empty;
                _maxPlayer.value = 0f;
                _roomOptionPanel.gameObject.SetActive(true);
            });

            _roomOptionPanel = transform.Find("Panel - RoomOption").gameObject;
            _roomName = transform.Find("Panel - RoomOption/BG/InputField (TMP) - RoomName").GetComponent<TMP_InputField>();
            _maxPlayer = transform.Find("Panel - RoomOption/BG/Scrollbar - MaxPlayer").GetComponent<Scrollbar>();
            _maxPlayerValue = transform.Find("Panel - RoomOption/BG/Text (TMP) - MaxPlayerValue").GetComponent<TMP_Text>();
            _confirmRoomOptions = transform.Find("Panel - RoomOption/BG/Button - ConfirmRoomOptions").GetComponent<Button>();
            _cancelRoomOptions = transform.Find("Panel - RoomOption/BG/Button - CancelRoomOptions").GetComponent<Button>();

            _roomName.onValueChanged.AddListener((value) => _confirmRoomOptions.interactable = value.Length > 1); // 방 제목 두 글자 이상일 떄만 확인버튼 누를 수 있음
            _maxPlayer.onValueChanged.AddListener(value =>
            {
                _maxPlayerValue.text = Mathf.RoundToInt(value * _maxPlayer.numberOfSteps + 1).ToString();
            });
            _cancelRoomOptions.onClick.AddListener(() => _roomOptionPanel.gameObject.SetActive(false));
            _confirmRoomOptions.interactable = false;
            _confirmRoomOptions.onClick.AddListener(() =>
            {
                if (PhotonNetwork.CreateRoom(_roomName.text,
                                        new RoomOptions
                                        {
                                            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
                                            {
                                            { "levelLimit", 10}
                                            },
                                            MaxPlayers = Mathf.RoundToInt(_maxPlayer.value * _maxPlayer.numberOfSteps + 1),
                                            PublishUserId = true
                                        }))
                {
                    // 방 생성중.. 같은 메세지 띄우면서 유저한테 기다려달라고 하는 UI 띄우기
                    UIManager.instance.Get<UI_LoadingPanel>()
                                        .Show();
                }
            });
        }

        private void Start()
        {
            StartCoroutine(C_JoinLobby());
        }

        IEnumerator C_JoinLobby()
        {
            yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer);
            PhotonNetwork.JoinLobby();
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
            StartCoroutine(C_JoinLobby());
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnJoinedLobby()
        {
            Debug.Log("[UI Lobby] : Joined lobby");
            //Join 되기 전에는 로딩패널 같은 거 띄어 놓고 이 함수가 호출되면 로딩패널 지워주기
            UIManager.instance.Get<UI_LoadingPanel>()
                                .Hide();
        }

        public void OnLeftLobby()
        {
            throw new System.NotImplementedException();
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            _localRoomList = roomList;
            for (int i = 0; i < _roomListSlots.Count; i++)
                Destroy(_roomListSlots[i].gameObject);

            _roomListSlots.Clear();

            for (int i = 0; i < roomList.Count; i++)
            {
                RoomListSlot slot = Instantiate(_roomListSlot, _roomListContent);
                slot.roomIndex = i;
                slot.OnSelected += (index) => roomListSlotIndexSelected = index;
                slot.Refresh(roomList[i].Name, roomList[i].PlayerCount, roomList[i].MaxPlayers);
                _roomListSlots.Add(slot);

            }
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            throw new System.NotImplementedException();
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            throw new System.NotImplementedException();
        }

        public void OnCreatedRoom()
        {
            UIManager.instance.Get<UI_LoadingPanel>()
                                 .Hide();
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            UIManager.instance.Get<UI_LoadingPanel>()
                                .Hide();
        }

        public void OnJoinedRoom()
        {
            UIManager.instance.Get<UI_LoadingPanel>()
                                .Hide();
            UIManager.instance.Get<UI_ReadyGamePlayInRoom>()
                                .Show();
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            UIManager.instance.Get<UI_LoadingPanel>()
                                .Hide();
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            UIManager.instance.Get<UI_LoadingPanel>()
                                .Hide();
        }

        public void OnLeftRoom()
        {
            UIManager.instance.Get<UI_LoadingPanel>()
                                .Hide();
            Show();
        }
    }
}



