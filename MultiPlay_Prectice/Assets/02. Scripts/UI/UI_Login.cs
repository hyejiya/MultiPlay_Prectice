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
                                                    // 로그인 취소에 대한 알림창 팝업
                                                    UIManager.instance.Get<UIWarningWindow>()
                                                                      .Show("로그인 취소됨");
                                                    return;
                                                }

                                                if (task.IsFaulted)
                                                {
                                                    // 로그인 실패에 대한 알림창 팝업
                                                    UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show($"로그인 실패 {task.Exception.Message}");
                                                    return;
                                                }

                                                // 로그인 성공에 대한 알림창 팝업
                                                Login_Information.Refresh(id);

                                                UIManager.instance.Get<UIProfileSettingWindow>().Show();

                                                // 로그인 성공 후에 실행할 추가 내용 (씬 전환, 리소스 로드...)
/*                                                _ = GetNicknameAsync(id)
                                                    .ContinueWithOnMainThread(task =>
                                                    {
                                                        string nickname = task.Result;

                                                        if (string.IsNullOrEmpty(nickname))
                                                        {
                                                            UIManager.instance.Get<UIProfileSettingWindow>().Show();
                                                        }
                                                        else
                                                        {
                                                            // todo -> 로비 씬으로 이동..
                                                            //SceneManager.LoadScene("Lobby")
                                                        }

                                                });*/
                                            });
            });

            _register.onClick.AddListener(() =>
            {
                string id = _id.text;
                string pw = _pw.text;

                if (IsValidID(id) == false)
                {
                    UIManager.instance.Get<UIWarningWindow>()
                                        .Show("이메일 형식이 올바르지 않습니다");
                    return;
                }

                if (IsValidPW(pw) == false)
                {
                    UIManager.instance.Get<UIWarningWindow>()
                                        .Show("6자리 이상 입력해야 합니다");
                }


                FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(id, pw)
                                            .ContinueWithOnMainThread(task =>
                                            {
                                                if (task.IsCanceled)
                                                {
                                                    // todo -> 회원가입 취소에 대한 알림창 팝업
                                                    UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show("회원가입 취소됨");
                                                    return;
                                                }

                                                if(task.IsFaulted)
                                                {
                                                    // todo -> 회원가입 실패에 대한 알림창 팝업
                                                    UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show($"회원가입 실패 {task.Exception.Message}");
                                                    return;
                                                }

                                                // todo -> 회원가입 성공에 대한 알림창 팝업
                                                UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show("회원가입 성공");
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


