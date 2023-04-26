using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Attraction", menuName = "Weezy/Attraction", order = 1)]
public class Attraction : ScriptableObject
{
    /// <summary> Name of the attraction </summary>
    [FormerlySerializedAs("name")]
    public string attractionName;
    /// <summary> 5-letter shorthand name of the attraction for map markers </summary>
    public string shortName;
    /// <summary> Latitude of the attraction </summary>
    public double latitude;
    /// <summary> Longitude of the attraction </summary>
    public double longitude;
}
