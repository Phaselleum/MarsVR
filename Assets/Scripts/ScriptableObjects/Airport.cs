using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Airport", menuName = "Weezy/Airport", order = 1)]
public class Airport : ScriptableObject
{
    /// <summary> Name of the airport </summary>
    [FormerlySerializedAs("name")]
    public string airportName;
    /// <summary> 3-Letter IATA code of the airport </summary>
    public string IATACode;
    /// <summary> Latitude of the airport </summary>
    public double latitude;
    /// <summary> Longitude of the airport </summary>
    public double longitude;
    public EarthEngineCity city;
}
