//
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Microsoft.MixedReality.Toolkit.UI
{
    /// <summary>
    /// Component that plays sounds to communicate the states of a range control.
    /// </summary>
    [RequireComponent(typeof(Rotator))]
    [AddComponentMenu("Scripts/MRTK/SDK/RangeSounds")]
    public class RangeSounds : MonoBehaviour
    {
        [Header("Audio Clips")]
        [SerializeField]
        [Tooltip("Sound to play when interaction with rotator starts")]
        private AudioClip interactionStartSound = null;
        [SerializeField]
        [Tooltip("Sound to play when interaction with rotator ends")]
        private AudioClip interactionEndSound = null;

        [Header("Tick Notch Sounds")]

        [SerializeField]
        [Tooltip("Whether to play 'tick tick' sounds as the rotator passes notches")]
        private bool playTickSounds = true;

        [SerializeField]
        [Tooltip("Sound to play when rotator passes a notch")]
        private AudioClip passNotchSound = null;

        [Range(0, 1)]
        [SerializeField]
        private float tickEvery = 0.1f;

        [SerializeField]
        private float startPitch = 0.75f;

        [SerializeField]
        private float endPitch = 1.25f;

        [SerializeField]
        private float minSecondsBetweenTicks = 0.01f;


        #region Private members
        private IMixedRealityRangeControl control;

        // Play sound when passing through range notches
        private float accumulatedDeltaValue = 0;
        private float lastSoundPlayTime;

        private AudioSource grabReleaseAudioSource = null;
        private AudioSource passNotchAudioSource = null;
        #endregion

        #region Internal Methods
        private void HandleControlChange(IMixedRealityRangeControl oldControl, IMixedRealityRangeControl newControl)
        {
            if (oldControl != null)
            {
                oldControl.OnInteractionStarted.RemoveListener(OnInteractionStarted);
                oldControl.OnInteractionEnded.RemoveListener(OnInteractionEnded);
                oldControl.OnValueUpdated.RemoveListener(OnValueUpdated);
            }
            if (newControl != null)
            {
                newControl.OnInteractionStarted.AddListener(OnInteractionStarted);
                newControl.OnInteractionEnded.AddListener(OnInteractionEnded);
                newControl.OnValueUpdated.AddListener(OnValueUpdated);
            }
        }
        #endregion // Internal Methods

        #region Overrides / Event Handlers
        private void OnValueUpdated(RangeValueEventData eventData)
        {
            if (playTickSounds && passNotchAudioSource != null && passNotchSound != null)
            {
                float delta = eventData.NewValue - eventData.OldValue;
                accumulatedDeltaValue += Mathf.Abs(delta);
                var now = Time.timeSinceLevelLoad;
                if (accumulatedDeltaValue > tickEvery && now - lastSoundPlayTime > minSecondsBetweenTicks)
                {
                    passNotchAudioSource.pitch = Mathf.Lerp(startPitch, endPitch, eventData.NewValue);
                    passNotchAudioSource.PlayOneShot(passNotchSound);

                    accumulatedDeltaValue = 0;
                    lastSoundPlayTime = now;
                }
            }
        }

        private void OnInteractionEnded(RangeEventData arg0)
        {
            if (interactionEndSound != null && grabReleaseAudioSource != null)
            {
                grabReleaseAudioSource.PlayOneShot(interactionEndSound);
            }
        }

        private void OnInteractionStarted(RangeEventData arg0)
        {
            if (interactionStartSound != null && grabReleaseAudioSource != null)
            {
                grabReleaseAudioSource.PlayOneShot(interactionStartSound);
            }
        }
        #endregion // Overrides / Event Handlers

        #region Unity Overrides
        private void Start()
        {
            if (grabReleaseAudioSource == null)
            {
                grabReleaseAudioSource = gameObject.AddComponent<AudioSource>();
            }
            if (passNotchAudioSource == null)
            {
                passNotchAudioSource = gameObject.AddComponent<AudioSource>();
            }
            if (control == null)
            {
                control = GetComponent<RangeControl>();
            }
        }
        #endregion // Unity Overrides

        #region Public Properties
        /// <summary>
        /// The <see cref="IMixedRealityRangeControl"/> that sounds are being played for.
        /// </summary>
        public IMixedRealityRangeControl Control
        {
            get
            {
                return control;
            }
            set
            {
                if (control != value)
                {
                    HandleControlChange(control, value);
                    control = value;
                }
            }
        }
        #endregion // Public Properties
    }
}