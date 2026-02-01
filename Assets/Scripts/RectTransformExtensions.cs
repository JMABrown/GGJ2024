using UnityEngine;
using DG.Tweening;

public static class RectTransformExtensions
{
    public static Tween Punch(
        this RectTransform rectTransform,
        float duration = 0.2f,
        float punchScale = 0.2f,
        int vibrato = 10,
        float elasticity = 1f
    )
    {
        if (rectTransform == null)
        {
            Debug.LogWarning("Attempted to punch a null RectTransform.");
            return null;
        }

        // Kill existing scale tweens to prevent stacking
        rectTransform.DOKill();

        Vector3 punchVector = Vector3.one * punchScale;

        return rectTransform.DOPunchScale(
            punch: punchVector,
            duration: duration,
            vibrato: vibrato,
            elasticity: elasticity
        );
    }
    
    public static Tween Shake(
        this RectTransform rectTransform,
        float duration = 0.3f,
        float strength = 20f,
        int vibrato = 20,
        float randomness = 90f,
        bool fadeOut = true,
        bool snapping = false
    )
    {
        if (rectTransform == null)
        {
            Debug.LogWarning("Attempted to shake a null RectTransform.");
            return null;
        }

        // Kill any existing shake on this rect to avoid stacking
        rectTransform.DOKill();

        return rectTransform.DOShakeAnchorPos(
            duration: duration,
            strength: strength,
            vibrato: vibrato,
            randomness: randomness,
            snapping: snapping,
            fadeOut: fadeOut
        );
    }
    
    public static Tween YeetCopy(
        this RectTransform rectTransform,
        RectTransform target
    )
    {
        if (rectTransform == null)
        {
            Debug.LogWarning("Attempted to shake a null RectTransform.");
            return null;
        }
        
        // Kill any existing shake on this rect to avoid stacking
        rectTransform.DOKill();

        var copy = GameObject.Instantiate(rectTransform.gameObject, rectTransform.parent);
        var copyRectTransform = copy.GetComponent<RectTransform>();
        
        return copyRectTransform.DOMove(target.position,
            duration:3f,
            snapping:false)
            .SetEase(Ease.OutCubic);
    }
}