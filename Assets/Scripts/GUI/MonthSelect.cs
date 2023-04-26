using UnityEngine;
using UnityEngine.UI;

public class MonthSelect : MonoBehaviour
{
    /// <summary>The associated season</summary>
    public FilterBehaviour.Months month;

    /// <summary> Is this month currently selected? </summary>
    public bool isSelected;

    /// <summary> Default Colour option used to colour the button </summary>
    private ColorBlock colors;
    /// <summary> Selected Colour option used to colour the button </summary>
    private ColorBlock selColors;

    /// <summary> Array of max temperature value Text components in climate graph </summary>
    public Text[] maxTempTexts;
    /// <summary> Array of min temperature value Text components in climate graph </summary>
    public Text[] minTempTexts;
    /// <summary> Array of precipitation value Text components in climate graph </summary>
    public Text[] precipTexts;

    private void Awake()
    {
        //System.DateTime before = System.DateTime.Now;

        //set default colours
        colors = new ColorBlock();
        colors.disabledColor = new Color(.5f, .5f, .5f, 1);
        colors.normalColor = new Color(0, .05f, .85f);
        colors.highlightedColor = new Color(.1f, .15f, 1);
        colors.pressedColor = new Color(.15f, .175f, 1);
        colors.selectedColor = new Color(.3f, .4f, 1);
        colors.colorMultiplier = 1;
        colors.fadeDuration = .1f;

        selColors.disabledColor = new Color(.5f, .5f, .5f, 1);
        selColors.normalColor = new Color(1, .5f, .5f, 1);
        selColors.highlightedColor = new Color(1, .5f, .5f, 1);
        selColors.pressedColor = new Color(1, .5f, .5f, 1);
        selColors.selectedColor = new Color(1, .5f, .5f, 1);
        selColors.colorMultiplier = 1;
        selColors.fadeDuration = .1f;

        //Debug.Log(System.DateTime.Now.Subtract(before).Milliseconds);
    }

    private void Start()
    {
        GetComponent<Button>().colors = colors;
        HighlightClimateMonth();
    }

    private void OnMouseDown()
    {
        //mark the current month as selected
        FilterBehaviour.Instance.SetMonth(month);
        MonthSelect[] monthselect = transform.parent.GetComponentsInChildren<MonthSelect>();
        foreach (MonthSelect ms in monthselect)
        {
            ms.isSelected = false;
        }
        isSelected = true;

        Button[] buttons = transform.parent.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.colors = colors;
        }
        GetComponent<Image>().color = new Color(1, 1, 1, 1);
        GetComponent<Button>().colors = selColors;

        HighlightClimateMonth();
    }

    /// <summary>
    /// Call from button press, imitates OnMouseDown
    /// </summary>
    public void Select()
    {
        OnMouseDown();
    }

    /// <summary>
    /// Highlight the selected month in the climate graph
    /// </summary>
    private void HighlightClimateMonth()
    {
        foreach (Text text in minTempTexts) text.fontStyle = FontStyle.Normal;
        foreach (Text text in maxTempTexts) text.fontStyle = FontStyle.Normal;
        foreach (Text text in precipTexts) text.fontStyle = FontStyle.Normal;
        foreach (Text text in minTempTexts) text.color = new Color(0.4431373f, 0.7568628f, 0.9294118f);
        foreach (Text text in maxTempTexts) text.color = new Color(1, 0.3686275f, 0);
        foreach (Text text in precipTexts) text.color = new Color(0.22201f, 0.247611f, 0.745f);
        minTempTexts[(int)month].fontStyle = FontStyle.Bold;
        maxTempTexts[(int)month].fontStyle = FontStyle.Bold;
        precipTexts[(int)month].fontStyle = FontStyle.Bold;
        minTempTexts[(int)month].color = new Color(1, 1, 1);
        maxTempTexts[(int)month].color = new Color(1, 1, 1);
        precipTexts[(int)month].color = new Color(1, 1, 1);
    }
}
