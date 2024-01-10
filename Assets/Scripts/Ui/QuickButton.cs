using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TestTask.Ui
{
    public class QuickButton : MonoBehaviour, IPointerDownHandler
    {
        public event Action onClick;

        public void OnPointerDown(PointerEventData eventData) => onClick?.Invoke();
    }
}