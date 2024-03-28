﻿using MP.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MP.UI
{
    /// <summary>
    /// 모든 UI를 관리, 전체스크린용 및 팝업용 UI 추가로 관리.
    /// </summary>
    public class UIManager : SingletonMonoBase<UIManager>
    {
        private Dictionary<Type,IUI> _uis = new Dictionary<Type, IUI> ();
        private List<IUI> _screens = new List<IUI>();
        private Stack<IUI> _popups = new Stack<IUI>();

        private void Update()
        {
            if(_popups.Count > 0)
            {
                if (_popups.Peek().inputActionEnable)
                    _popups.Peek().InputAction();
            }
        }


        /// <summary>
        /// 스크리용 UI 등록
        /// </summary>
        public void RegisterScreen(IUI ui)
        {
            if(_uis.TryAdd(ui.GetType(), ui)) //여기서 문제가 생기면 Hierachy창에 동일한 UI가 두 개 있는 것이다
            {
                _screens.Add(ui);
            }
            else 
                throw new Exception($"[UIManager] : UI 등록 실패. {ui.GetType()}는 이미 등록되어있습니다...");
        }

        /// <summary>
        /// 팝업용 UI 등록
        /// </summary>
        public void RegisterPopup(IUI ui)
        {
            if (_uis.TryAdd(ui.GetType(), ui)) //여기서 문제가 생기면 Hierachy창에 동일한 UI가 두 개 있는 것이다
            {
                
            }
            else
                throw new Exception($"[UIManager] : UI 등록 실패. {ui.GetType()}는 이미 등록되어있습니다...");
        }

        public T Get<T>()
            where T : IUI
        {
            return (T)_uis[typeof(T)];
        }

        public void PushPopup(IUI ui)
        {
            if(_popups.Count > 0)
            {
                _popups.Peek().inputActionEnable = false; //기존에 있던 팝업의 입력 안먹히게
            }

            _popups.Push(ui);
            _popups.Peek().inputActionEnable = true; //새로 띄울 팝업의 입력 먹히게
            _popups.Peek().sortingOrder = _popups.Count; //새로 띄울 팝업을 최상단으로 정렬
        }

        public void PopPopup(IUI ui)
        {
            //닫으려는 UI가 최상단에 있지 않으면 예외
            if(_popups.Peek() != ui)
                throw new Exception($"[UIManager] : {ui.GetType()} 팝업을 닫기 시도하였으나 최상단에 있지 않음..");

            _popups.Pop();

            //이전 팝업의 입력이 먹히게
            if (_popups.Count > 0)
                _popups.Peek().inputActionEnable = true;
        }
    }
}