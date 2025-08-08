using UnityEngine;
using DG.Tweening;


public class EnemyBase : MonoBehaviour
{
    [Tooltip("敵のHP")]
    [SerializeField] int _enemyHP = 5;
    [Tooltip("攻撃力")]
    [SerializeField] int _enemyAttackPower = 1;
    [SerializeField] GameObject _player;
    [Tooltip("スピード")]
    [SerializeField] float _speed;
    [Tooltip("ノックバックの強さ")]
    [SerializeField] float _knockbackForce;
    [Tooltip("ノックバックの長さ")]
    [SerializeField] float _knockbackTime;
    [Tooltip("コインドロップ数")]
    [SerializeField] int _numCoin;
    [SerializeField] GameObject _coin;
    PlayerController _playerController;
    CastleControler _castleControler;
    GameObject _castle;
    Rigidbody2D _rb;
    bool _knockback = false;
    float _timer = 0;

    [SerializeField] float _duration;
    [SerializeField] float _strength;
    [SerializeField] int _vibrato;
    [SerializeField] float _randomness;
    bool _fadeOut;
    private Tweener _shakeTweener;
    private Vector3 _initPosition;

    PauseManager2D _pauseManager = default;
    Vector3 _velocity;
    bool _pause = false;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = _player.GetComponent<PlayerController>();
        _castle = GameObject.FindGameObjectWithTag("Castle");
        _castleControler = _castle.GetComponent<CastleControler>();
        _rb = GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        _pauseManager = GameObject.FindObjectOfType<PauseManager2D>();  // この処理は Start やると遅いので Awake で行う。OnEnable の方が Start より先に呼ばれるため。
    }
    void OnEnable()
    {
        // 呼んで欲しいメソッドを登録する。
        _pauseManager.OnPauseResume += PauseResume;
    }
    void OnDisable()
    {
        // OnDisable ではメソッドの登録を解除すること。さもないとオブジェクトが無効にされたり破棄されたりした後にエラーになってしまう。
        _pauseManager.OnPauseResume -= PauseResume;
    }
    // Update is called once per frame
    void Update()
    {
        if (!_pause)
        {
            _timer += Time.deltaTime;
            if (_timer >= _knockbackTime)
            {
                _knockback = false;
            }
            if (!_knockback)
            {
                Homing();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_playerController._life > 0)
            {
                Vector2 thisPos = transform.position;
                float xPos = thisPos.x;
                float yPos = thisPos.y;
                if (!_playerController._down)
                {
                    if (_enemyHP > 0)
                    {
                        if (_playerController._powerup)
                        {
                            _knockback = true;
                            _timer = 0;
                            float xDis = thisPos.x - _player.transform.position.x;
                            float yDis = thisPos.y - _player.transform.position.y;
                            Vector2 kb = new Vector2(xDis, yDis);
                            _rb.velocity = kb * _knockbackForce * 1.4f;

                            float p = (float)_playerController._playerAttackPower;
                            p *= 1.5f;
                            int _p = (int)p;
                            _enemyHP -= _p;
                        }
                        else
                        {
                            _playerController.OnDamagePlayer(_enemyAttackPower, xPos, yPos);
                            _enemyHP -= _playerController._playerAttackPower;
                            _initPosition = transform.position;
                            StartShake(_duration, _strength, _vibrato, _randomness, _fadeOut);
                        }
                    }
                    if (_enemyHP <= 0)
                    {
                        for (int i = 0; i < _numCoin; i++)
                        {
                            Instantiate(_coin, transform.position, Quaternion.identity);
                        }
                        Destroy(this.gameObject);
                    }
                }
            }

            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_knockback)
        {
            if (collision.gameObject.tag == "Castle")
            {
                _castleControler.OnDamageCastle(_enemyAttackPower);
                //Debug.Log("ノックバック");
                _knockback = true;
                _timer = 0;
                Vector2 thisPos = transform.position;
                Vector2 castlePos = _castle.transform.position;
                float xDistination = thisPos.x - castlePos.x;
                float yDistination = thisPos.y - castlePos.y;
                Vector2 knockback = new Vector2(xDistination, yDistination);
                _rb.velocity = knockback * _knockbackForce;
            }
        }
    }
    void Homing()
    {
        Vector2 castlePos = _castle.transform.position;
        float x = castlePos.x;
        float y = castlePos.y;
        Vector2 direction = new Vector2(x - transform.position.x, y - transform.position.y).normalized;
        _rb.velocity = direction * _speed;
    }

    public void StartShake(float duration, float strength, int vibrato, float randomness, bool fadeOut)
    {
        // 前回の処理が残っていれば停止して初期位置に戻す
        if (_shakeTweener != null)
        {
            _shakeTweener.Kill();
            gameObject.transform.position = _initPosition;
        }
        // 揺れ開始
        _shakeTweener = gameObject.transform.DOShakePosition(duration, strength, vibrato, randomness, fadeOut);
    }

    void PauseResume(bool isPause)
    {
        if (isPause)
        {
            Pause();
        }
        else
        {
           Resume();
        }
    }
    public void Pause()
    {
        // 速度・回転を保存し、Rigidbody を停止する
        
        _velocity = _rb.velocity;
        _rb.Sleep();
        _pause = true;
        //Debug.Log("PAUSE");
    }

    public void Resume()
    {
        // Rigidbody の活動を再開し、保存しておいた速度・回転を戻す
        _rb.WakeUp();
        _pause  = false;
        _rb.velocity = _velocity;
        //Debug.Log("RESUME");
    }
}
