using MP.GameElements;
using MP.GameElements.Characters;
using MP.GameElements.Interactions;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MP.UI
{
    public class UI_GamePlay : UIScreenBase
    {
        private Button _jump;
        private Button _interaction;
        private List<RaycastResult> _raycastBuffer = new List<RaycastResult>();

        protected override void Awake()
        {
            base.Awake();
            _jump = transform.Find("Button - Jump").GetComponent<Button>();
            _interaction = transform.Find("Button - Interaction").GetComponent <Button>();

            _jump.onClick.AddListener(() =>
            {
                if (ClientCharacterController._spawned.TryGetValue(PhotonNetwork.LocalPlayer.ActorNumber,
                    out ClientCharacterController controller))
                {
                    controller.ChangeState(State.Jump);
                }
            });

            _interaction.onClick.AddListener(() =>
            {
                if (ClientCharacterController._spawned.TryGetValue(PhotonNetwork.LocalPlayer.ActorNumber,
                    out ClientCharacterController controller) && controller.TryGetComponent(out PlayerInteractor interactor))
                { 
                    {
                        interactor.TryInteraction();
                    }
                    controller.ChangeState(State.Jump);
                }
            });

            Show();
        }

        public override void InputAction()
        {
            base.InputAction();

            if (Input.GetMouseButtonDown(0))
            {
                _raycastBuffer.Clear();

                //유저가 월드를 클릭했을 때 플레이어를 해당위치로 이동 시킴
                if (Raycast(_raycastBuffer) == false)
                {
                    if(ClientCharacterController._spawned.TryGetValue(PhotonNetwork.LocalPlayer.ActorNumber,
                        out ClientCharacterController controller))
                    {
                        controller.MoveTo(Input.mousePosition);
                    }       
                }
            }
        }
    }
}

