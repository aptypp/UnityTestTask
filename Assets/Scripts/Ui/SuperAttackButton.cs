using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.Ui
{
    public class SuperAttackButton : MonoBehaviour
    {
        public State currentState { get; private set; }

        [SerializeField]
        private Button _button;

        [SerializeField]
        private GameObject _cooldownState;

        [SerializeField]
        private GameObject _inactiveState;

        [SerializeField]
        private GameObject _activeState;

        [SerializeField]
        private Image _cooldownImage;

        public void SetOnClick(Action action) => _button.onClick.AddListener(() => action());

        public void ToActiveState()
        {
            currentState = State.Active;
            _activeState.SetActive(true);
            _inactiveState.SetActive(false);
            _cooldownState.SetActive(false);
            _button.interactable = true;
        }

        public void ToCooldownState(float cooldownTime, Action onCooldownEnd)
        {
            currentState = State.Cooldown;
            _activeState.SetActive(false);
            _inactiveState.SetActive(false);
            _cooldownState.SetActive(true);
            _button.interactable = false;

            StartCoroutine(CooldownAnimation(cooldownTime, onCooldownEnd));
        }

        public void ToInactiveState()
        {
            currentState = State.Inactive;
            _activeState.SetActive(false);
            _inactiveState.SetActive(true);
            _cooldownState.SetActive(false);
            _button.interactable = false;
        }


        private IEnumerator CooldownAnimation(float cooldownTime, Action onCooldownEnd)
        {
            float speed = 1.0f / cooldownTime;
            _cooldownImage.fillAmount = 1.0f;

            for (float progress = 0; progress < 1.0f; progress += speed * Time.deltaTime)
            {
                _cooldownImage.fillAmount = 1.0f - progress;
                yield return null;
            }

            _cooldownImage.fillAmount = 0;
            onCooldownEnd();
        }


        public enum State
        {
            Active,
            Inactive,
            Cooldown
        }
    }
}