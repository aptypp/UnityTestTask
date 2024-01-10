using System;

namespace TestTask.Extensions.Observables
{
    public interface IObservable<out T>
    {
        event Action<T, T> Changed;
        
        T value { get; }
    }
}