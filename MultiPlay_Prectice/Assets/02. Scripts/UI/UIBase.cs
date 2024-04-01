using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MP.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UIBase : MonoBehaviour, IUI
    {
        public int sortingOrder 
        { 
            get => _canvas.sortingOrder; 
            set => _canvas.sortingOrder = value; 
        }
        
        public bool inputActionEnable 
        {
            get => _inputActionEnable;  
            set
            {
                if (_inputActionEnable)
                    return;

                _inputActionEnable = value;
                onInputActionEnableChanged?.Invoke(value);
            }
        }

        private bool _inputActionEnable;
        private Canvas _canvas;
        private GraphicRaycaster _module;
        private EventSystem _eventSystem;

        public event Action<bool> onInputActionEnableChanged;

        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _module = GetComponent<GraphicRaycaster>();
            _eventSystem = EventSystem.current;
        }

        public virtual void Show()
        {
            _canvas.enabled = true;
        }

        public virtual void Hide()
        {
            _canvas.enabled = false;
        }

        public virtual void InputAction()
        {
          
        }

        public bool Raycast(List<RaycastResult> results)
        {
            int count = results.Count;
            PointerEventData pointEventData = new PointerEventData(_eventSystem);
            pointEventData.position = Input.mousePosition;
            _module.Raycast(pointEventData, results);
            return count < results.Count;
        }


    }
}
