using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagment
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        [Range(0, 6)] [SerializeField] float fadeOutTime = 3f;
        Coroutine currentFade = null;
        // Start is called before the first frame update
        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeOut(float fadeTime)
        {
            if(currentFade != null)
            {
                StopCoroutine(currentFade);
            }
            currentFade = StartCoroutine(FadeOutCoroutine(fadeTime));
            yield return currentFade;
        }
        public void InstantFadeOut()
        {
            canvasGroup.alpha = 1f;
        }

        private IEnumerator FadeOutCoroutine(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }
        public IEnumerator FadeIn(float fadeTime)
        {
            if (currentFade != null)
            {
                StopCoroutine(currentFade);
            }
            currentFade = StartCoroutine(FadeInCoroutine(fadeTime));
            yield return currentFade;
        }
        private IEnumerator FadeInCoroutine(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
        public IEnumerator DoCompleteFadeProcess()
        {
            yield return FadeOut(fadeOutTime);
            yield return FadeIn(fadeOutTime);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
