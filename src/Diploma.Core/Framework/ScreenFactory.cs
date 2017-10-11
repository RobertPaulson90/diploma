using System;
using Caliburn.Micro;
using JetBrains.Annotations;
using SimpleInjector;

namespace Diploma.Core.Framework
{
    internal sealed class ScreenFactory : IScreenFactory
    {
        [NotNull]
        private readonly Container _container;

        public ScreenFactory([NotNull] Container container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public TScreen CreateScreen<TScreen>()
            where TScreen : class, IScreen
        {
            var screen = _container.GetInstance<TScreen>();
            return screen;
        }
    }
}