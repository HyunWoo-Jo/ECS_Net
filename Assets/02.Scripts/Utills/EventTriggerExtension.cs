using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Utills
{
    public static class EventTriggerExtension
    {
        /// <summary>
        /// down 버튼을 쉽게 할당 할수 있도록 해주는 확장 매서드
        /// </summary>
        /// <param name="button"></param>
        /// <param name="action"></param>
        public static void AddDownButton(this EventTrigger button, Action action) {
            // 초기화
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { action?.Invoke(); });
            // 할당
            button.triggers.Add(entry);

        }
    }
}
