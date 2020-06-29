// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using UnityEngine.Events;

namespace Microsoft.MixedReality.Toolkit.UI
{
    /// <summary>
    /// A UnityEvent callback containing a RangeEventData payload.
    /// </summary>
    [System.Serializable]
    public class RangeEvent : UnityEvent<RangeEventData> { }

    /// <summary>
    /// A UnityEvent callback containing a RangeValueEventData payload.
    /// </summary>
    [System.Serializable]
    public class RangeValueEvent : UnityEvent<RangeValueEventData> { }
}
