using CardMatch.Services.Base;
using CardMatch.UI.Base;
using UnityEngine.EventSystems;

namespace CardMatch.Services
{
    public class UserInterfaceService : ServiceBase
    {
        private EventSystem _currentEventSystem;
        private UserInterfaceBase _currentInterface;

        internal UserInterfaceBase CurrentInterface
        {
            get => _currentInterface;
            set
            {
                _currentEventSystem = FindObjectOfType<EventSystem>();
                _currentInterface = value;
            }
        }

        internal void ToggleInteractions(bool enable) =>
            _currentEventSystem.enabled = enable;

        protected override void RegisterService() =>
            Bootstrap.RegisterService(this);
    }
}