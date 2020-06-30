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
    /// The interface for a control that provides a ranged value.
    /// </summary>
    public interface IMixedRealityRangeControl
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the current value of the control.
        /// </summary>
        float Value { get; set; }
        #endregion // Public Properties

        #region Unity Events
        /// <summary>
        /// Raised when the user's hand or controller hovers over the control.
        /// </summary>
        /// <remarks>
        /// This event is raised for both near and far interactions.
        /// </remarks>
        RangeEvent OnHoverEntered { get; }

        /// <summary>
        /// Raised when the user's hand or controller is no longer over the control.
        /// </summary>
        /// <remarks>
        /// This event is raised for both near and far interactions.
        /// </remarks>
        RangeEvent OnHoverExited { get; }

        /// <summary>
        /// Raised when the user begins interacting with the control.
        /// </summary>
        /// <remarks>
        /// For example, this event is raised when the user grabs the thumb of the
        /// <see cref="PinchSlider"/> control.
        /// </remarks>
        RangeEvent OnInteractionStarted { get; }

        /// <summary>
        /// Called when the user is no longer interacting with the control.
        /// </summary>
        /// <remarks>
        /// For example, this event is raised when the user releases the thumb of the
        /// <see cref="PinchSlider"/> control.
        /// </remarks>
        RangeEvent OnInteractionEnded { get; }

        /// <summary>
        /// Raised whenever the <see cref="Value"/> property changes.
        /// </summary>
        RangeValueEvent OnValueUpdated { get; }
        #endregion // Unity Events
    }

    /// <summary>
    /// The base class for a control that provides a ranged value.
    /// </summary>
    public abstract class RangeControl : MonoBehaviour, IMixedRealityRangeControl, IMixedRealityPointerHandler, IMixedRealityFocusHandler
    {
        #region Constants
        private const float VALUE_MIN = 0.0f;
        private const float VALUE_MAX = 1.0f;
        private const float VALUE_DEFAULT = VALUE_MIN;
        #endregion // Constants

        #region Member Variables
        private float lastValue = VALUE_DEFAULT;
        #endregion // Member Variables

        #region Unity Inspector Fields
        [Header("Range", order=0)]
        [Tooltip("The starting value of the control.")]
        [Range(VALUE_MIN, VALUE_MAX)]
        [SerializeField]
        private float value = VALUE_DEFAULT;
        #endregion // Unity Inspector Fields

        #region Internal Methods
        /// <summary>
        /// Ends any current interactions.
        /// </summary>
        protected virtual void EndInteraction()
        {
            On_InteractionEnded();
            Pointer = null;
        }
        #endregion // Internal Methods

        #region Unity Overrides
        protected virtual void Start()
        {
            // Always notify the current value on start.
            On_ValueUpdated(value, value);
        }

        protected virtual void OnDisable()
        {
            if (Pointer != null)
            {
                EndInteraction();
            }
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnValidate()
        {
        }

        protected virtual void Update()
        {
            // Handle inspector changes
            if (lastValue != value)
            {
                On_ValueUpdated(lastValue, value);
                lastValue = value;
            }
        }
        #endregion // Unity Overrides

        #region Overridables / Event Triggers
        /// <see cref="IMixedRealityFocusHandler.OnFocusEnter"/>.
        protected virtual void On_FocusEnter(FocusEventData eventData)
        {
            On_HoverEntered();
        }

        /// <see cref="IMixedRealityFocusHandler.OnFocusExit"/>.
        protected virtual void On_FocusExit(FocusEventData eventData)
        {
            On_HoverExited();
        }

        /// <summary>
        /// Called when the user's hand or controller hovers over the control.
        /// </summary>
        protected virtual void On_HoverEntered()
        {
            OnHoverEntered.Invoke(new RangeEventData(this));
        }

        /// <summary>
        /// Called when the user's hand or controller is no longer over the control.
        /// </summary>
        protected virtual void On_HoverExited()
        {
            OnHoverExited.Invoke(new RangeEventData(this));
        }

        /// <summary>
        /// Called when the user begins interacting with the control.
        /// </summary>
        protected virtual void On_InteractionStarted()
        {
            OnInteractionStarted.Invoke(new RangeEventData(this));
        }

        /// <summary>
        /// Called when the user is no longer interacting with the control.
        /// </summary>
        protected virtual void On_InteractionEnded()
        {
            OnInteractionEnded.Invoke(new RangeEventData(this));
        }

        /// <see cref="IMixedRealityPointerHandler.OnPointerClicked"/>
        protected virtual void On_PointerClicked(MixedRealityPointerEventData eventData)
        {
            // Leave up to each inheriting class to decide whether or not to process this event
        }

        /// <see cref="IMixedRealityPointerHandler.OnPointerDown"/>
        protected virtual void On_PointerDown(MixedRealityPointerEventData eventData)
        {
            if (Pointer == null && !eventData.used)
            {
                Pointer = eventData.Pointer;
                On_InteractionStarted();
                eventData.Use();
            }
        }

        /// <see cref="IMixedRealityPointerHandler.OnPointerDragged"/>
        protected virtual void On_PointerDragged(MixedRealityPointerEventData eventData)
        {
            // Leave up to each inheriting class to decide whether or not to process this event
        }

        /// <see cref="IMixedRealityPointerHandler.OnPointerUp"/>
        protected virtual void On_PointerUp(MixedRealityPointerEventData eventData)
        {
            if (eventData.Pointer == Pointer && !eventData.used)
            {
                EndInteraction();
                eventData.Use();
            }
        }

        /// <summary>
        /// Called whenever the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="oldValue">
        /// The old value.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        protected virtual void On_ValueUpdated(float oldValue, float newValue)
        {
            OnValueUpdated.Invoke(new RangeValueEventData(this, oldValue, newValue));
        }
        #endregion // Overridables / Event Triggers

        #region IMixedRealityFocusHandler
        /// <inheritdoc/>
        void IMixedRealityFocusHandler.OnFocusEnter(FocusEventData eventData)
        {
            On_FocusEnter(eventData);
        }

        /// <inheritdoc/>
        void IMixedRealityFocusHandler.OnFocusExit(FocusEventData eventData)
        {
            On_FocusExit(eventData);
        }
        #endregion

        #region IMixedRealityPointerHandler
        /// <inheritdoc/>
        void IMixedRealityPointerHandler.OnPointerClicked(MixedRealityPointerEventData eventData)
        {
            On_PointerClicked(eventData);
        }

        /// <inheritdoc/>
        void IMixedRealityPointerHandler.OnPointerDown(MixedRealityPointerEventData eventData)
        {
            On_PointerDown(eventData);
        }

        /// <inheritdoc/>
        void IMixedRealityPointerHandler.OnPointerDragged(MixedRealityPointerEventData eventData)
        {
            On_PointerDragged(eventData);
        }

        /// <inheritdoc/>
        void IMixedRealityPointerHandler.OnPointerUp(MixedRealityPointerEventData eventData)
        {
            On_PointerUp(eventData);
        }
        #endregion

        #region IMixedRealityRangeControl
        RangeEvent IMixedRealityRangeControl.OnHoverEntered => OnHoverEntered;
        RangeEvent IMixedRealityRangeControl.OnHoverExited => OnHoverExited;
        RangeEvent IMixedRealityRangeControl.OnInteractionStarted => OnInteractionStarted;
        RangeEvent IMixedRealityRangeControl.OnInteractionEnded => OnInteractionEnded;
        RangeValueEvent IMixedRealityRangeControl.OnValueUpdated => OnValueUpdated;
        #endregion // IMixedRealityRangeControl

        #region Public Properties
        /// <summary>
        /// Gets the currently active pointer manipulating or hovering over the control.
        /// </summary>
        public IMixedRealityPointer Pointer { get; protected set; }

        /// <inheritdoc/>
        public float Value
        {
            get { return value; }
            set
            {
                if (this.value != value)
                {
                    var oldValue = this.value;
                    this.value = value;
                    On_ValueUpdated(oldValue, value);
                }
            }
        }
        #endregion // Public Properties

        #region Unity Events
        [Header("Events", order = 255)]
        /// <summary>
        /// Raised when the user's hand or controller hovers over the control.
        /// </summary>
        /// <remarks>
        /// This event is raised for both near and far interactions.
        /// </remarks>
        public RangeEvent OnHoverEntered = new RangeEvent();

        /// <summary>
        /// Raised when the user's hand or controller is no longer over the control.
        /// </summary>
        /// <remarks>
        /// This event is raised for both near and far interactions.
        /// </remarks>
        public RangeEvent OnHoverExited = new RangeEvent();

        /// <summary>
        /// Raised when the user begins interacting with the control.
        /// </summary>
        /// <remarks>
        /// For example, this event is raised when the user grabs the thumb of the
        /// <see cref="PinchSlider"/> control.
        /// </remarks>
        public RangeEvent OnInteractionStarted = new RangeEvent();

        /// <summary>
        /// Called when the user is no longer interacting with the control.
        /// </summary>
        /// <remarks>
        /// For example, this event is raised when the user releases the thumb of the
        /// <see cref="PinchSlider"/> control.
        /// </remarks>
        public RangeEvent OnInteractionEnded = new RangeEvent();

        /// <summary>
        /// Raised whenever the <see cref="Value"/> property changes.
        /// </summary>
        public RangeValueEvent OnValueUpdated = new RangeValueEvent();
        #endregion // Unity Events
    }
}
