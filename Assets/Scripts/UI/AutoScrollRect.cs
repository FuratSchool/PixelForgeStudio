 
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
// Add the script to your Dropdown Menu Template Object via (Your Dropdown Button > Template)
 
[RequireComponent(typeof(ScrollRect))]
public class AutoScrollRect : MonoBehaviour
{
    // Sets the speed to move the scrollbar
    public float scrollSpeed = 10f;
 
    // Set as Template Object via (Your Dropdown Button > Template)
    public ScrollRect m_templateScrollRect;
 
    // Set as Template Viewport Object via (Your Dropdown Button > Template > Viewport)
    public RectTransform m_templateViewportTransform;
 
    // Set as Template Content Object via (Your Dropdown Button > Template > Viewport > Content)
    public RectTransform m_ContentRectTransform;
 
    private RectTransform m_SelectedRectTransform;
 
    void Update()
    {
        UpdateScrollToSelected(m_templateScrollRect, m_ContentRectTransform, m_templateViewportTransform);
    }
 
    void UpdateScrollToSelected(ScrollRect scrollRect, RectTransform contentRectTransform, RectTransform viewportRectTransform)
    {
        bool isthirdparent = false;
        bool isfourthparent = false;
        // Get the current selected option from the eventsystem.
        GameObject selected = EventSystem.current.currentSelectedGameObject;
 
        if (selected == null)
        {
            return;
        }
        
        if (selected.transform.parent.parent != contentRectTransform.transform)
        {
            if (selected.transform.parent.parent.parent != contentRectTransform.transform)
            {
                if (selected.transform.parent.parent.parent.parent != contentRectTransform.transform)
                {
                    return;
                }
                else
                {
                    isfourthparent = true;
                }
            }
            else
            {
                isthirdparent = true;
            }
            
        }
        
        if(isfourthparent)
            m_SelectedRectTransform = selected.transform.parent.parent.parent.GetComponent<RectTransform>();
        else  if(isthirdparent)
            m_SelectedRectTransform = selected.transform.parent.parent.GetComponent<RectTransform>();
        else
            m_SelectedRectTransform = selected.transform.parent.GetComponent<RectTransform>();
        // Math stuff
        Vector3 selectedDifference = viewportRectTransform.localPosition - m_SelectedRectTransform.localPosition;
        float contentHeightDifference = (contentRectTransform.rect.height - viewportRectTransform.rect.height);
 
        float selectedPosition = (contentRectTransform.rect.height - selectedDifference.y);
        float currentScrollRectPosition = scrollRect.normalizedPosition.y * contentHeightDifference;
        float above = currentScrollRectPosition - (m_SelectedRectTransform.rect.height / 2) + viewportRectTransform.rect.height;
        float below = currentScrollRectPosition + (m_SelectedRectTransform.rect.height / 2);
 
        // Check if selected option is out of bounds.
        if (selectedPosition > above)
        {
            float step = selectedPosition - above;
            float newY = currentScrollRectPosition + step;
            float newNormalizedY = newY / contentHeightDifference;
            scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.unscaledDeltaTime);
        }
        else if (selectedPosition < below)
        {
            float step = selectedPosition - below;
            float newY = currentScrollRectPosition + step;
            float newNormalizedY = newY / contentHeightDifference;
            scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.unscaledDeltaTime);
        }
    }
}