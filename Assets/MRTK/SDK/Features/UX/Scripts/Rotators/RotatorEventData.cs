//
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//
using Microsoft.MixedReality.Toolkit.Input;

namespace Microsoft.MixedReality.Toolkit.UI
{
    public class RotatorEventData
    {
        public RotatorEventData(float o, float n, IMixedRealityPointer pointer, Rotator rotator)
        {
            OldValue = o;
            NewValue = n;
            Pointer = pointer;
            Rotator = rotator;
        }

        /// <summary>
        /// The previous value of the rotator
        /// </summary>
        public float OldValue { get; private set; }

        /// <summary>
        /// The current value of the rotator
        /// </summary>
        public float NewValue { get; private set; }

        /// <summary>
        /// The rotator that triggered this event
        /// </summary>
        public Rotator Rotator { get; private set; }

        /// <summary>
        /// The currently active pointer manipulating / hovering the rotator,
        /// or null if no pointer is manipulating the rotator.
        /// Note: OnSliderUpdated is called with .Pointer == null
        /// OnStart, so always check if this field is null before using!
        /// </summary>
        public IMixedRealityPointer Pointer { get; set; }
    }
}
