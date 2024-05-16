using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeGraphic : MonoBehaviour {

    [SerializeField] private List<MaskableGraphic> graphics;

    // ----------------------------------------------------------------------------------------------------------------

    #region Fade In

    /// <summary>
    /// Fades object referenced graphics linearly from transparent to opaque
    /// </summary>
    /// <param name="time">Amount of total time to fade for</param>
    /// <param name="step">Increment interval</param>
    /// <param name="onFinish">Action to perform when complete</param>
    public void FadeIn(float time, float step = 0, Action onFinish = null) {
        FadeIn(graphics, time, step, onFinish);
    }

    /// <summary>
    /// Fades given graphics linearly from transparent to opaque
    /// </summary>
    /// <param name="graphics">List of graphics to fade</param>
    /// <param name="time">Amount of total time to fade for</param>
    /// <param name="step">Increment interval</param>
    /// <param name="onFinish">Action to perform when complete</param>
    public void FadeIn(List<MaskableGraphic> graphics, float time, float step = 0, Action onFinish = null) {
        StartCoroutine(FadeInCoroutine());

        IEnumerator FadeInCoroutine() {
            // Set all alpha to 0
            foreach(MaskableGraphic graphic in graphics) {
                SetAlpha(graphic, 0);
                graphic.gameObject.SetActive(true);
            }

            // Fade
            for(float i = 0; i < time; i += (step == 0) ? Time.deltaTime : step) {
                foreach(MaskableGraphic graphic in graphics)
                    SetAlpha(graphic, i / time);
                yield return new WaitForSeconds(step);
            }

            // Set all alpha to 1
            foreach(MaskableGraphic graphic in graphics)
                SetAlpha(graphic, 1);

            onFinish?.Invoke();
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------

    #region Fade Out

    /// <summary>
    /// Fades object referenced graphics linearly from opaque to transparent
    /// </summary>
    /// <param name="time">Amount of total time to fade for</param>
    /// <param name="step">Increment interval</param>
    /// <param name="disableOnFinish">Whether the objects should be disabled when complete</param>
    /// <param name="onFinish">Action to perform when complete</param>
    public void FadeOut(float time, float step = 0, bool disableOnFinish = true, Action onFinish = null) {
        FadeOut(graphics, time, step, disableOnFinish, onFinish);
    }

    /// <summary>
    /// Fades given graphics linearly from opaque to transparent
    /// </summary>
    /// <param name="graphics">List of graphics to fade</param>
    /// <param name="time">Amount of total time to fade for</param>
    /// <param name="step">Increment interval</param>
    /// <param name="disableOnFinish">Whether the objects should be disabled when complete</param>
    /// <param name="onFinish">Action to perform when complete</param>
    public void FadeOut(List<MaskableGraphic> graphics, float time, float step = 0, bool disableOnFinish = true, Action onFinish = null) {
        StartCoroutine(FadeOutCoroutine());

        IEnumerator FadeOutCoroutine() {
            // Set all alpha to 1
            foreach(MaskableGraphic graphic in graphics)
                SetAlpha(graphic, 1);

            // Fade
            for(float i = 0; i < time; i += (step == 0) ? Time.deltaTime : step) {
                foreach(MaskableGraphic graphic in graphics)
                    SetAlpha(graphic, 1 - (i / time));
                yield return new WaitForSeconds(step);
            }

            // Set all alpha to 0
            foreach(MaskableGraphic graphic in graphics) {
                SetAlpha(graphic, 0);
                if(disableOnFinish)
                    graphic.gameObject.SetActive(false);
            }

            onFinish?.Invoke();
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Fades object referenced graphics linearly from transparent to opaque and back to transparent (fade in, then fade out)
    /// </summary>
    /// <param name="time">Amount of total time to fade for</param>
    /// <param name="step">Increment interval</param>
    /// <param name="holdTime">Amount of time to hold opaque graphic (time between fade in and fade out)</param>
    /// <param name="disableOnFinish">Whether the objects should be disabled when complete</param>
    /// <param name="onHold">Action to perform in between fade in and fade out</param>
    /// <param name="onFinish">Action to perform when complete</param>
    public void FadeInOut(float time, float step = 0, float holdTime = 0, bool disableOnFinish = true, Action onHold = null, Action onFinish = null) {
        FadeInOut(graphics, time, step, holdTime, disableOnFinish, onHold, onFinish);
    }

    /// <summary>
    /// Fades given graphics linearly from transparent to opaque and back to transparent (fade in, then fade out)
    /// </summary>
    /// <param name="graphics">List of graphics to fade</param>
    /// <param name="time">Amount of total time to fade for</param>
    /// <param name="step">Increment interval</param>
    /// <param name="holdTime">Amount of time to hold opaque graphic (time between fade in and fade out)</param>
    /// <param name="disableOnFinish">Whether the objects should be disabled when complete</param>
    /// <param name="onHold">Action to perform in between fade in and fade out</param>
    /// <param name="onFinish">Action to perform when complete</param>
    public void FadeInOut(List<MaskableGraphic> graphics, float time, float step = 0, float holdTime = 0, bool disableOnFinish = true, Action onHold = null, Action onFinish = null) {
        FadeIn(graphics, time, step, () => {
            onHold?.Invoke();
            StartCoroutine(Out());
        });

        IEnumerator Out() {
            yield return new WaitForSeconds(holdTime);
            FadeOut(graphics, time, step, disableOnFinish, onFinish);
        }
    }

    // ----------------------------------------------------------------------------------------------------------------

    private void SetAlpha(MaskableGraphic graphic, float alpha) {
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }

}
