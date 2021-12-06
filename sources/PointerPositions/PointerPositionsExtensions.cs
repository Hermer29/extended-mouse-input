using System;

namespace Rubicks.ExtendedPointerInput
{
    public static class PointerPositionsExtensions
    {
        public static IDisposable NotifyWhen(this PointerPositions source, Predicate<PointerPositions> predicate, Action<PointerPositions> callback)
        {
            return new NotifyWhenHelper(source, predicate, callback);
        }
    }

    internal class NotifyWhenHelper : IDisposable
    {
        private PointerPositions _source;
        private Predicate<PointerPositions> _predicate;
        private Action<PointerPositions> _callback;

        public NotifyWhenHelper(PointerPositions source, Predicate<PointerPositions> predicate, Action<PointerPositions> callback)
        {
            _source = source;
            _predicate = predicate;
            _callback = callback;

            Subscribe();
        }

        private void Subscribe()
        {
            _source.PointerMoved += OnPointerMoved;
        }

        private void OnPointerMoved()
        {
            if ((bool) _predicate?.Invoke(_source))
            {
                _callback?.Invoke(_source);
            }
        }

        public void Dispose()
        {
            _source.PointerMoved -= OnPointerMoved;
            _source = null;
            _predicate = null;
            _callback = null;
            GC.Collect();
        }
    }
}
