//ORG: Ghostyii & MOONLIGHTGAME
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]
public class UIArrow : MonoBehaviour
{
    //Aim target
    public Transform target;
    //Canvas for clamp rect area
    public Canvas canvas;
    //Arrow pointer
    public RectTransform arrow;

    private RectTransform rectTrans = null;
    private SpriteRenderer targetRenderer = null;
    private Vector2 anchoredPosition = Vector2.zero;
    private Image arrowImg = null;

    private void Start()
    {
        if (!rectTrans)
            rectTrans = GetComponent<RectTransform>();

        if (target)
            targetRenderer = target.GetComponent<SpriteRenderer>();

        if (canvas)
            anchoredPosition = canvas.GetComponent<RectTransform>().anchoredPosition;

        if (arrow)
            arrowImg = arrow.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
            return;

        if (arrowImg)
            arrowImg.enabled = !targetRenderer.isVisible;

        var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);

        rectTrans.position = GetClampPos(screenPoint, CalRectByCanvas(canvas, rectTrans.sizeDelta));

        if (arrow)
            arrow.eulerAngles = LookAt2D(arrow.transform, screenPoint);
    }

    private Vector2 GetClampPos(Vector2 pos, Rect area)
    {
        Vector2 safePos = Vector2.zero;
        safePos.x = Mathf.Clamp(pos.x, area.xMin, area.xMax);
        safePos.y = Mathf.Clamp(pos.y, area.yMin, area.yMax);

        return safePos;
    }

    private Rect CalRectByCanvas(Canvas c, Vector2 uiSize)
    {
        Rect rect = Rect.zero;
        Vector2 area = c.GetComponent<RectTransform>().sizeDelta;

        rect.xMax = area.x - uiSize.x / 2;
        rect.yMax = area.y - uiSize.y / 2;
        rect.xMin = uiSize.x / 2;
        rect.yMin = uiSize.y / 2;

        return rect;
    }

    public Vector3 LookAt2D(Transform from, Vector3 to)
    {
        float dx = to.x - from.transform.position.x;
        float dy = to.y - from.transform.position.y;
        float rotationZ = Mathf.Atan2(dy, dx) * 180 / Mathf.PI;
        rotationZ -= 90;

        float originRotationZ = from.eulerAngles.z;
        float addRotationZ = rotationZ - originRotationZ;
        if (addRotationZ > 180)
            addRotationZ -= 360;

        return new Vector3(0, 0, from.eulerAngles.z + addRotationZ);
    }

}
