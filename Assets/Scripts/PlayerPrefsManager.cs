using UnityEngine;
using System;
using System.Collections.Generic;

namespace CardMatch.GameData
{
    public static class PlayerPrefsManager
    {
        private const string UserMatchDataKey = nameof(UserMatchDataKey);

        internal static string UserMatchData
        {
            get
            {
                if (!PlayerPrefs.HasKey(UserMatchDataKey))
                    UserMatchData = string.Empty;
                return PlayerPrefs.GetString(UserMatchDataKey);
            }
            set
            {
                PlayerPrefs.SetString(UserMatchDataKey, value);
                Debug.Log($"UserMatchDataKey---> : {value}");
                PlayerPrefs.Save();
            }
        }
    }

    [Serializable]
    public class UserData
    {
        public int userMatches;
        public int userTurns;
        public List<CardData> cardData = new List<CardData>();
        public UserData()
        {

        }
    }
    [Serializable]
    public class CardData
    {
        public int cardId;
        public bool isMatched;

        public CardData()
        {
        }
    }
}