//
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//

using Microsoft.MixedReality.Toolkit.Input;
using System;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.UI
{
    /// <summary>
    /// A value control whose value comes from the rotation of a control surface.
    /// </summary>
    [HelpURL("https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/README_Rotators.html")]
    [AddComponentMenu("Scripts/MRTK/SDK/Rotator")]
    public class Rotator : RangeControl
    {
        #region Member Variables
        #endregion // Member Variables

        #region Unity Inspector Fields
        [Tooltip("The GameObject that represents the grip.")]
        [SerializeField]
        private GameObject gripRoot = null;
        /// <summary>
        /// Gets or sets the GameObject that represents the grip.
        /// </summary>
        public GameObject GripRoot { get { return gripRoot; } set { gripRoot = value; } }

        [Header("Visuals")]

        [Tooltip("The gameObject that contains the tickMarks.  This will get rotated to match the slider axis")]
        [SerializeField]
        private GameObject tickMarks = null;
        /// <summary>
        /// Gets or sets the GameObject that contains the desired tick marks.
        /// </summary>
        public GameObject TickMarks
        {
            get
            {
                return tickMarks;
            }
            set
            {
                if (tickMarks != value)
                {
                    tickMarks = value;
                    UpdateTickMarks();
                }
            }
        }
        #endregion // Unity Inspector Fields

        #region Internal Methods
        private void InitializeGrip()
        {
            //var startToThumb = gripRoot.transform.position - SliderStartPosition;
            //var thumbProjectedOnTrack = SliderStartPosition + Vector3.Project(startToThumb, SliderTrackDirection);
            //sliderThumbOffset = gripRoot.transform.position - thumbProjectedOnTrack;

            UpdateUI();
        }

        /// <summary>
        /// Update orientation of tick marks based on slider axis orientation
        /// </summary>
        private void UpdateTickMarks()
        {
            /*
            if (TickMarks)
            {
                TickMarks.transform.localPosition = Vector3.zero;
                TickMarks.transform.localRotation = Quaternion.identity;

                var grid = TickMarks.GetComponent<Utilities.GridObjectCollection>();
                if (grid)
                {
                    // Update cellwidth or cellheight depending on what was the previous axis set to
                    var previousAxis = grid.Layout;
                    if (previousAxis == Utilities.LayoutOrder.Vertical)
                    {
                        grid.CellWidth = grid.CellHeight;
                    }
                    else
                    {
                        grid.CellHeight = grid.CellWidth;
                    }

                    grid.Layout = (sliderAxis == SliderAxis.YAxis) ? Utilities.LayoutOrder.Vertical : Utilities.LayoutOrder.Horizontal;
                    grid.UpdateCollection();
                }

                if (sliderAxis == SliderAxis.ZAxis)
                {
                    TickMarks.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                }
            }
            */
        }

        private void UpdateUI()
        {
            /*
            var newSliderPos = SliderStartPosition + sliderThumbOffset + SliderTrackDirection * value;
            gripRoot.transform.position = newSliderPos;
            */
        }
        #endregion // Internal Methods

        #region Unity Overrides
        protected override void Start()
        {
            if (gripRoot == null)
            {
                Debug.LogError($"{nameof(GripRoot)} on {gameObject.name} is not specified. {nameof(Rotator)} will be disabled.");
                enabled = false;
                return;
            }

            // Pass on to base to complete
            base.Start();
        }
        #endregion // Unity Overrides

        #region Overrides / Event Handlers
        /// <inheritdoc/>
        protected override void On_ValueUpdated(float oldValue, float newValue)
        {
            // Allow base to process first
            base.On_ValueUpdated(oldValue, newValue);

            // Update the UI
            UpdateUI();
        }
        #endregion // Overrides / Event Handlers
    }
}
