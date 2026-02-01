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
}