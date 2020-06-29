//
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//
using Microsoft.MixedReality.Toolkit.Input;

namespace Microsoft.MixedReality.Toolkit.UI
{
    /// <summary>
    /// Provides data for IMixedRealityRangeControl events.
    /// </summary>
    public class RangeEventData
    {
        /// <summary>
        /// Initializes a <see cref="RangeEventData"/>.
        /// </summary>
        /// <param name="control">
        /// The control that triggered the event.
        /// </param>
        public RangeEventData(IMixedRealityRangeControl control)
        {
            Control = control;
        }

        /// <summary>
        /// The control that triggered the event.
        /// </summary>
        public IMixedRealityRangeControl Control { get; private set; }
    }

    /// <summary>
    /// Provides data for IMixedRealityRangeControl value events.
    /// </summary>
    public class RangeValueEventData : RangeEventData
    {
        /// <summary>
        /// Initializes a <see cref="RangeEventData"/>.
        /// </summary>
        /// <param name="control">
        /// The control that triggered the event.
        /// </param>
        /// <param name="oldValue">
        /// The previous value of the control.
        /// </param>
        /// <param name="newValue">
        /// The current value of the control.
        /// </param>
        public RangeValueEventData(IMixedRealityRangeControl control, float oldValue, float newValue) : base(control)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// The previous value of the control.
        /// </summary>
        public float OldValue { get; private set; }

        /// <summary>
        /// The current value of the control.
        /// </summary>
        public float NewValue { get; private set; }
    }
}
