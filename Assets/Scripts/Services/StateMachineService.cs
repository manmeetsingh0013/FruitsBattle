using CardMatch.Services.Base;
using CardMatch.StateMachine;
using CardMatch.StateMachine.States;

namespace CardMatch.Services
{
    public class StateMachineService : ServiceBase
    {
        internal GameplaySystem CurrentFsm { get; private set; }

        protected override void RegisterService()
        {
            CurrentFsm ??= new GameplaySystem(this);
            CurrentFsm.SetState(new InitialState());
            Bootstrap.RegisterService(this);
        }
    }
}