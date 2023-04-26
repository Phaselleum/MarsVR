using UnityEngine;

public class SatelliteBehaviour : MonoBehaviour
{
    void Update()
    {
        //rotate the satellite
        transform.rotation = Quaternion.Euler(new Vector3(0, -Time.deltaTime * 10, 0) + transform.eulerAngles);
    }
}
