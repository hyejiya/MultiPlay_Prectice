using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System.Text.RegularExpressions;
using MP.Authentication;
using System.Threading.Tasks;
using Firebase.Firestore;
using System.Collections.Generic;
using MP.Utilities;
using UnityEngine.SceneManagement;

namespace MP.UI
{
    public class UI_Login : MonoBehaviour
    {
        private TMP_InputField _id;
        private TMP_InputField _pw;
        
        private Button _trylogin;
        private Button _register;

        private async void Awake()
        {
            _id = transform.Find("Panel/InputField (TMP) - ID").GetComponent<TMP_InputField>();
            _pw = transform.Find("Panel/InputField (TMP) - PW").GetComponent<TMP_InputField>();
            _trylogin = transform.Find("Panel/Button - TryLogin").GetComponent<Button>();
            _register = transform.Find("Panel/Button - Register").GetComponent<Button>();

            var dependencyState = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyState != DependencyStatus.Available)
                throw new Exception();

            _trylogin.onClick.AddListener(() =>
            {
                string id = _id.text;
                string pw = _pw.text;

                FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(id, pw)
                                            .ContinueWithOnMainThread(async task =>
                                            {
                                                if (task.IsCanceled)
                                                {
                                                    UpdateDispatcher.instance.Enqueue(() =>
                                                    {
                                                        // �α��� ��ҿ� ���� �˸�â �˾�
                                                        UIManager.instance.Get<UIWarningWindow>()
                                                                          .Show("�α��� ��ҵ�");
                                                    });
  
                                                    return;
                                                }

                                                if (task.IsFaulted)
                                                {
                                                    UpdateDispatcher.instance.Enqueue(() =>
                                                    {
                                                        // �α��� ���п� ���� �˸�â �˾�
                                                        UIManager.instance.Get<UIWarningWindow>()
                                                                         .Show($"�α��� ���� {task.Exception.Message}");
                                                    });
   
                                                    return;
                                                }

                                                // �α��� ������ ���� �˸�â �˾�
                                                Login_Information.Refresh(id);

                                                UIManager.instance.Get<UIProfileSettingWindow>().Show();

                                                await FirebaseFirestore.DefaultInstance
                                                                    .Collection("users")
                                                                      .Document(Login_Information.userKey)
                                                                         .GetSnapshotAsync()
                                                                            .ContinueWithOnMainThread(task =>
                                                                            {
                                                                                Dictionary<string, object> documentDictionary = task.Result.ToDictionary();

                                                                                UpdateDispatcher.instance.Enqueue(() =>
                                                                                {
                                                                                    if (documentDictionary.TryGetValue("nickname", out object value))
                                                                                    {
                                                                                        Login_Information.nickname = (string)value;
                                                                                        SceneManager.LoadScene("Lobby");
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        UIManager.instance.Get<UIProfileSettingWindow>().Show();
                                                                                    }
                                                                                });

                                                                                
                                                                            });
                                            });
            });

            _register.onClick.AddListener(() =>
            {
                string id = _id.text;
                string pw = _pw.text;

                if (IsValidID(id) == false)
                {
                    UIManager.instance.Get<UIWarningWindow>()
                                        .Show("�̸��� ������ �ùٸ��� �ʽ��ϴ�");
                    return;
                }

                if (IsValidPW(pw) == false)
                {
                    UIManager.instance.Get<UIWarningWindow>()
                                        .Show("6�ڸ� �̻� �Է��ؾ� �մϴ�");
                }


                FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(id, pw)
                                            .ContinueWithOnMainThread(task =>
                                            {
                                                if (task.IsCanceled)
                                                {
                                                    UpdateDispatcher.instance.Enqueue(() =>
                                                    {
                                                        // todo -> ȸ������ ��ҿ� ���� �˸�â �˾�
                                                        UIManager.instance.Get<UIWarningWindow>()
                                                                         .Show("ȸ������ ��ҵ�");
                                                    });

                                                    return;
                                                }

                                                if(task.IsFaulted)
                                                {
                                                    UpdateDispatcher.instance.Enqueue(() =>
                                                    {
                                                        // todo -> ȸ������ ���п� ���� �˸�â �˾�
                                                        UIManager.instance.Get<UIWarningWindow>()
                                                                         .Show($"ȸ������ ���� {task.Exception.Message}");
                                                    });

                                                    return;
                                                }

                                                // todo -> ȸ������ ������ ���� �˸�â �˾�
                                                UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show("ȸ������ ����");
                                            });
            });
        }

        private async Task<string> GetNicknameAsync(string userKey)
        {
            string nickname = string.Empty;

            await FirebaseFirestore.DefaultInstance
                     .Collection("users")
                        .Document(userKey)
                           .GetSnapshotAsync()
                                .ContinueWithOnMainThread(task =>
                                {
                                    Dictionary<string, object> documentDictionary = task.Result.ToDictionary();
                                    if(documentDictionary.TryGetValue("nickname", out object value))
                                        nickname = (string)value;
                                });
            return nickname;
        }

        private bool IsValidID(string id)
        {
            return Regex.IsMatch(id, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPW(string pw)
        {
            return pw.Length >= 6;
        }
    }


}


