using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSpaceCameraMousePositioning : MonoBehaviour
{
    [Header("WARNING: presumes Screen Space Canvas!")]
    public Canvas MyCanvas;
    public RectTransform Thingy;

    // since we assume it, just get it from the canvas;
    // if this property comes back null, you failed to set a Canvas above.
    Camera cam { get { return MyCanvas.worldCamera; } }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var pos = Input.mousePosition;

            Vector3 output = Vector2.zero;

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                MyCanvas.GetComponent<RectTransform>(),
                pos,
                cam,
                out output);

            Thingy.position = output;
        }
    }
}