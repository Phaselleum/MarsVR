using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "Video", menuName = "Weezy/Video", order = 1)]
public class Video : ScriptableObject
{
    public enum Format
    {
        full_360,
        half_180,
        standard
    };

    /// <summary> Format of the Video (deprecated) </summary>
    public Format format;

    /// <summary> YouTube url of the Video (others might not run or run without sound) </summary>
    public string url;
    /// <summary> Is the video local? </summary>
    public bool proprietary;
    /// <summary> Local video path (do not use in build) </summary>
    public string proprietaryFileName;
    /// <summary> VideoClip included in build </summary>
    public VideoClip localVideo;

    /// <summary> Array of CollectibleObjects appearing in the Video </summary>
    public CollectibleObject[] collectibleObjects;
}
