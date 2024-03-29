using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MP.UI
{
    /// <summary>
    /// Canvas 단위의 기본 인터페이스
    /// </summary>
    public interface IUI 
    {
        /// <summary>
        /// Canvas의 정렬 순서
        /// </summary>
        int sortingOrder { get; set; }

        /// <summary>
        /// 유저의 입력 상호작용 가능 여부
        /// </summary>
        bool inputActionEnable { get; set; }

        /// <summary>
        /// 유저의 입력 상호작용 가능 여부가 바뀌었을 때
        /// </summary>
        event Action<bool> onInputActionEnableChanged;


        /// <summary>
        /// Canvas 활성화
        /// </summary>
        void Show();

        /// <summary>
        /// Canvas 비활성화
        /// </summary>
        void Hide();

        /// <summary>
        /// 유저와의 상호작용
        /// </summary>
        void InputAction();

        /// <summary>
        /// 이 Canvas 내의 어떤 RaycastTarget을 감지하기 위한 기능 
        /// </summary>
        /// <param name="results"> 결과 반환용 버퍼 </param>
        /// <returns> 감지된 타켓이 있으면 true, 없으면 false </returns>
        bool Raycast(List<RaycastResult> results);
    }
}

