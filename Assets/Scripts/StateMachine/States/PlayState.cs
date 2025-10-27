using System.Collections;
using CardMatch.Services;
using CardMatch.StateMachine.States.Base;
using CardMatch.UI.Screens;
using UnityEngine;

namespace CardMatch.StateMachine.States
{
    public class PlayState : StateBase
    {
        internal override IEnumerator Initialise()
        {
            Bootstrap.GetService<UserInterfaceService>()
                .CurrentInterface.ActivateThisScreen<GameplayHudScreen>();
            yield return Bootstrap.GetService<StateMachineService>().StartCoroutine(Perform());
        }

        internal override IEnumerator Perform()
        {
            yield return new WaitForSeconds(.5F);
            var hudScreen = Bootstrap.GetService<UserInterfaceService>()
                .CurrentInterface
                .GetScreen<GameplayHudScreen>();
            if(!hudScreen.IsCardMatched())
                yield return hudScreen.ShowCountdownTimer();
        }
    }
}