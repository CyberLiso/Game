using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagment
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        [Range(0, 6)] [SerializeField] float fadeOutTime = 3f;
        // Start is called before the first frame update
        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeOut(float fadeTime)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / fadeTime;
                yield return null;
            }
        }
        public void InstantFadeOut()
        {
            canvasGroup.alpha = 1f;
        }
        public IEnumerator FadeIn(float fadeTime)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / fadeTime;
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
