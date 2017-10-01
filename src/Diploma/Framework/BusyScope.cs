using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Diploma.Framework
{
    public sealed class BusyScope : INotifyPropertyChanged
    {
        private bool _isBusy;

        private int _worksCount;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private int WorksCount
        {
            get
            {
                return _worksCount;
            }

            set
            {
                if (_worksCount == value)
                {
                    return;
                }

                if (_worksCount == 0 && value == 1)
                {
                    IsBusy = true;
                }
                else if (_worksCount == 1 && value == 0)
                {
                    IsBusy = false;
                }

                _worksCount = value;
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
