using UnityEngine;

[CreateAssetMenu(fileName = "Slideshow", menuName = "Weezy/Slideshow", order = 1)]

public class Slideshow : ScriptableObject
{
    /// <summary>Name of the Hotel Tour</summary>
    public string slideshowName;
    /// <summary>Array of panoramic images</summary>
    public Texture[] images;
    /// <summary>Names of image categories</summary>
    public string[] categoryNames;
    /// <summary>Images in each category by ID</summary>
    public string[] categoryImageIds;
}
