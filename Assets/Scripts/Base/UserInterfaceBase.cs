using System;
using System.Collections.Generic;
using CardMatch.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CardMatch.UI.Base
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class UserInterfaceBase : MonoBehaviour
    {
        [SerializeField] protected List<ScreenBase> screenBases;

        private int _currentIndex;
        private InputAction _escapeKeyAction;

        internal IReadOnlyList<ScreenBase> Screens => screenBases;

        protected virtual void Awake() =>
            _escapeKeyAction = new InputAction("Escape",
                InputActionType.Button, Keyboard.current.escapeKey.path, "tap");

        protected virtual void Start()
        {
            Bootstrap.GetService<UserInterfaceService>().CurrentInterface = this;
            screenBases.ForEach(x => x.DisableScreen());
            screenBases[0].EnableScreen();
        }

        private void OnEnable()
        {
            _escapeKeyAction.started += OnEscapeKeyCallback;
            _escapeKeyAction.Enable();
        }

        private void OnDisable()
        {
            _escapeKeyAction.started -= OnEscapeKeyCallback;
            _escapeKeyAction.Disable();
        }

        private void OnEscapeKeyCallback(InputAction.CallbackContext callbackContext)
        {
            switch (callbackContext.control.name)
            {
                case "escape":
                    screenBases[_currentIndex].OnBackKeyPressed();
                    break;
            }
        }

        internal virtual void ActivatePreviousScreen(ScreenBase screen, bool disableCurrent = true)
        {
            var index = screenBases.FindIndex(x => x.Equals(screen));
            if (index > 0)
            {
                if (disableCurrent)
                    screen.DisableScreen();
                screenBases[_currentIndex = index - 1].EnableScreen();
            }
            else Debug.LogError("Doesn't have a Previous Screen!");
        }

        internal virtual void ActivateNextScreen(ScreenBase screen, bool disableCurrent = true)
        {
            var index = screenBases.FindIndex(x => x.Equals(screen));
            if (index < screenBases.Count - 1)
            {
                if (disableCurrent)
                    screen.DisableScreen();
                screenBases[_currentIndex = index + 1].EnableScreen();
            }
            else Debug.LogError("Doesn't have a Next Screen!");
        }

        internal virtual void ActivateNextScreen(int index = -1, bool disableCurrent = true)
        {
            if (index < 0)
                index = _currentIndex;

            if (index < screenBases.Count - 1)
            {
                if (disableCurrent)
                    screenBases[_currentIndex = index].DisableScreen();
                screenBases[_currentIndex = index + 1].EnableScreen();
            }
            else Debug.LogError("Doesn't have a Next Screen!");
        }

        internal virtual void ActivatePreviousScreen(int index = -1, bool disableCurrent = true)
        {
            if (index < 0)
                index = _currentIndex;

            if (index > 0)
            {
                if (disableCurrent)
                    screenBases[_currentIndex = index].DisableScreen();
                screenBases[_currentIndex = index - 1].EnableScreen();
            }
            else Debug.LogError("Doesn't have a Previous Screen!");
        }

        internal virtual void ActivateThisScreen<T>(bool disableCurrent = true) where T : ScreenBase
        {
            var screen = GetScreen<T>();
            if (!screen)
            {
                Debug.LogError($"Cannot Activate Screen: {nameof(T)}");
                return;
            }

            if (disableCurrent)
                screenBases[_currentIndex].DisableScreen();
            else screen.SetOverridePreviousScreen(screenBases[_currentIndex]);

            screen.EnableScreen();
            _currentIndex = screenBases.FindIndex(x => x.Equals(screen));
        }

        internal virtual void ActivateThisScreen(ScreenBase screen, bool disableCurrent = true)
        {
            if (disableCurrent)
                screenBases[_currentIndex].DisableScreen();
            else screen.SetOverridePreviousScreen(screenBases[_currentIndex]);

            screen.EnableScreen();
            _currentIndex = screenBases.FindIndex(x => x.Equals(screen));
        }

        internal virtual void DeActivateThisScreen<T>() where T : ScreenBase
        {
            var screen = GetScreen<T>();
            if (!screen)
            {
                Debug.LogError($"Cannot Activate Screen: {nameof(T)}");
                return;
            }

            screen.DisableScreen();
        }

        internal virtual void DeActivateThisScreen(ScreenBase screen) =>
            screen.DisableScreen();

        internal T GetScreen<T>() where T : ScreenBase => (T) screenBases.Find(x => x.GetType() == typeof(T));

        internal ScreenBase GetScreen(Type screenType) =>
            screenBases.Find(x => x.GetType() == screenType);
    }
}