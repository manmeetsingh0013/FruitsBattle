using CardMatch.Services;
using CardMatch.StateMachine.States;
using CardMatch.UI.Base;
using UnityEngine;

namespace CardMatch.UI.Screens
{
    public class MainMenuScreen : ScreenBase
    {
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private GameObject invalidGamePanel;
        [SerializeField] private GameConfig gameConfig;

        private Coroutine _titleCoroutine;

        protected internal override void EnableScreen()
        {
            base.EnableScreen();
            if((gameConfig.cardRow * gameConfig.cardCol) %2==1)
            {
                invalidGamePanel.gameObject.SetActive(true);
            }
        }

        protected internal override void DisableScreen()
        {
            base.DisableScreen();
            if (_titleCoroutine != null) StopCoroutine(_titleCoroutine);
        }

        public void PlayGame()
        {
            Bootstrap.GetService<AudioService>().PlayAudio(AudioTag.ButtonTap);
            Bootstrap.GetService<StateMachineService>().CurrentFsm.SetState(new PlayState());
        }

        public void ToggleInfoPanel(bool enable)
        {
            Bootstrap.GetService<AudioService>().PlayAudio(AudioTag.ButtonTap);
            infoPanel.SetActive(enable);
        }

        public void ExitGame()
        {
            Bootstrap.GetService<AudioService>().PlayAudio(AudioTag.ButtonTap);
            Bootstrap.GetService<StateMachineService>().CurrentFsm.SetState(new FinalState());
        }

        public override void OnBackKeyPressed() =>
            PreviousScreen(false);

    }
}