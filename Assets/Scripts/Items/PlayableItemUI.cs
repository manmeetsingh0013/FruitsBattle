using CardMatch.Services;
using CardMatch.UI.Screens;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace CardMatch.UI.Items
{
    public class PlayableItemUI : MonoBehaviour
    {
        [SerializeField] private Image revealImg;
        [SerializeField] private RectTransform hideImg;
        [SerializeField] private GameConfig gameConfig;

        public int cardId { get; private set; }

        private float flipTime = 0.2f;
        private Vector3 revealRotation = new Vector3(0, 90, 0);
        private Button button;

        public bool isMatched { get; private set; }

        private Coroutine flipRoutine;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        internal void SetValues(int index)
        {
            cardId = index;
            revealImg.sprite = gameConfig.sprites[index];
        }
        internal void ShowAndHideCard(bool status)
        {
            gameObject.SetActive(status && !isMatched);
        }
        internal void SetRevealCard()
        {
            hideImg.transform.eulerAngles = revealRotation;
        }
        internal void SetMatched(bool match)
        {
            isMatched = match;
        }

        internal void FlipAnimation(float delay =1.5f)
        {
            if (gameObject.activeInHierarchy)
            {
                if (flipRoutine != null)
                    StopCoroutine(flipRoutine);
                flipRoutine = StartCoroutine(CardCoroutine(hideImg.transform, delay, revealRotation, Vector3.zero));
            }
        }

        private IEnumerator CardCoroutine(Transform target,float delay,Vector3 start,Vector3 end,Action AfterReveal=null)
        {
            button.enabled = false;
            yield return new WaitForSeconds(delay);

            float currentTime = 0;
            while(currentTime < flipTime)
            {
                target.eulerAngles = Vector3.Lerp(start, end, currentTime / flipTime);
                currentTime += Time.deltaTime;
                yield return null;
            }
            target.eulerAngles = end;
            yield return new WaitForSeconds(0.8f);
            AfterReveal?.Invoke();
            button.enabled = true;
        }

        public void OnOptionClicked()
        {
            button.enabled = false;
            StartCoroutine(CardCoroutine(hideImg.transform, 0, Vector3.zero, revealRotation,StartCompare));
            Bootstrap.GetService<AudioService>().PlayAudio(AudioTag.ButtonTap);
        }

        private void StartCompare()
        {
            GameplayHudScreen hudScreen = Bootstrap.GetService<UserInterfaceService>()
                .CurrentInterface.GetScreen<GameplayHudScreen>();
            hudScreen.SetAndComparePlayerOption(this);
        }

        internal void SetRandomSiblingValue()
        {
            int totalChild = transform.parent.transform.childCount;
            int childIndex = RandomizationWithExclusion.GetRandomWithExclusion(new System.Random(), totalChild - 1);
            transform.SetSiblingIndex(childIndex);
        }
    }
}