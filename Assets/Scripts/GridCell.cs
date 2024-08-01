using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    public Text numberText;
    public int number;
    public int gridX;
    public int gridY;
    private Coroutine textAnimationCoroutine;
    private bool isAnimating = false;

    public void SetNumber(int num)
    {
        number = num;
        numberText.text = num > 0 ? num.ToString() : "";
    }

    public void SetPosition(int x, int y)
    {
        gridX = x;
        gridY = y;
    }
    public void ClearText()
    {
        numberText.text = "";
    }

 //   public void AnimateText(float n)
 //   {
  //      if (textAnimationCoroutine != null)
   //     {
  //         StopCoroutine(textAnimationCoroutine);
  //      }
  //      textAnimationCoroutine = StartCoroutine(AnimateTextCoroutine(n));
 //   }


    private IEnumerator AnimateTextCoroutine(float n)
    {
        if (isAnimating) yield break; // Exit if an animation is already in progress
        isAnimating = true;

        float duration = n; // Animation duration
        float targetScale = 1.5f; // Scale up to 1.5 times
        Vector3 originalScale = numberText.transform.localScale;
        Vector3 targetScaleVector = originalScale * targetScale;

        // Scale up
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            numberText.transform.localScale = Vector3.Lerp(originalScale, targetScaleVector, normalizedTime);
            yield return null;
        }
        numberText.transform.localScale = targetScaleVector;

        // Scale down
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            numberText.transform.localScale = Vector3.Lerp(targetScaleVector, originalScale, normalizedTime);
            yield return null;
        }
        numberText.transform.localScale = originalScale;

        isAnimating = false; // Reset flag when done
    }

    public void AnimateText(float duration)
    {
        if (textAnimationCoroutine != null)
        {
            StopCoroutine(textAnimationCoroutine); // Stop any previous coroutine
        }
        textAnimationCoroutine = StartCoroutine(AnimateTextCoroutine(duration));
    }
}
