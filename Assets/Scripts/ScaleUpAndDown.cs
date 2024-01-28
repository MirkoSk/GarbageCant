using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleUpAndDown : MonoBehaviour
{
    void Start()
    {
        GetComponent<RectTransform>().DOScale(1.1f, 1f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }
}
