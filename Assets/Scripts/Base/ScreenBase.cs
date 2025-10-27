using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.UI.Base
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class ScreenBase : MonoBehaviour
    {
        private Canvas _screenCanvas;
        private GraphicRaycaster _screenRaycaster;
        protected UserInterfaceBase currentInterfaceManager;

        [Header("Optional")] [SerializeField] private ScreenBase overrideNextScreen;
        [SerializeField] private ScreenBase overridePreviousScreen;

        protected virtual void Awake() => InitializeScreen();

        protected virtual void OnDestroy()
        {
        }

        internal bool IsScreenEnabled => _screenCanvas.enabled && _screenRaycaster.enabled;

        protected virtual void InitializeScreen()
        {
            _screenCanvas = GetComponent<Canvas>();
            _screenRaycaster = GetComponent<GraphicRaycaster>();
            currentInterfaceManager = GetComponentInParent<UserInterfaceBase>();
        }

        internal void SetOverridePreviousScreen(ScreenBase screen) =>
            overridePreviousScreen = screen;

        protected internal virtual void EnableScreen()
        {
            if (_screenCanvas is { })
                _screenCanvas.enabled = true;
            else Debug.LogWarning($"{name} Canvas Null");
            if (_screenRaycaster is { })
                _screenRaycaster.enabled = true;
            else Debug.LogWarning($"{name} Raycaster Null");
        }

        protected internal virtual void DisableScreen()
        {
            if (_screenCanvas is { })
                _screenCanvas.enabled = false;
            else Debug.LogWarning($"{name} Canvas Null");
            if (_screenRaycaster is { })
                _screenRaycaster.enabled = false;
            else Debug.LogWarning($"{name} Raycaster Null");
        }

        public abstract void OnBackKeyPressed();

        internal void PreviousScreen(bool disableCurrent)
        {
            if (!currentInterfaceManager)
            {
                Debug.LogWarning("Interface Manager is not yet Initialised!");
                return;
            }

            if (!overridePreviousScreen)
                currentInterfaceManager.ActivatePreviousScreen(this, disableCurrent);
            else currentInterfaceManager.ActivateThisScreen(overridePreviousScreen, disableCurrent);
        }

        internal void NextScreen(bool disableCurrent)
        {
            if (!currentInterfaceManager)
            {
                Debug.LogWarning("Interface Manager is not yet Initialised!");
                return;
            }

            if (!overrideNextScreen)
                currentInterfaceManager.ActivateNextScreen(this, disableCurrent);
            else currentInterfaceManager.ActivateThisScreen(overrideNextScreen, disableCurrent);
        }
    }
}