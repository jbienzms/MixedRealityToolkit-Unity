﻿//
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos
{
    [AddComponentMenu("Scripts/MRTK/Examples/SliderLunarLander")]
    public class SliderLunarLander : MonoBehaviour
    {
        [SerializeField]
        private Transform transformLandingGear = null;

        public void OnSliderUpdated(RangeValueEventData eventData)
        {
            if (transformLandingGear != null)
            {
                // Rotate the target object using Slider's eventData.NewValue
                transformLandingGear.localPosition = new Vector3(transformLandingGear.localPosition.x, 1.0f - eventData.NewValue, transformLandingGear.localPosition.z);
            }
        }
    }
}
