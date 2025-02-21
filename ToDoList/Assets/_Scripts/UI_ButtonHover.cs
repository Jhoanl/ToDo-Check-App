using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RapidFireTactics
{
    public class UI_ButtonHover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const float MIN_SCALE_ON_HOVER = .85f;

        protected void OnEnable()
        {
            transform.localScale = Vector3.one;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            LerpTo(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            LerpTo(false);
        }

        private void LerpTo(bool down)
        {
            if (!down)
            {
                StartCoroutine(InterpolateSize(1, MIN_SCALE_ON_HOVER, .15f, transform));
            }
            else
            {
                StartCoroutine(InterpolateSize(MIN_SCALE_ON_HOVER, 1, .15f, transform));
            }
        }

        private IEnumerator InterpolateSize(float from, float to,
           float duration, Transform transf)
        {
            float time = 0;

            while (time < 1)
            {
                time += (1 / (duration * .25f)) * Time.deltaTime;
                transf.localScale = Vector3.Lerp(transf.localScale,
                    Vector3.one * to, time);

                yield return null;
            }

            time = 0;

            while (time < 1)
            {
                time += (1 / duration) * Time.deltaTime;
                transf.localScale = Vector3.Lerp(transf.localScale,
                    Vector3.one * from, time);

                yield return null;
            }

            transf.localScale = Vector3.one * from;
        }
    }
}
