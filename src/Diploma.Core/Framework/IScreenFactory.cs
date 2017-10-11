using Caliburn.Micro;

namespace Diploma.Core.Framework
{
    public interface IScreenFactory
    {
        TScreen CreateScreen<TScreen>() where TScreen : class, IScreen;
    }
}