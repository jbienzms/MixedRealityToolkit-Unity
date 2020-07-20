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
        #region Constants
        private const float MIN_ANGLE_LOW = -180f;
        private const float MIN_ANGLE_HIGH = 0f;
        private const float MIN_ANGLE_DEFAULT = MIN_ANGLE_LOW;
        private const float MAX_ANGLE_LOW = 0f;
        private const float MAX_ANGLE_HIGH = 180f;
        private const float MAX_ANGLE_DEFAULT = MAX_ANGLE_HIGH;
        #endregion // Constants

        #region Member Variables
        private float lastMaxAngle = MAX_ANGLE_DEFAULT;
        private float lastMinAngle = MIN_ANGLE_DEFAULT;
        private Rigidbody rigidBody;
        #endregion // Member Variables

        #region Unity Inspector Fields
        [Tooltip("The joint that will supply rotational values.")]
        [SerializeField]
        private HingeJoint joint = null;

        [SerializeField]
        [Tooltip("The maximum angle that can be rotated to.")]
        [Range(MAX_ANGLE_LOW, MAX_ANGLE_HIGH)]
        private float maxAngle = MAX_ANGLE_DEFAULT;

        [SerializeField]
        [Tooltip("The minimum angle that can be rotated to.")]
        [Range(MIN_ANGLE_LOW, MIN_ANGLE_HIGH)]
        private float minAngle = MIN_ANGLE_DEFAULT;
        #endregion // Unity Inspector Fields

        #region Internal Methods
        /// <summary>
        /// Handles the primary rotator joint being changed.
        /// </summary>
        /// <param name="oldJoint">
        /// The old joint being replaced.
        /// </param>
        /// <param name="newJoint">
        /// The new joint being assigned.
        /// </param>
        private void HandleJointChange(HingeJoint oldJoint, HingeJoint newJoint)
        {
            // Current rigid body is no longer valid
            rigidBody = null;

            // If there is a new joint
            if (newJoint != null)
            {
                // Get the new rigid body
                rigidBody = newJoint.gameObject.GetComponent<Rigidbody>();
                // ConfigurableJoint
                // Update angle limits for the joint
                UpdateLimits();
            }

            // Update the UI to reflect
            UpdateUI();
        }

        /// <summary>
        /// Handles initial setup of the joint.
        /// </summary>
        private void SetupJoint()
        {
            // If joint is not currently specified, attempt to find one in the child tree.
            if (joint == null)
            {
                Joint = GetComponentInChildren<HingeJoint>();
            }

            // If the joint is still missing, add it.
            if (joint == null)
            {
                Joint = gameObject.AddComponent<HingeJoint>();
            }
        }

        /// <summary>
        /// Updates angle limits for the joint.
        /// </summary>
        private void UpdateLimits()
        {
            if (joint != null)
            {
                JointLimits limits = new JointLimits();
                limits.min = minAngle;
                limits.max = maxAngle;
                joint.limits = limits;
                joint.useLimits = true;
            }
        }

        /// <summary>
        /// Updates UI elements to match the current value.
        /// </summary>
        private void UpdateUI()
        {
            // Only proceed if we have all required elements
            if ((joint == null) || (rigidBody == null)) { return; }

            // Get the total usable angle range
            float angleRange = maxAngle - minAngle;

            // Calculate angle along that range that represents the current value
            float angle = (Value * angleRange) + minAngle;

            // Calculate the rotation
            Quaternion rotation = Quaternion.AngleAxis(angle, joint.axis);

            //// Offset by transform
            //rotation = rotation * rigidBody.transform.rotation;

            Debug.Log($"Setting Angle: {rotation.eulerAngles}");

            // Apply the rotation to the rigid body
            rigidBody.rotation = rotation;
        }
        #endregion // Internal Methods

        #region Unity Overrides
        protected override void Start()
        {
            // Setup components
            SetupJoint();

            // Pass on to base to complete
            base.Start();
        }

        protected override void Update()
        {
            // Handle inspector changes to angles
            if ((lastMinAngle != minAngle) || (lastMaxAngle != maxAngle))
            {
                lastMinAngle = minAngle;
                lastMaxAngle = maxAngle;
                UpdateLimits();
            }

            // Pass on to base for additional updates
            base.Update();
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

        #region Public Properties
        /// <summary>
        /// Gets or sets the joint that will supply rotational values.
        /// </summary>
        public HingeJoint Joint
        {
            get
            {
                return joint;
            }
            set
            {
                if (joint != value)
                {
                    HingeJoint oldJoint = joint;
                    joint = value;
                    HandleJointChange(oldJoint, joint);
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum angle that can be rotated to.
        /// </summary>
        public float MaxAngle
        {
            get
            {
                return maxAngle;
            }
            set
            {
                if (maxAngle != value)
                {
                    if ((value < MAX_ANGLE_LOW) || (value > MAX_ANGLE_HIGH)) { throw new ArgumentOutOfRangeException(nameof(value)); }
                    maxAngle = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum angle that can be rotated to.
        /// </summary>
        public float MinAngle
        {
            get
            {
                return minAngle;
            }
            set
            {
                if (minAngle != value)
                {
                    if ((value < MIN_ANGLE_LOW) || (value > MIN_ANGLE_HIGH)) { throw new ArgumentOutOfRangeException(nameof(value)); }
                    minAngle = value;
                }
            }
        }
        #endregion // Public Properties
    }
}
