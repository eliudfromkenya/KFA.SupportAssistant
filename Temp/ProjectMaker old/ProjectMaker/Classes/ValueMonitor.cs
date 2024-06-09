using System.Collections.Generic;

namespace Pilgrims.ProjectManagement.Contracts.Classes
{
    /// <summary>
    ///     For the Value monitor <b>ValueChanged</b> event.
    /// </summary>
    /// <typeparam name="TValueType"> The type of the value in ValueMonitor class </typeparam>
    /// <param name="oldValue"> The old value, that has been overwritten </param>
    /// <param name="newValue"> The new value, that has been set </param>
    public delegate void ValueChangedDelegate<TValueType>(TValueType oldValue, TValueType newValue);

    public interface IValueMonitor<TValueType>
    {
        TValueType Value { get; }
        event ValueChangedDelegate<TValueType> ValueChanged;
    }

    /// <summary>
    ///     Monitors the value of any variable.
    ///     If the value changes by means of <b>set</b> function - the event is rised.
    ///     If set function is called with the same value - then we still quiet.
    /// </summary>
    /// <typeparam name="TValueType"></typeparam>
    public class ValueMonitor<TValueType> : IValueMonitor<TValueType>
    {
        private readonly IEqualityComparer<TValueType> _comparer;
        private TValueType _aValue;

        /// <summary>
        ///     Creates an instance of the value monitor with the operator== function as comparison function.
        /// </summary>
        /// <param name="initialValue"> The initial value of the variable. </param>
        public ValueMonitor(TValueType initialValue)
        {
            _aValue = initialValue;
        }

        /// <summary>
        ///     Creates an instance of ValueMonitor with the given comparer.
        /// </summary>
        /// <param name="initialValue">  The initial value of the variable.  </param>
        /// <param name="comparator"> The comparer object </param>
        public ValueMonitor(TValueType initialValue, IEqualityComparer<TValueType> comparator)
        {
            _aValue = initialValue;
            _comparer = comparator;
        }

        /// <summary>
        ///     Gets or sets the value of the variable.
        /// </summary>
        public TValueType Value
        {
            get { return _aValue; }
            set
            {
                var areEqual = _comparer == null ? _aValue.Equals(value) : _comparer.Equals(_aValue, value);

                if (areEqual) return;
                var oldValue = _aValue; // remember previous for the event rising
                _aValue = value;
                if (ValueChanged != null)
                    ValueChanged(oldValue, _aValue);
            }
        }

        /// <summary>
        ///     Raised, whenever the value REALLY changed
        /// </summary>
        public event ValueChangedDelegate<TValueType> ValueChanged;

        /// <summary>
        ///     "Quietly" sets the connection status.
        /// </summary>
        /// <param name="newValue"></param>
        public void SetQuietly(TValueType newValue)
        {
            _aValue = newValue;
        }
    }
}