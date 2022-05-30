using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeGraphic : MonoBehaviour {

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
                yield return (step == 0) ? null : new WaitForSeconds(step);
            }

            // Set all alpha to 1
            foreach(MaskableGraphic graphic in graphics)
                SetAlpha(graphic, 1);

            onFinish?.Invoke();
        }
    }

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
                yield return (step == 0) ? null : new WaitForSeconds(step);
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

    private void SetAlpha(MaskableGraphic graphic, float alpha) {
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }

}