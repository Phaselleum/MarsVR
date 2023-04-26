using UnityEngine;
using UnityEngine.UI;

public class MapBehaviour : MonoBehaviour
{
    public Slider zoomSlider;

    /// <summary>
    /// Move map Image by a given Vector
    /// </summary>
    /// <param name="dir">Direction to move to</param>
    public void MoveMap(Vector2 dir)
    {
        transform.localPosition = new Vector3(Mathf.Max(-100, Mathf.Min(100, transform.localPosition.x + dir.x)), 
            Mathf.Max(-100, Mathf.Min(100, transform.localPosition.y + dir.y)));
    }

    /// <summary>
    /// Move map up by a set amount
    /// </summary>
    public void MoveMapUp()
    {
        MoveMap(new Vector2(0, -25));
    }

    /// <summary>
    /// Move map down by a set amount
    /// </summary>
    public void MoveMapDown()
    {
        MoveMap(new Vector2(0, 25));
    }

    /// <summary>
    /// Move map left by a set amount
    /// </summary>
    public void MoveMapLeft()
    {
        MoveMap(new Vector2(25, 0));
    }

    /// <summary>
    /// Move map right by a set amount
    /// </summary>
    public void MoveMapRight()
    {
        MoveMap(new Vector2(-25, 0));
    }

    /// <summary>
    /// Return map to original position
    /// </summary>
    public void CenterMap()
    {
        transform.localPosition = new Vector3(0, 0, -.25f);
    }

    /// <summary>
    /// Zoom map in
    /// </summary>
    public void ZoomMapIn()
    {
        transform.localScale += Vector3.one * .5f;
    }

    /// <summary>
    /// Zoom map out
    /// </summary>
    public void ZoomMapOut()
    {
        transform.localScale -= Vector3.one * .5f;
    }

    public void SetMapZoom()
    {
        transform.localScale = Vector3.one * (2 + .5f * zoomSlider.value);
    }

    /// <summary>
    /// Reset map zoom
    /// </summary>
    public void ZoomNeutral()
    {
        transform.localScale = Vector3.one * 2;
        zoomSlider.SetValueWithoutNotify(0);
    }
}
