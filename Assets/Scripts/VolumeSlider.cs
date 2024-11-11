using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER,
        MUSIC,
        SFX
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider _volumeSlider;

    private void Awake()
    {
        _volumeSlider = this.GetComponent<Slider>();
    }

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

