using System.Collections;
using System.Collections.Generic;
using CardMatch.GameData;
using CardMatch.Services;
using CardMatch.UI.Base;
using CardMatch.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.UI.Screens
{
    public class GameplayHudScreen : ScreenBase
    {
        [SerializeField] private Transform cardContainer;
        [SerializeField] private GridLayoutGroup cardLayoutGroup;
        [SerializeField] private TextMeshProUGUI countdownTmp;

        [SerializeField] private TextMeshProUGUI currentScoreTmp;
        [SerializeField] private TextMeshProUGUI currentTurnsTmp;
        [SerializeField] private PlayableItemUI playerItemPrefab;

        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private CaptionCreator captionCreator;
        [SerializeField] private GameObject gamePanel;

        private List<PlayableItemUI> playableCards;
        private PlayableItemUI previousPlayer;
        private UserData userData = new UserData();

        private int currentMatchNumber = 0;
        private int currentMatchTurns = 0;
        private bool isBackPressed = false;

        protected override void InitializeScreen()
        {
            base.InitializeScreen();
            SetupGrid();
            playableCards = new List<PlayableItemUI>();

            userData = JsonUtility.FromJson<UserData>(PlayerPrefsManager.UserMatchData);
            if (IsCardMatched())
            {
                gamePanel.SetActive(true);
            }
            else
            {
                PopulatePlayerOptions();
            }
        }
        public bool IsCardMatched()
        {
            return string.IsNullOrEmpty(PlayerPrefsManager.UserMatchData) ? false : userData.cardData.FindAll((item) => item.isMatched.Equals(true)).Count > 0;
        }

        public void LoadLastGame()
        {
            LoadPreviousGame();
            gamePanel.SetActive(false);
            ShowCountdownTimer();
        }
        public void LoadNewGame()
        {
            PopulatePlayerOptions();
            gamePanel.SetActive(false);
            ShowCountdownTimer();
        }

        protected internal override void EnableScreen()
        {
            base.EnableScreen();
            captionCreator.gameObject.SetActive(false);

            playableCards.ForEach(Items => {
                if (Items)
                {
                    Items.SetRevealCard();
                    Items.ShowAndHideCard(true);
                }
            });

            if (isBackPressed)
                StartCoroutine(CountdownRoutine());
        }

        protected internal override void DisableScreen()
        {
            base.DisableScreen();
        }
        private void SaveDataWhenUserQuit()
        {
            if(userData !=null)
                userData.cardData.Clear();
            playableCards.ForEach(Items =>
            {
                CardData cardData = new CardData();
                cardData.cardId = Items.cardId;
                cardData.isMatched = Items.isMatched;
                if (userData == null) userData = new UserData();
                userData.cardData.Add(cardData);
            });

            if (userData != null && userData.cardData.Count > 0)
            {
                userData.userMatches = currentMatchNumber;
                userData.userTurns = currentMatchTurns;
                PlayerPrefsManager.UserMatchData = JsonUtility.ToJson(userData);
            }
        }

        public override void OnBackKeyPressed()
        {
            Bootstrap.GetService<AudioService>().PlayAudio(AudioTag.ButtonTap);
            PreviousScreen(true);
            cardContainer.gameObject.SetActive(false);
            isBackPressed = true;
        }

        internal Coroutine ShowCountdownTimer()=> StartCoroutine(CountdownRoutine());

        internal void UpdateCurrentScore(int score)
        {
            currentMatchNumber = score;
            currentScoreTmp.text = $"Matches : {score}";
            int totalElements = gameConfig.cardRow * gameConfig.cardCol;
            if(totalElements/2 ==score)
            {
                ShowOutcomeMessage();
            }
        }
        internal void ShowOutcomeMessage()
        {
            captionCreator.gameObject.SetActive(true);
            captionCreator.TranslateCaptionTransform(new Vector2(1560,0), Vector2.zero);
            Bootstrap.GetService<ScoreService>().ResetCurrentScore();
            UpdateCurrentScore(0);
            UpdateCurrentTurns(0);
            userData.cardData.Clear();
            isBackPressed = false;
            playableCards.ForEach(Items =>
            {
                if (Items)
                    Items.SetMatched(false);
            });
        }
        internal void UpdateCurrentTurns(int turns)
        {
            currentMatchTurns = turns;
            currentTurnsTmp.text = $"Turns : {turns}";
        }
        internal void SetAndComparePlayerOption(PlayableItemUI currentPlayer)
        {
            if (previousPlayer != null)
            {
                if (previousPlayer.cardId == currentPlayer.cardId)
                {
                    //Its A match...increment the score as well...
                    previousPlayer.SetMatched(true);
                    currentPlayer.SetMatched(true);
                    previousPlayer.ShowAndHideCard(false);
                    currentPlayer.ShowAndHideCard(false);
                    Bootstrap.GetService<ScoreService>().IncrementCurrentScore(1);
                    Bootstrap.GetService<AudioService>().PlayAudio(AudioTag.Match);
                }
                else
                {
                    previousPlayer.FlipAnimation(0);
                    currentPlayer.FlipAnimation(0);
                    Bootstrap.GetService<AudioService>().PlayAudio(AudioTag.UnMatch);
                }
                Bootstrap.GetService<ScoreService>().IncrementCurrentTurns(1);
                previousPlayer = null;
            }
            else
            {
                previousPlayer = currentPlayer;
            }
        }

        private void OnApplicationQuit()
        {
            SaveDataWhenUserQuit();
            DePopulatePlayerOptions();
        }

        private void PopulatePlayerOptions()
        {
            int totalCells = gameConfig.cardRow * gameConfig.cardCol;
            playableCards.Clear();
            RandomizationWithExclusion.ClearExcludedList();
            cardContainer.gameObject.SetActive(false);
            int count = 0;
            int pairCardCount = 0;
            while (count < totalCells)
            {
                PlayableItemUI item = Instantiate(playerItemPrefab, cardContainer);
                item.SetValues(pairCardCount);
                playableCards.Add(item);
                count++;

                pairCardCount = count % 2 == 0 ? pairCardCount+1 : pairCardCount;
                if(pairCardCount >= gameConfig.sprites.Count)
                {
                    pairCardCount = 0;
                }
            }
            SetCardsRandomSibling();
        }

        private void LoadPreviousGame()
        {
            RandomizationWithExclusion.ClearExcludedList();
            cardContainer.gameObject.SetActive(false);
            playableCards.Clear();
            foreach (var item in userData.cardData)
            {
                PlayableItemUI playableItemUI = Instantiate(playerItemPrefab, cardContainer);
                playableItemUI.SetValues(item.cardId);
                playableItemUI.SetMatched(item.isMatched);
                playableItemUI.gameObject.SetActive(!item.isMatched);
                playableCards.Add(playableItemUI);
            }
            UpdateCurrentScore(userData.userMatches);
            UpdateCurrentTurns(userData.userTurns);
            SetCardsRandomSibling();
            Bootstrap.GetService<ScoreService>().UpdateCurrentScoreAndTurns(userData.userMatches, userData.userTurns);
        }
        private void SetCardsRandomSibling()
        {
            playableCards.ForEach(item => {
                item.SetRandomSiblingValue();
            });
        }
        private void SetupGrid()
        {
            cardLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            cardLayoutGroup.constraintCount = gameConfig.cardCol;
        }

        private void DePopulatePlayerOptions()
        {
            var count = playableCards.Count;
            while (count-- > 0)
                Destroy(playableCards[count]?.gameObject);
        }

        private IEnumerator CountdownRoutine()
        {
            var wait = new WaitForSeconds(.75F);
            var countdown = 3;
            while (countdown > 0)
            {
                countdownTmp.text = $"{countdown--}";
                yield return wait;
            }

            cardContainer.gameObject.SetActive(true);
            playableCards.ForEach(item =>
            {
                if (item)
                {
                    item.FlipAnimation();
                }
            });
            countdownTmp.text = string.Empty;

            yield return null;
            cardLayoutGroup.enabled = false;
            isBackPressed=false;
        }
    }
}