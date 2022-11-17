using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lean.Gui;

public class UIdata : MonoBehaviour
{
    /// <summary>
    /// The vibration bool
    /// </summary>
    private bool vibration = true;
    private bool vibrationProperty
    {
        get => vibration;
        set
        {
            vibration = value;
            ////////Sound Change///////////
            Debug.Log(vibration + " Vibration");
            PlayerPrefs.SetInt("vibration", ConvertToİnt(value));
        }
    }
    [SerializeField] LeanToggle VibrationToogle;


    /// <summary>
    /// The Starscount  
    /// </summary>
    private int StartsCount;
    private int StartsCountProperty
    {
        get => StartsCount;
        set
        {
            StartsCount = value;
            ////////Sound Change///////////
            Debug.Log(StartsCount + " StartScOUNT");
            PlayerPrefs.GetInt("stars", value);
            StartsText.text = StartsCount.ToString();
        }
    }
    public TextMeshProUGUI StartsText;





    /// <summary>
    /// The Bool Sound  
    /// </summary>
    private bool sound = true;
    private bool SoundProperty
    {
        get => sound;
        set
        {
            sound = value;
            ////////Sound Change///////////
            Debug.Log(sound + " Sound");
            PlayerPrefs.SetInt("sound", ConvertToİnt(value));
        }
    }
    [SerializeField] LeanToggle SoundToogle;



    /// <summary>
    /// The Bool Music  
    /// </summary>
    private bool music = true;
    private bool MusicProperty
    {
        get => music;
        set
        {
            music = value;
            ////////Music Change///////////
            Debug.Log(music + " Music");
            PlayerPrefs.SetInt("music", ConvertToİnt(value));
        }
    }
    [SerializeField] LeanToggle MusicToogle;

    private void Awake()
    {
        StartsCountProperty = PlayerPrefs.GetInt("stars", 0);
        SoundProperty = SoundToogle.On = ConvertToBoolen(PlayerPrefs.GetInt("sound", 1));
        MusicProperty = MusicToogle.On = ConvertToBoolen(PlayerPrefs.GetInt("music", 1));
        vibrationProperty = VibrationToogle.On = ConvertToBoolen(PlayerPrefs.GetInt("vibration", 1));
    }
    // public void setProperty(int Starts_count = -1, bool sound = true, bool music = true)
    // {
    //     if (Starts_count != -1) StartsCountProperty = StartsCount;
    //     if (sound != SoundProperty) SoundProperty = sound;
    //     if (music != MusicProperty) MusicProperty = music;
    // }

    public void SetMusic(bool m) => MusicProperty = m;
    public void SetSound(bool s) => SoundProperty = s;
    public void SetVibration(bool v) => vibrationProperty = v;



    private bool ConvertToBoolen(int i)
    {
        if (i == 1) return true;
        else return false;
    }
    private int ConvertToİnt(bool i)
    {
        if (i) return 1;
        else return 0;
    }

}
