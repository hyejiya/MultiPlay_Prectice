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
                                                    // todo -> 로그인 취소에 대한 알림창 팝업
                                                    UIManager.instance.Get<UIWarningWindow>()
                                                                      .Show("로그인 취소됨");
                                                    return;
                                                }

                                                if (task.IsFaulted)
                                                {
                                                    // todo -> 로그인 실패에 대한 알림창 팝업
                                                    UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show("로그인 실패");
                                                    return;
                                                }

                                                UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show("로그인 성공");
                                                // todo -> 로그인 성공에 대한 알림창 팝업
                                                // todo -> 로그인 성공 후에 실행할 추가 내용 (씬 전환, 리소스 로드...)
                                            });
            });

            _register.onClick.AddListener(() =>
            {
                FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(_id.text, _pw.text)
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
                                                                     .Show("회원가입 실패");
                                                    return;
                                                }

                                                // todo -> 회원가입 성공에 대한 알림창 팝업
                                                UIManager.instance.Get<UIWarningWindow>()
                                                                     .Show("회원가입 성공");
                                            });
            });
        }
    }
}


