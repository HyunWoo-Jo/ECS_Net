using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Game.Mono.UI
{
    public class UI_View<T> : MonoBehaviour, IView where T : IPresenter
    {
        protected T _presenter;

        protected virtual void Awake() {
            _presenter = this.GetComponent<T>();
        }
        /// <summary>
        /// 버튼에 action 할당
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="action"></param>
        protected void Assign2Button(EventTrigger trigger, Action action) {
            EventTrigger.Entry eventTrigger_pointDown = new();
            eventTrigger_pointDown.eventID = EventTriggerType.PointerDown;
            eventTrigger_pointDown.callback.AddListener((data) => { action(); });
            trigger.triggers.Add(eventTrigger_pointDown);
        }
    }
}
