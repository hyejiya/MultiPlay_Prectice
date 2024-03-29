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
                                        //todo -> profile�� �г��� �ߺ��˻�, �ߺ��Ȱ� ������ �˸�â �����
                                        //������ ������ ���� ���� ����ϰ� �κ������ �Ѿ����.

                                        //�г��� �ߺ� �˻�
                                        foreach(DocumentSnapshot document in task.Result.Documents)
                                        {
                                            if (document.GetValue<string>("nickname").Equals(nickname))
                                            {
                                                UIManager.instance.Get<UIWarningWindow>()
                                                                    .Show("�̹� �����ϴ� �г����Դϴ�");
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

