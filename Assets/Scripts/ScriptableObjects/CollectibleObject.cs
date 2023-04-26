using UnityEngine;

[CreateAssetMenu(fileName = "Collectible", menuName = "Weezy/Collectible Object", order = 1)]

public class CollectibleObject : ScriptableObject
{
    /// <summary> Name of the Collectible Object </summary>
    public string objectName;
    /// <summary> ID of the Collectible Object </summary>
    public string objectId;
    /// <summary> Ttime to start displaying the object in-video </summary>
    public float videoDisplayStart;
    /// <summary> Time to end displaying the object in-video </summary>
    public float videoDisplayEnd;
    /// <summary> Prefab of the object to be displayed in-video </summary>
    public GameObject gameObject;
}
