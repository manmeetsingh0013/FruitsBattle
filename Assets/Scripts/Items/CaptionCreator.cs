using System.Collections;
using CardMatch.Services;
using TMPro;
using UnityEngine;

namespace CardMatch.UI.Items
{
    public class CaptionCreator : MonoBehaviour
    {
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;

        [SerializeField] private RectTransform captionTransform;
        [SerializeField] private AnimationCurve tweenCurve;

        private Vector3 _start, _end;

        private void Awake()
        {
            SetStartEndPoints(startPoint.position,endPoint.position);   
        }

        private void SetStartEndPoints(Vector2 start,Vector2 end)
        {
            _start = start;
            _end = end;
            captionTransform.anchoredPosition= _start;
        }
        internal void TranslateCaptionTransform(Vector2 start,Vector2 end)
        {
            SetStartEndPoints(start, end);
            StartCoroutine(TranslationRoutine());
        }

        private IEnumerator TranslationRoutine()
        {
            var progress = 0F;
            var eof = new WaitForEndOfFrame();
            var textTransform = captionTransform.anchoredPosition;
            var finalProgress = tweenCurve[tweenCurve.length - 1].time;
            while (progress < finalProgress)
            {
                captionTransform.anchoredPosition = Vector3.Lerp(_start, _end, tweenCurve.Evaluate(progress));
                yield return eof;
                progress += Time.deltaTime;
            }
            Bootstrap.GetService<AudioService>().PlayAudio(AudioTag.Completed);
            captionTransform.anchoredPosition = _end;
        }
    }
}