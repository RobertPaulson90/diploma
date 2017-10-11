namespace Diploma.Framework.Messages
{
    public enum AuthenticationManagerState
    {
        Login,

        Register
    }

    internal sealed class ChangeAuthenticationManagerStateMessage
    {
        public ChangeAuthenticationManagerStateMessage(AuthenticationManagerState state)
        {
            State = state;
        }

        public AuthenticationManagerState State { get; }
    }
}
