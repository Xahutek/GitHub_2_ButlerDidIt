using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DraggableImage : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public new RectTransform transform;
    public float scrollSpeed=0.1f, moveSpeed=1;
    float currentZoom = 1;
    public Vector2
        zoomCaps = new Vector2(1f, 3f),
        moveBoundsX = new Vector2( 0, 2000f),
        moveBoundsY = new Vector2( 0,1250f);

    private void Update()
    {
        float scrollDelta = Input.mouseScrollDelta.y;

        currentZoom = Mathf.Clamp(currentZoom + scrollDelta * scrollSpeed, zoomCaps.x, zoomCaps.y);

        transform.localScale = currentZoom * Vector3.one;

        if (scrollDelta != 0) Realign();
    }

    public void Realign()
    {
        float
            zoomLerp = (currentZoom - zoomCaps.x) / (zoomCaps.y - zoomCaps.x),
            XBounds = Mathf.Lerp(moveBoundsX.x, moveBoundsX.y, zoomLerp),
            YBounds = Mathf.Lerp(moveBoundsY.x, moveBoundsY.y, zoomLerp),
            Xanchor = Mathf.Clamp(transform.anchoredPosition.x, -XBounds, XBounds),
            Yanchor = Mathf.Clamp(transform.anchoredPosition.y, -YBounds, YBounds);

        transform.anchoredPosition = new Vector2(Xanchor, Yanchor);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    Vector2 DragStartPos, DragStartPosMouse;
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 movement = eventData.delta*moveSpeed;
        transform.anchoredPosition += movement;

        Realign();
    }
    

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
