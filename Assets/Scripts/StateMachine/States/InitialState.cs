using System.Collections;
using CardMatch.Audio;
using CardMatch.Services;
using CardMatch.StateMachine.States.Base;
using CardMatch.UI.Screens;
using UnityEngine;

namespace CardMatch.StateMachine.States
{
    public class InitialState : StateBase
    {
        internal override IEnumerator Initialise()
        {
            yield return null;
            Bootstrap.GetService<UserInterfaceService>()
                .CurrentInterface
                .ActivateThisScreen<MainMenuScreen>();
            yield return null;
            Bootstrap.GetService<AudioService>().PlayAudio(AudioTag.BG);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}