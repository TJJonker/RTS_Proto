namespace Jonko.Patterns
{
    public interface IStatePattern
    {
        public void EnterState();

        public void UpdateState();

        public void ExitState();
    }
}