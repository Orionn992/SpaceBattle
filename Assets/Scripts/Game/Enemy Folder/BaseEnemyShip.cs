using UnityEngine;
using UniRx;
using System;
using System.Collections;


public enum StageShip
{
    In,
    Wait,
    Out,
}
public abstract class BaseEnemyShip : MonoBehaviour
{

    [Header("Base Fields")]
    [SerializeField] private float _normanSpeed = 8;
    [SerializeField] private float _delayTurbo = 2;
    [SerializeField] private float _turboSpeed = 5;
    [SerializeField] private float _speedRotation = 0.01f;
    [SerializeField] private int _collisionDamage = 10;
    [SerializeField] private int _maxHealth = 2;
    [SerializeField] private int _costpointersScore = 5;

    public int CostpointersScore => _costpointersScore;


    [HideInInspector] public PlayerShip _player;
    [HideInInspector] public Transform _myRoot;
    [HideInInspector] public Vector3 _playerLastPos = Vector3.up;
    private Subject<MonoBehaviour> _putMe = new Subject<MonoBehaviour>();
    public IObservable<MonoBehaviour> PutMe => _putMe;
    private Vector3 DirectionToPlayer => transform.position - new Vector3(_playerLastPos.x, _playerLastPos.y, 0);
    private int _health = 100;
    private float _goTo;
    private float _goToPointTurbo;
    private float _timerDelay;

    private IEnumerator Core()
    {
        UpdateStage(StageShip.In);
        while (transform.position.y > _goToPointTurbo)
        {
            transform.position -= new Vector3(0, Time.deltaTime * _normanSpeed, 0);
            Look(new Vector3(0, _goToPointTurbo, 0));
            yield return null;
        }
        UpdateStage(StageShip.Wait);
        while (_timerDelay < _delayTurbo)
        {
            _timerDelay += Time.deltaTime;
            yield return null;
        }
        UpdateStage(StageShip.Out);
        if (_playerLastPos != Vector3.up)
        {
            var dir = DirectionToPlayer / DirectionToPlayer.magnitude;
            while (transform.position.y > _goTo && transform.position.y < -_goTo)
            {
                Look(dir);
                transform.position -= dir * Time.deltaTime * _turboSpeed;
                yield return null;
            }
        }
        else
        {
            while (transform.position.y > _goTo)
            {
                transform.position -= new Vector3(0, Time.deltaTime * _turboSpeed, 0);
                yield return null;
            }
        }
        _putMe.OnNext(this);
    }
    private void OnEnable()
    {
        _timerDelay = 0;
        var controller = Controller.Instance;
        _goTo = controller.RightDownPoint.y - 2;
        _goToPointTurbo = UnityEngine.Random.Range((controller.CentrCam.y + 1), (controller.LeftUpPoint.y - 1));
        _health = _maxHealth;
        StartCoroutine(Core());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected void Look(Vector3 dir, bool lerp = false, bool invertion = false)
    {
        float singnedAngle = Vector2.SignedAngle(Vector2.down, dir);
        if (invertion == true) singnedAngle += 180;
        if (Mathf.Abs(singnedAngle) >= 1e-3f)
        {
            var angles = transform.eulerAngles;
            angles.z = singnedAngle;
            if (lerp)
            {
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, angles, _speedRotation);
            }
            else
            {
                transform.eulerAngles = angles;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject;
        if (obj.CompareTag("Bullet"))
        {
            var bull = obj.GetComponent<Bullet>();
            bull.HitMe();
            DamageMe(bull._damage, this);
            return;
        }
        if (obj.CompareTag("Player"))
        {
            obj.GetComponent<PlayerShip>().DamageMe(_collisionDamage);
            Controller.Instance.Score.Value += (_costpointersScore/2);
            _putMe.OnNext(this);
        }
    }
    private void DamageMe(int damage, BaseEnemyShip baseEnemy)
    {
        _health -= damage;
        if(_health <= 0)
        {
            _health = _maxHealth;
            Controller.Instance.Score.Value += _costpointersScore;
            _putMe.OnNext(this);
        }
    }
    protected abstract void UpdateStage(StageShip stage);

}
