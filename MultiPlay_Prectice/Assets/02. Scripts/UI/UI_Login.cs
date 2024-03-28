using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

namespace MP.UI
{
    public class UI_Login : MonoBehaviour
    {
        private InputField _id;
        private InputField _pw;
        
        private Button _trylogin;
        private Button _register;

        private async void Awake()
        {
            _id = transform.Find("Panel/InputField (TMP) - ID").GetComponent<InputField>();
            _pw = transform.Find("Panel/InputField (TMP) - PW").GetComponent<InputField>();
            _trylogin = transform.Find("Panel/Button - TryLogin").GetComponent<Button>();
            _register = transform.Find("Panel/Button - Register").GetComponent<Button>();

            var dependencyState = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyState != DependencyStatus.Available)
                throw new Exception();

            _trylogin.onClick.AddListener(() =>
            {
                FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(_id.text, _pw.text)
                                            .ContinueWithOnMainThread(task =>
                                            {
                                                if (task.IsCanceled)
                                                {
                                                    // todo -> �α��� ��ҿ� ���� �˸�â �˾�
                                                    UIManager.instance.Get<UIWarningWindow>()
                                                                      .Show("�α��� ��ҵ�");
                                                    return;
                                                }

                                                if (task.IsFaulted)
                                                {
                                                    // todo -> �α��� ���п� ���� �˸�â �˾�
                                                    UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show("�α��� ����");
                                                    return;
                                                }

                                                UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show("�α��� ����");
                                                // todo -> �α��� ������ ���� �˸�â �˾�
                                                // todo -> �α��� ���� �Ŀ� ������ �߰� ���� (�� ��ȯ, ���ҽ� �ε�...)
                                            });
            });

            _register.onClick.AddListener(() =>
            {
                FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(_id.text, _pw.text)
                                            .ContinueWithOnMainThread(task =>
                                            {
                                                if (task.IsCanceled)
                                                {
                                                    // todo -> ȸ������ ��ҿ� ���� �˸�â �˾�
                                                    UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show("ȸ������ ��ҵ�");
                                                    return;
                                                }

                                                if(task.IsFaulted)
                                                {
                                                    // todo -> ȸ������ ���п� ���� �˸�â �˾�
                                                    UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show("ȸ������ ����");
                                                    return;
                                                }

                                                // todo -> ȸ������ ������ ���� �˸�â �˾�
                                                UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show("ȸ������ ����");
                                            });
            });
        }
    }
}


