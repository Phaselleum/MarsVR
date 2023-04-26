using UnityEngine;
using UnityEngine.UI;

public class PriceSelect : MonoBehaviour
{
    /// <summary>Destination filter price slider</summary>
    public Slider slider;
    /// <summary>Dropdown of number of guests</summary>
    public Dropdown guestsNo;

    /// <summary>
    /// Sets the filter price on slider value change
    /// </summary>
    public void Select()
    {
        FilterBehaviour.Instance.SetPrice((int)slider.value);
    }

    /// <summary>
    /// Lower the filter price by a set step (rounded to 50)
    /// </summary>
    public void LessPrice()
    {
        slider.value = slider.value - 1;
        FilterBehaviour.Instance.SetPrice((int)slider.value);
    }

    /// <summary>
    /// Increase the filter price by a set step (rounded to 50)
    /// </summary>
    public void MorePrice()
    {
        slider.value = slider.value + 1;
        FilterBehaviour.Instance.SetPrice((int)slider.value);
    }

    /// <summary>
    /// Sets the number of guests in budget mode on Dropdown select
    /// </summary>
    public void ChangeGuestsNo()
    {
        TravelOverviewBehaviour.Instance.ChangeGuestsNo(guestsNo.value);
    }
}
