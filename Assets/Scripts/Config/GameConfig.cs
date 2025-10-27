using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "GameConfig/Create Game Config File", order = 1)]
public class GameConfig : ScriptableObject
{
    [Tooltip("Row number for the cards to create in game")]
    [Range(2, 7)]
    public int cardRow;

    [Tooltip("Coloum number for the cards to create in game")]
    [Range(2, 5)]
    public int cardCol;

    [Tooltip("List of all the animal images which will be shown on the cards")]
    public List<Sprite> sprites = new List<Sprite>();

    [Tooltip("List of all the audios which will play in game")]
    public List<AudioFile> audios = new List<AudioFile>();
}

[Serializable]
public class AudioFile
{
    [SerializeField] private AudioClip audioClip;
    [Range(0F, 1F)] public float volume = 1;
    [SerializeField] private AudioTag audioTag;
    public bool loop;
    public bool isMusic;
    internal AudioTag AudioTag => audioTag;
    internal AudioClip AudioClip => audioClip;
}

public enum AudioTag
{
    BG,
    ButtonTap,
    Match,
    UnMatch,
    Completed
}
