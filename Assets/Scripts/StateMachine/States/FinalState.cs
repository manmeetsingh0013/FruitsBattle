using System.Collections;
using CardMatch.StateMachine.States.Base;
using UnityEngine;

namespace CardMatch.StateMachine.States
{
    public class FinalState : StateBase
    {
        internal override IEnumerator Initialise()
        {
            Application.Quit(0);
            yield break;
        }
    }
}