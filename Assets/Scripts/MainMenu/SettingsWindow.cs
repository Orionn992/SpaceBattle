using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] private Slider _sliderMusic;
    [SerializeField] private Slider _effectMusic;
    private void Awake()
    {
        _sliderMusic.value = Controller.Instance.MusicSource.volume;
        _sliderMusic.value = Controller.Instance.EffectSource.volume;
    }
    public void CangeValueMusic(float value)
    {
        Controller.Instance.MusicSource.volume = value;
    }
        public void CangeValueEffect(float value)
    {
        Controller.Instance.EffectSource.volume = value;
    }
}
