using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MP.GameElements
{
    [RequireComponent(typeof(PhotonView), typeof(PhotonTransformView))]
    public class ClientCharacterController : MonoBehaviour, IPunObservable
    {
        public Vector3 velocity { get; set; }
        private PhotonView _view;

        private void Awake()
        {
            _view = GetComponent<PhotonView>();
        }

        private void Update()
        {
            if(_view.IsMine)
            {
                velocity = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));

            }
        }

        private void FixedUpdate()
        {
            if(_view.IsMine)
            {
                transform.position += velocity * Time.fixedDeltaTime;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //데이터 송신
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            //데이터 수신
            else
            {
                transform.position = (Vector3)stream.ReceiveNext();
                transform.rotation = (Quaternion)stream.ReceiveNext();

            }
        }
    }
}

