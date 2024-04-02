using MP.Authentication;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace MP.Network
{
 
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        #region Singleton
        public static PhotonManager instance
        {
            get
            {
                if(s_instance == null)
                {
                    s_instance = new GameObject(typeof(PhotonManager).Name).AddComponent<PhotonManager>();  
                    DontDestroyOnLoad(s_instance.gameObject);   
                }
                return s_instance;
            }
        }
        private static PhotonManager s_instance;
        #endregion

        private void Awake()
        {
            if(PhotonNetwork.IsConnected == false)
            {
                bool IsConnected = PhotonNetwork.ConnectUsingSettings();
                Debug.Assert(IsConnected, "[PhotonManager] : Failed to connect to photon server");
            }
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnected();
            PhotonNetwork.AutomaticallySyncScene = true; // PhotonNetwork.LoadLevel() 호출 시 현재 동일한 방에 있는 모든 클라이언트의 씬을 동기화하는 옵션
            // PhotonNetwork.JoinLobby(); 로비에 들어가기
            // PhotonNetwork.CreateRoom(); 방만들기

            PhotonNetwork.NickName = Login_Information.nickname;
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log($"[PhotonManager] : Joined lobby");
        }

        /// <summary>
        /// 방에 입장한 플레이어 상태 저장
        /// </summary>
        public override void OnJoinedRoom() 
        {
            base.OnJoinedRoom();

            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
            {
                {"isReady", false },
            });
        }

        /// <summary>
        /// 방에서 나갔을 때 데이터 초기화
        /// </summary>
        public override void OnLeftRoom()
        {
            base.OnLeftRoom();

            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
            {

            });
        }
    }



}

