using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Utills
{
    public static class EventTriggerExtension
    {
        /// <summary>
        /// down ��ư�� ���� �Ҵ� �Ҽ� �ֵ��� ���ִ� Ȯ�� �ż���
        /// </summary>
        /// <param name="button"></param>
        /// <param name="action"></param>
        public static void AddDownButton(this EventTrigger button, Action action) {
            // �ʱ�ȭ
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { action?.Invoke(); });
            // �Ҵ�
            button.triggers.Add(entry);

        }
    }
}
