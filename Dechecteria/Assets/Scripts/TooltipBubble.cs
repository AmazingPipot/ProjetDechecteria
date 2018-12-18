using UnityEngine;
using UnityEngine.UI;

public class TooltipBubble : MonoBehaviour
{
    public Text bubbleText;

    public enum TipPosition { TOP, BOTTOM, LEFT, RIGHT, TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT };

    [Header("Bubble Tips positions")]
    public Image tipLeft;
    public Image tipRight;
    public Image tipTop;
    public Image tipBottom;

    public GameObject inputCatcher;
    private RectTransform rectTransform;

    public void Init(string text, TipPosition tipPosition, float x, float y, float width = 320, bool closeOnInputCatcherClick = false)
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);

        this.tipLeft.gameObject.SetActive(tipPosition == TipPosition.LEFT);
        this.tipRight.gameObject.SetActive(tipPosition == TipPosition.RIGHT);
        this.tipTop.gameObject.SetActive(tipPosition == TipPosition.TOP || tipPosition == TipPosition.TOP_LEFT || tipPosition == TipPosition.TOP_RIGHT);
        this.tipBottom.gameObject.SetActive(tipPosition == TipPosition.BOTTOM || tipPosition == TipPosition.BOTTOM_LEFT  || tipPosition == TipPosition.BOTTOM_RIGHT);

        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, 0);

        this.bubbleText.text = text;
        
        switch(tipPosition)
        {
            case TipPosition.TOP:
                rectTransform.pivot = new Vector2(0.5f, 1.0f);
                tipTop.rectTransform.anchoredPosition = new Vector3(0.0f, tipTop.rectTransform.localPosition.y, tipTop.rectTransform.localPosition.z);
                break;
            case TipPosition.TOP_RIGHT:
                rectTransform.pivot = new Vector2(0.75f, 1.0f);
                tipTop.rectTransform.anchoredPosition = new Vector3(width / 4.0f, tipTop.rectTransform.localPosition.y, tipTop.rectTransform.localPosition.z);
                break;
            case TipPosition.TOP_LEFT:
                rectTransform.pivot = new Vector2(0.25f, 1.0f);
                tipTop.rectTransform.anchoredPosition = new Vector3(width / -4.0f, tipTop.rectTransform.localPosition.y, tipTop.rectTransform.localPosition.z);
                break;
            case TipPosition.BOTTOM:
                rectTransform.pivot = new Vector2(0.5f, 0.0f);
                tipBottom.rectTransform.anchoredPosition = new Vector3(0.0f, tipBottom.rectTransform.localPosition.y, tipBottom.rectTransform.localPosition.z);
                break;
            case TipPosition.BOTTOM_RIGHT:
                rectTransform.pivot = new Vector2(0.75f, 0.0f);
                tipBottom.rectTransform.anchoredPosition = new Vector3(width / 4.0f, tipBottom.rectTransform.localPosition.y, tipBottom.rectTransform.localPosition.z);
                break;
            case TipPosition.BOTTOM_LEFT:
                rectTransform.pivot = new Vector2(0.25f, 0.0f);
                tipBottom.rectTransform.anchoredPosition = new Vector3(width / -4.0f, tipBottom.rectTransform.localPosition.y, tipBottom.rectTransform.localPosition.z);
                break;
            case TipPosition.LEFT:
                rectTransform.pivot = new Vector2(0.0f, 0.5f);
                break;
            case TipPosition.RIGHT:
                rectTransform.pivot = new Vector2(1.0f, 0.5f);
                break;
            default:
                break;
        }

        rectTransform.position = new Vector3(x, y);
        gameObject.SetActive(true);

        if (inputCatcher)
            inputCatcher.SetActive(closeOnInputCatcherClick);
    }

    private void CloseIfClickedOutside()
    {
        if (Input.GetMouseButton(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, null))
                this.Close();
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        CloseIfClickedOutside();
    }
}
