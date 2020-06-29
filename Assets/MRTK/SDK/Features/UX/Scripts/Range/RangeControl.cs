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
        #region Unity Inspector Fields
        [Header("Range")]
        [Tooltip("The starting value of the control.")]
        [Range(0, 1)]
        [SerializeField]
        private float value = 0.0f;
        #endregion // Unity Inspector Fields

        #region Unity Events
        [Header("Events")]
        [SerializeField]
        private RangeEvent onHoverEntered = new RangeEvent();
        [SerializeField]
        private RangeEvent onHoverExited = new RangeEvent();
        [SerializeField]
        private RangeEvent onInteractionStarted = new RangeEvent();
        [SerializeField]
        private RangeEvent onInteractionEnded = new RangeEvent();
        [SerializeField]
        private RangeValueEvent onValueUpdated = new RangeValueEvent();
        #endregion // Unity Events

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
            onHoverEntered.Invoke(new RangeEventData(this));
        }

        /// <summary>
        /// Called when the user's hand or controller is no longer over the control.
        /// </summary>
        protected virtual void On_HoverExited()
        {
            onHoverExited.Invoke(new RangeEventData(this));
        }

        /// <summary>
        /// Called when the user begins interacting with the control.
        /// </summary>
        protected virtual void On_InteractionStarted()
        {
            onInteractionStarted.Invoke(new RangeEventData(this));
        }

        /// <summary>
        /// Called when the user is no longer interacting with the control.
        /// </summary>
        protected virtual void On_InteractionEnded()
        {
            onInteractionEnded.Invoke(new RangeEventData(this));
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
            onValueUpdated.Invoke(new RangeValueEventData(this, value, value));
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

        #region Events
        /// <inheritdoc/>
        public RangeEvent OnHoverEntered => onHoverEntered;

        /// <inheritdoc/>
        public RangeEvent OnHoverExited => onHoverExited;

        /// <inheritdoc/>
        public RangeEvent OnInteractionStarted => onInteractionStarted;

        /// <inheritdoc/>
        public RangeEvent OnInteractionEnded => onInteractionEnded;

        /// <inheritdoc/>
        public RangeValueEvent OnValueUpdated => onValueUpdated;
        #endregion // Events
    }
}
