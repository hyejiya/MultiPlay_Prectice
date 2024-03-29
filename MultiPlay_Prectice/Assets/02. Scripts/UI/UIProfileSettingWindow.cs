using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using MP.Authentication;
using System.Collections.Generic;

namespace MP.UI
{
    public class UIProfileSettingWindow : UIPopupBase
    {
        private TMP_InputField _nickname;
        private Button _confirm;


        protected override void Awake()
        {
            base.Awake();

            _nickname = transform.Find("Panel/InputField (TMP) - Nickname").GetComponent<TMP_InputField>();
            _confirm = transform.Find("Panel/Button - Confirm").GetComponent <Button>();
            onInputActionEnableChanged += value =>
            {
                _nickname.interactable = value;
                _confirm.interactable = value;
            };

            _confirm.onClick.AddListener(() =>
            {
                string nickname = _nickname.text;
                CollectionReference userCollectionRef = FirebaseFirestore.DefaultInstance.Collection("users");
                userCollectionRef.GetSnapshotAsync()
                                    .ContinueWithOnMainThread(task =>
                                    {
                                        //todo -> profile의 닉네임 중복검사, 중복된거 있으면 알림창 띄어줌
                                        //없으면 프로필 정보 새로 등록하고 로비씬으로 넘어가야함.

                                        //닉네임 중복 검사
                                        foreach(DocumentSnapshot document in task.Result.Documents)
                                        {
                                            if (document.GetValue<string>("nickname").Equals(nickname))
                                            {
                                                UIManager.instance.Get<UIWarningWindow>()
                                                                    .Show("이미 존재하는 닉네임입니다");
                                                return;
                                            }                                            
                                        }

                                        FirebaseFirestore.DefaultInstance
                                            .Collection("users")
                                                .Document(Login_Information.userKey)
                                                    .SetAsync(new Dictionary<string, object>
                                                    {
                                                        {"nickname", nickname },
                                                    })
                                                    .ContinueWithOnMainThread(task =>
                                                    {
                                                        Login_Information.nickname = nickname;
                                                    });
                                    });
            });

            _confirm.interactable = false;
            _nickname.onValueChanged.AddListener(value => _confirm.interactable = IsValidNickname(value));

        }

        private bool IsValidNickname(string nickname)
        {
            return (nickname.Length > 1) && (nickname.Length < 11);
        }
    }
}

