using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioUtils
{
    // Minimum dB value to avoid -∞ when normalized is 0
    private const float MinDb = -80.0f;

    /// <summary>
    /// Converts a dB value to a normalized 0-1 range.
    /// </summary>
    /// <param name="db">The dB value to convert.</param>
    /// <returns>A float value in the range 0-1 representing the normalized volume.</returns>
    public static float DbToNormalized(float db)
    {
        // Clamp dB to avoid values lower than the minimum (silence)
        db = Mathf.Clamp(db, MinDb, 0);
        return Mathf.Pow(10, db / 20.0f);
    }

    /// <summary>
    /// Converts a normalized 0-1 value to a dB value.
    /// </summary>
    /// <param name="normalized">The normalized value to convert, in the range 0-1.</param>
    /// <returns>A float representing the value in dB.</returns>
    public static float NormalizedToDb(float normalized)
    {
        // Clamp normalized value to avoid -∞ at 0
        normalized = Mathf.Clamp(normalized, Mathf.Epsilon, 1.0f);
        return 20.0f * Mathf.Log10(normalized);
    }

    public static AudioSource Play( AudioClip clip, AudioMixerGroup group, Transform parent = null, float pitch = 1.0f, float spatialBlend = 0.0f ) {
        GameObject go = new GameObject($"AudioPlayer[{clip.name}]")
        {
            transform =
            {
                parent = parent
            }
        };

        var source = go.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = group;
        source.spatialBlend = spatialBlend;
        source.pitch = pitch;
        source.clip = clip;
        source.Play();
        
        Object.Destroy(go, clip.length);

        return source;
    }
}
