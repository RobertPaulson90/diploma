using System;
using Caliburn.Micro;
using Diploma.Core.Framework;
using Diploma.Framework.Messages;
using JetBrains.Annotations;

namespace Diploma.ViewModels
{
    internal sealed class AuthenticationManagerViewModel : Conductor<IScreen>, IHandle<ChangeAuthenticationManagerStateMessage>
    {
        [NotNull]
        private readonly IScreenFactory _screenFactory;

        [NotNull]
        private readonly IEventAggregator _eventAggregator;

        public AuthenticationManagerViewModel([NotNull] IScreenFactory screenFactory, [NotNull] IEventAggregator eventAggregator)
        {
            _screenFactory = screenFactory ?? throw new ArgumentNullException(nameof(screenFactory));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        }

        void IHandle<ChangeAuthenticationManagerStateMessage>.Handle(ChangeAuthenticationManagerStateMessage message)
        {
            ActiveItem = GetScreenByState(message.State);

            IScreen GetScreenByState(AuthenticationManagerState state)
            {
                switch (state)
                {
                    case AuthenticationManagerState.Login:
                        return _screenFactory.CreateScreen<LoginViewModel>();
                    case AuthenticationManagerState.Register:
                        return _screenFactory.CreateScreen<RegisterViewModel>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state));
                }
            }
        }

        protected override void OnActivate()
        {
            _eventAggregator.Subscribe(this);
            ActiveItem = _screenFactory.CreateScreen<LoginViewModel>();
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
        }
    }
}
