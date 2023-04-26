using UnityEngine;
using System;

/// <summary>
/// Holds methods to calculate from earth to world coordinates
/// </summary>
public class GeoLocator {

    /// <summary>
    /// Converts a given cartesian vector to polar coordinates
    /// </summary>
    /// <param name="point">the given cartesian vector</param>
    /// <returns>The transformed polar coordinates</returns>
	public Vector2 CartesianToPolar(Vector3 point)
	{
		Vector2 polar = new Vector2();

		//calc longitude
		polar.y = Mathf.Atan2(point.x,point.z);

		//this is easier to write and read than sqrt(pow(x,2), pow(y,2))!
		float xzLen;
		xzLen = new Vector2(point.x,point.z).magnitude; 
		//atan2 does the magic
		polar.x = Mathf.Atan2(-point.y,xzLen);

		//convert to deg
		polar *= Mathf.Rad2Deg;

		return polar;
	}

    /// <summary>
    /// Converts given polar coordinates to a carestian vector
    /// </summary>
    /// <param name="polar">the given polar coordinates</param>
    /// <returns>The transformed cartesian vector</returns>
	public Vector3 PolarToCartesian(Vector2 polar)
	{
		Vector3 origin = new Vector3(0,0,1);
		//build a quaternion using euler angles for lat,lon
		Quaternion rotation = Quaternion.Euler(polar.x,polar.y,0);
		//transform our reference vector by the rotation. Easy-peasy!
		Vector3 point = rotation * origin;
		return point;
	}

    /// <summary>
    /// Returns a world vector given a double latitude and a double longitude based on a sphere with a given radius
    /// </summary>
    /// <param name="myradius">the radius of the sphere</param>
    /// <param name="mylatitude">the latitude of the given coordinates</param>
    /// <param name="mylongitude">the longitude of the given coordinates</param>
    /// <returns>world vector</returns>
	public Vector3 GetVectorFromLatLong(float myradius, double mylatitude, double mylongitude) {
		mylatitude = Mathf.PI  * mylatitude / 180f;
		mylongitude = Mathf.PI  * mylongitude / 180;
		//mylongitude = Mathf.PI  * mylongitude / -180f;
		float degrees90 = 1.570795765134f;
		mylatitude -= (degrees90 * 1); // subtract 90 degrees (in radians)
		mylongitude -= (degrees90 * 1); // subtract 90 degrees (in radians)

		double xPos = (myradius) * ((Math.Sin((mylatitude))) * (Math.Cos((mylongitude))));
		double zPos = (myradius) * (Math.Sin((mylatitude)) * Math.Sin((mylongitude)));
		double yPos = (myradius) * Math.Cos((mylatitude)); 

		// move marker to position
		return new Vector3(System.Convert.ToSingle(xPos),System.Convert.ToSingle(yPos),System.Convert.ToSingle(zPos));
    }

    /// <summary>
    /// Returns a world vector given a float latitude and a float longitude based on a sphere with a given radius
    /// </summary>
    /// <param name="myradius">the radius of the sphere</param>
    /// <param name="mylatitude">the latitude of the given coordinates</param>
    /// <param name="mylongitude">the longitude of the given coordinates</param>
    /// <returns>world vector</returns>
	public Vector3 ConvertSphericalToCartesian(float myradius, float mylatitude, float mylongitude)
	{
		float lat = DegtoRad(mylatitude);
		float lon = DegtoRad(mylongitude);

		var x = myradius * Mathf.Cos(lat)*Mathf.Cos(lon);
		var y = myradius * Mathf.Cos(lat)*Mathf.Sin(lon);
		var z = myradius * Mathf.Sin(lat);
		return new Vector3(x,y,z);
	}

    /// <summary>
    /// Degrees to radians
    /// </summary>
    /// <param name="x">Given angle in degrees</param>
    /// <returns>angle in radians</returns>
	private float DegtoRad(float x){
		return x*Mathf.PI/180;
	}

    /// <summary>
    /// Radians to degrees
    /// </summary>
    /// <param name="x">Given angle in radians</param>
    /// <returns>angle in degrees</returns>
    private float RadtoDeg(float x){
		return x*180/Mathf.PI;
	}

	/// <summary>
	/// Returns the distance between two points over the surface of an earth-sized sphere, given geodetic coordinates (latitudes/longitudes)
	/// </summary>
	/// <param name="lat1">Latitude of starting point</param>
	/// <param name="long1">Longitude of starting point</param>
	/// <param name="lat1">Latitude of end point</param>
	/// <param name="long1">Longitude of end point</param>
	/// <returns>distance in km</returns>
	public static double GeodeticDistance(double lat1, double long1, double lat2, double long2)
    {
		double r = 6371e3;
		double lat1Rad = lat1 * Math.PI / 180;
		double lat2Rad = lat2 * Math.PI / 180;
		double deltaLat = lat1Rad - lat2Rad;
		double deltaLong = (long1 - long2) * Math.PI / 180;
		double a = Math.Sin(deltaLat * .5f) * Math.Sin(deltaLat * .5f)
			+ Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Sin(deltaLong * .5f) * Math.Sin(deltaLong * .5f);
		double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
		return r * c;
    }

	/// <summary>
	/// Returns a normalized position vector given geodetic coordinates (latitudes/longitudes)
	/// </summary>
	/// <param name="latitude">Latitude of the given point</param>
	/// <param name="longitude">Longitude of the given point</param>
	/// <returns>Vector3 of the point (normalized)</returns>
	public static Vector3 GeodeticToVector3(double latitude, double longitude)
    {
		return new Vector3(
			(float)(Math.Cos(latitude * Math.PI / 180) * Math.Sin(-longitude * Math.PI / 180)),
			(float)(Math.Sin(latitude * Math.PI / 180)),
			(float)(Math.Cos(latitude * Math.PI / 180) * Math.Cos(-longitude * Math.PI / 180)));
	}
}
