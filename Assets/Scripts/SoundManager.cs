using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    void Start()
    {
        if(!PlayerPrefs.HasKey("volume")){
            PlayerPrefs.SetFloat("volume", 1);
            Load();
        }else Load();
    }

    public void ChangeVolume(){
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    void Load(){
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    void Save(){
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }

}
