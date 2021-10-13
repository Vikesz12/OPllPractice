using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    [RequireComponent(typeof(Button))]
    public class ChangeAllToggles : MonoBehaviour
    {
        [SerializeField] private bool _changeTo;
        [SerializeField] private Transform _toggleParent;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(ChangeToggles);
        }

        private void OnDestroy() => _button.onClick.RemoveListener(ChangeToggles);

        private void ChangeToggles()
        {
            var toggles = _toggleParent.GetComponentsInChildren<Toggle>();

            foreach (var toggle in toggles)
            {
                toggle.isOn = _changeTo;
            }
        }
    }
}
