//
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos
{
    [AddComponentMenu("Scripts/MRTK/Examples/RangeChangeColor")]
    public class RangeChangeColor : MonoBehaviour
    {
        [SerializeField]
        private Renderer TargetRenderer;

        public void OnRangeUpdatedRed(RangeValueEventData eventData)
        {
            TargetRenderer = GetComponentInChildren<Renderer>();
            if ((TargetRenderer != null) && (TargetRenderer.material != null))
            {
                TargetRenderer.material.color = new Color(eventData.NewValue, TargetRenderer.sharedMaterial.color.g, TargetRenderer.sharedMaterial.color.b);
            }
        }

        public void OnRangeUpdatedGreen(RangeValueEventData eventData)
        {
            TargetRenderer = GetComponentInChildren<Renderer>();
            if ((TargetRenderer != null) && (TargetRenderer.material != null))
            {
                TargetRenderer.material.color = new Color(TargetRenderer.sharedMaterial.color.r, eventData.NewValue, TargetRenderer.sharedMaterial.color.b);
            }
        }

        public void OnRangeUpdateBlue(RangeValueEventData eventData)
        {
            TargetRenderer = GetComponentInChildren<Renderer>();
            if ((TargetRenderer != null) && (TargetRenderer.material != null))
            {
                TargetRenderer.material.color = new Color(TargetRenderer.sharedMaterial.color.r, TargetRenderer.sharedMaterial.color.g, eventData.NewValue);
            }
        }
    }
}
