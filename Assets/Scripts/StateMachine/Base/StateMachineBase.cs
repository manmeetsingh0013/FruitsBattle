using CardMatch.StateMachine.States.Base;
using UnityEngine;

namespace CardMatch.StateMachine.Base
{
    public abstract class StateMachineBase
    {
        protected StateBase CurrentState;
        private readonly MonoBehaviour referencedBehaviour;

        protected StateMachineBase(MonoBehaviour referencedBehaviour)
        {
            this.referencedBehaviour = referencedBehaviour;
        }
        
        internal void SetState(StateBase newState)
        {
            CurrentState = newState;
            referencedBehaviour.StartCoroutine(CurrentState.Initialise());
        }
    }
}