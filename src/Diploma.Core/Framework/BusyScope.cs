using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Diploma.Core.Framework
{
    public sealed class BusyScope : INotifyPropertyChanged
    {
        private bool _isBusy;

        private int _worksCount;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy
        {
            get => _isBusy;

            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private int WorksCount
        {
            get => _worksCount;

            set
            {
                if (_worksCount == value)
                {
                    return;
                }

                _worksCount = value;

                if (_worksCount == 1)
                {
                    IsBusy = true;
                }
                else if (_worksCount == 0)
                {
                    IsBusy = false;
                }
            }
        }

        public IDisposable StartWork()
        {
            return new DisposableScope(this);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private class DisposableScope : IDisposable
        {
            private readonly BusyScope _parent;

            public DisposableScope(BusyScope parent)
            {
                _parent = parent;
                _parent.WorksCount++;
            }

            public void Dispose()
            {
                _parent.WorksCount--;
            }
        }
    }
}
