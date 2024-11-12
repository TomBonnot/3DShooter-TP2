using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    //enumerator of VolumeType to handle easily switch cases
    private enum VolumeType
    {
        MASTER,
        MUSIC,
        SFX
    }

    //Selecting the right VolumeType for the slider
    [Header("Type")]
    [SerializeField] private VolumeType volumeType;
    private Slider _volumeSlider;

    //Getting the slider automatically
    private void Awake()
    {
        _volumeSlider = this.GetComponent<Slider>();
    }

    /**
    *   Update to handle the instance through a switch case with a this volumeType variable
    **/
    void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                _volumeSlider.value = AudioManager.instance.masterVolume;
                break;
            case VolumeType.MUSIC:
                _volumeSlider.value = AudioManager.instance.musicVolume;
                break;
            case VolumeType.SFX:
                _volumeSlider.value = AudioManager.instance.sfxVolume;
                break;
            default:
                Debug.LogWarning("Not a correct Volume Type");
                break;
        }
    }

    /**
    *   When the slider has a value that has changed, correctly affect the variable 
    **/
    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.instance.masterVolume = _volumeSlider.value;
                break;
            case VolumeType.MUSIC:
                AudioManager.instance.musicVolume = _volumeSlider.value;
                break;
            case VolumeType.SFX:
                AudioManager.instance.sfxVolume = _volumeSlider.value;
                break;
            default:
                Debug.LogWarning("Not a correct Volume Type");
                break;
        }
    }
}

