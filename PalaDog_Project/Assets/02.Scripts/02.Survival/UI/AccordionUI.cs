using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;




public class AccordionUI: MonoBehaviour
{
    private RectTransform content;  // ÄÜÅÙÃ÷ ¿µ¿ª
    private bool isExpanded = false; // È®Àå »óÅÂ
    [SerializeField]
    private List<Transform> items;

    public float x;
    public float y;

    public float diff_X;
    public float diff_Y;



    private void Awake()
    {
        content = GetComponent<RectTransform>();
        int cnt = transform.childCount;
        x = content.anchoredPosition.x;
        y = content.anchoredPosition.y;
        for (int i = 0; i < cnt; i++)
        {
            var c = transform.GetChild(i);
            if (c.name != "Toggle")
            {
                items.Add(c);
            }
        }
    }

    public void Toggle()
    {
        isExpanded = !isExpanded;
        if (isExpanded)
        {
            Expand();
        }
        else
        {
            Collapse();
        }
    }

/*    public void ToggleVertical()
    {
        isExpanded = !isExpanded;
        if (isExpanded)
        {
            Expand();
        }
        else
        {
            CollapseVertical();
        }
    }*/

    private void Expand()
    {
        // ÄÜÅÙÃ÷¸¦ º¸¿©ÁÜ
        DOTween.To(() => content.anchoredPosition, x => content.anchoredPosition = x, new Vector2(x+diff_X,y+ diff_Y), 0.2f)
            .SetEase(Ease.OutSine);
    }

    private void Collapse()
    {
        DOTween.To(() => content.anchoredPosition, x => content.anchoredPosition = x, new Vector2(x, y), 0.2f)
        .SetEase(Ease.OutSine);

    }


/*    private void CollapseVertical(int diff)
    {
        DOTween.To(() => content.anchoredPosition, y => content.anchoredPosition = y, new Vector2(0, diff), 0.2f)
        .SetEase(Ease.OutSine);

    }*/
}
