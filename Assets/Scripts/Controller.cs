using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _effectSource;

    public AudioSource MusicSource => _musicSource;
    public AudioSource EffectSource => _effectSource;
    public static Controller Instance;
    public PlayerShip _myShip;
    private void Awake()
    {
        Instance = this;
    }
}
