using UnityEngine;
using UniRx;

public class PlayerShip : MonoBehaviour
{
    [SerializeField] private float _speed = 15;
    [SerializeField] private float _coolDown = 0.1f;
    public int _maxHealth = 100;
    [SerializeField] private float _shipRollEuler = 45;
    [SerializeField] private float _shipRollSpeed = 80;
    [SerializeField] private float _smothness = 1.2f;

    private Rigidbody2D _rigidbody;
    private float _coolDownCurrent = 10;
    private MeshRenderer _mR;
    private Vector3 _sizeWorldShip;
    private Controller _controller;

    [HideInInspector] public ReactiveProperty<int> _health = new ReactiveProperty<int>();
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _mR = GetComponent<MeshRenderer>();
        _controller = Controller.Instance;
        _controller._myShip = this;
        _sizeWorldShip = _mR.bounds.extents;
    }
    private void Start()
    {
        _health.Value = _maxHealth;
    }
    private void Update()
    {
        UpdateKey();
    }
    private void UpdateKey()
    {
        float moveHor = Input.GetAxis("Horizontal");
        float moveVert = Input.GetAxis("Vertical");
        _rigidbody.linearVelocity = Vector2.Lerp(_rigidbody.linearVelocity, new Vector2(moveHor * _speed *1.2f, moveVert * _speed), _smothness);
    }
}
