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
            PhotonNetwork.AutomaticallySyncScene = true; // PhotonNetwork.LoadLevel() ȣ�� �� ���� ������ �濡 �ִ� ��� Ŭ���̾�Ʈ�� ���� ����ȭ�ϴ� �ɼ�
            // PhotonNetwork.JoinLobby(); �κ� ����
            // PhotonNetwork.CreateRoom(); �游���

            PhotonNetwork.NickName = Login_Information.nickname;
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log($"[PhotonManager] : Joined lobby");
        }

        /// <summary>
        /// �濡 ������ �÷��̾� ���� ����
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
        /// �濡�� ������ �� ������ �ʱ�ȭ
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

