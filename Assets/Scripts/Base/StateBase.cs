using System.Collections;

namespace CardMatch.StateMachine.States.Base
{
    public abstract class StateBase
    {
        internal virtual IEnumerator Initialise()
        {
            yield break;
        }

        internal virtual IEnumerator Perform()
        {
            yield break;
        }

        internal virtual IEnumerator Finalise()
        {
            yield break;
        }
    }
}