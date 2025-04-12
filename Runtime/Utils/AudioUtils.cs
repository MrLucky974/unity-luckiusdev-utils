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

    /// <summary>
    /// Plays a one-shot audio clip through a temporary AudioSource attached to a new GameObject.
    /// Automatically destroys the GameObject after playback.
    /// </summary>
    /// <param name="clip">The audio clip to play. Cannot be null.</param>
    /// <param name="group">The AudioMixerGroup to route the sound through. Cannot be null.</param>
    /// <param name="parent">Optional parent for the temporary GameObject (useful for positioning in 3D space).</param>
    /// <param name="pitch">Pitch modifier for the clip. Default is 1.0 (normal).</param>
    /// <param name="spatialBlend">
    /// Blend between 2D and 3D audio. 
    /// 0.0 = fully 2D, 1.0 = fully 3D. Default is 0.0 (2D).
    /// </param>
    /// <returns>The AudioSource that was used to play the clip.</returns>
    public static AudioSource Play(
        AudioClip clip,
        AudioMixerGroup group,
        Transform parent = null,
        float pitch = 1.0f,
        float spatialBlend = 0.0f)
    {
        // Validate input
        if (clip == null)
        {
            Debug.LogWarning("Play() called with null AudioClip.");
            return null;
        }

        if (group == null)
        {
            Debug.LogWarning("Play() called with null AudioMixerGroup.");
            return null;
        }

        // Create a temporary GameObject to host the AudioSource
        GameObject go = new GameObject($"AudioPlayer[{clip.name}]");

        // Optional: set parent (for positioning or organization)
        if (parent != null)
        {
            go.transform.SetParent(parent);
            go.transform.localPosition = Vector3.zero; // Reset local position
        }

        // Configure the AudioSource
        var source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.outputAudioMixerGroup = group;
        source.pitch = pitch;
        source.spatialBlend = spatialBlend;
        source.Play();

        // Automatically destroy the GameObject after the clip finishes playing
        Object.Destroy(go, clip.length);

        return source;
    }
}
