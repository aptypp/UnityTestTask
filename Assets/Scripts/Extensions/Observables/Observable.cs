using System;

namespace Tools.Observables
{
    public class Observable<T> : IObservable<T>
    {
        public event Action<T, T> Changed;

        public T value
        {
            get => _value;
            set
            {
                if (_value.Equals(value)) return;

                T oldValue = _value;
                _value = value;

                Changed?.Invoke(value, oldValue);
            }
        }

        private T _value;

        public Observable() => _value = default;

        public Observable(T value) => _value = value;
    }
}