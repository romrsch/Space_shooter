using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] private Slider _slideMusic;
    [SerializeField] private Slider _slideEffect;

    private void Awake()
    {
        _slideMusic.value = Controller.Instance.MusicSource.volume;
        _slideEffect.value = Controller.Instance.EffectSource.volume;
    }

    public void ChangeValueMusic(float value)
    {
        Controller.Instance.MusicSource.volume = value;    
    }

    public void ChangeValueEffect(float value)
    {
        Controller.Instance.EffectSource.volume = value;
    }

}
