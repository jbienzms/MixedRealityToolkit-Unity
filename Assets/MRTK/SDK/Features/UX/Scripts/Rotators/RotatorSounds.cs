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
    /// Component that plays sounds to communicate the state of a rotator
    /// </summary>
    [RequireComponent(typeof(Rotator))]
    [AddComponentMenu("Scripts/MRTK/SDK/RotatorSounds")]
    public class RotatorSounds : MonoBehaviour
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
        private Rotator rotator;

        // Play sound when passing through rotator notches
        private float accumulatedDeltaRotatorValue = 0;
        private float lastSoundPlayTime;

        private AudioSource grabReleaseAudioSource = null;
        private AudioSource passNotchAudioSource = null;
        #endregion

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
            rotator = GetComponent<Rotator>();
            rotator.OnInteractionStarted.AddListener(OnInteractionStarted);
            rotator.OnInteractionEnded.AddListener(OnInteractionEnded);
            rotator.OnValueUpdated.AddListener(OnValueUpdated);
        }

        private void OnValueUpdated(RotatorEventData eventData)
        {
            if (playTickSounds && passNotchAudioSource != null && passNotchSound != null)
            {
                float delta = eventData.NewValue - eventData.OldValue;
                accumulatedDeltaRotatorValue += Mathf.Abs(delta);
                var now = Time.timeSinceLevelLoad;
                if (accumulatedDeltaRotatorValue > tickEvery && now - lastSoundPlayTime > minSecondsBetweenTicks)
                {
                    passNotchAudioSource.pitch = Mathf.Lerp(startPitch, endPitch, eventData.NewValue);
                    passNotchAudioSource.PlayOneShot(passNotchSound);

                    accumulatedDeltaRotatorValue = 0;
                    lastSoundPlayTime = now;
                }
            }
        }

        private void OnInteractionEnded(RotatorEventData arg0)
        {
            if (interactionEndSound != null && grabReleaseAudioSource != null)
            {
                grabReleaseAudioSource.PlayOneShot(interactionEndSound);
            }
        }

        private void OnInteractionStarted(RotatorEventData arg0)
        {
            if (interactionStartSound != null && grabReleaseAudioSource != null)
            {
                grabReleaseAudioSource.PlayOneShot(interactionStartSound);
            }
        }
    }


}