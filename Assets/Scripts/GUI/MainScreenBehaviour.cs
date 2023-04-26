using UnityEngine;

public class MainScreenBehaviour : MonoBehaviour
{
    public static MainScreenBehaviour Instance { get; private set; }

    /// <summary> Screen displayed on main screen </summary>
    public Displays currentDisplay = Displays.DEFAULT;

    public enum Displays
    {
        DEFAULT,
        AIRPORTS,
        FAVOURITES,
        SETTINGS,
        FILTERS
    }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Set given display as main screen
    /// </summary>
    /// <param name="display">Display to display</param>
    public void ChangeDisplay(Displays display)
    {
        currentDisplay = display;
        if(display == Displays.FILTERS)
        {
            //UserDataCollector.Instance.filterUseCount++;
        }
        //disable all other displays
        for (int i = 0; i < transform.childCount; i++)
        {
            if(i == (int)display)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            } else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
