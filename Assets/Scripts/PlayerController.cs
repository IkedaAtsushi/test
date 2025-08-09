using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// キャラクターを操作するコンポーネント
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("ステータス")]
    [Tooltip("体力の最大値")]
    [SerializeField] int _maxLife;
    [Tooltip("体力を表示するゲージ")]
    [SerializeField] Slider _lifeGauge;
    [Tooltip("体力の回復量")]
    [SerializeField] int _recoveryLife;
    [Tooltip("MPの最大値")]
    [SerializeField] float _maxMagicPower;
    [Tooltip("MPの減少速度")]
    [SerializeField] float _decreaseMagicPower;
    [Tooltip("MPを表示するゲージ")]
    [SerializeField] Slider _magicPowerGauge;
    [Tooltip("攻撃力")]
    [SerializeField] public int _playerAttackPower;
    [Tooltip("プレイヤーの速さ")]
    [SerializeField] float _moveSpeed = 5f;
    [Tooltip("ノックバックの強さ")]
    [SerializeField] float _knockbackForce = 3f;
    [Tooltip("ノックバックの長さ")]
    [SerializeField] float _knockbackTime;
    [SerializeField] Text _DamageText;
    [SerializeField] private Canvas _canvas;
    public int _life;
    public static float _magicPower;
    float _timer;
    public bool _down = false;
    public bool _powerup = false;

    Rigidbody2D _rb = default;
    Animator _animator;
    float _h;
    float _v;
    Image _fillimage;
    Color _yellow = Color.yellow;
    Color originalColor;

    PauseManager2D _pauseManager = default;
    Vector3 _velocity;
    bool _pause = false;
    // Start is called before the first frame update
    void Start()
    {
        _life = _maxLife;
        _lifeGauge.maxValue = _maxLife;
        _magicPower = 0;
        _magicPowerGauge.maxValue = _maxMagicPower;
        _timer = 1;
        _rb = GetComponent<Rigidbody2D>();
        _fillimage = _lifeGauge.fillRect.GetComponent<Image>();
        originalColor = _fillimage.color;
        this._animator = GetComponent<Animator>();
        this.gameObject.layer = 8;
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
            if (!_down)
            {
                if (_timer >= _knockbackTime)
                {
                    _h = Input.GetAxisRaw("Horizontal");
                    _v = Input.GetAxisRaw("Vertical");
                    Vector2 dir = new Vector2(_h, _v).normalized;
                    if (_powerup)
                    {
                        _rb.velocity = dir * _moveSpeed * 1.5f;
                    }
                    else
                    {
                        _rb.velocity = dir * _moveSpeed;
                    }
                }
            }
            //ダウン時
            else
            {
                _fillimage.color = _yellow;
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    _life += _recoveryLife;
                    if (_life >= _maxLife)
                    {
                        _fillimage.color = originalColor;
                        _life = _maxLife;
                        this.gameObject.layer = 8;
                        _down = false;
                    }
                }
                if (_timer >= _knockbackTime)
                {
                    _rb.velocity = new Vector2(0, 0);
                }
            }

            if (_magicPower >= _maxMagicPower)
            {
                _powerup = true;
                _magicPower = _maxMagicPower;
            }

            if (_powerup)
            {
                _magicPower -= _decreaseMagicPower;
                if (_magicPower <= 0)
                {
                    _powerup = false;
                }
            }
        }
        // ライフ表示処理（ゲージ）
        _lifeGauge.value = _life;
        //Debug.Log(_lifeGauge.value);
        //MP表示処理（ゲージ）
        _magicPowerGauge.value = _magicPower;
    }

    public void OnDamagePlayer(int damage, float xDirection, float yDirection)
    {
        if (!_down)
        {
            _life = _life - damage;
            _timer = 0;
            if (_life <= 0)
            {
                _life = 0;
                _down = true;
                Invoke(nameof(Down), _knockbackTime);
            }
            //ノックバック処理
            Vector2 thisPos = transform.position;
            float xDistination = thisPos.x - xDirection;
            float yDistination = thisPos.y - yDirection;
            Vector2 knockback = new Vector2(xDistination * _knockbackForce, yDistination * _knockbackForce);
            _rb.velocity = knockback;
            _DamageText.text = _playerAttackPower.ToString();

            Vector2 damageTextPosition = new Vector2(xDirection, yDirection + 1.5f);
            Instantiate(_DamageText, damageTextPosition, Quaternion.identity,_canvas.transform);
        }
    }

    public static void GetMP(float mp)
    {
        _magicPower += mp;
    }
    void Down()
    {
        this.gameObject.layer = 7;
        _rb.velocity = new Vector2(0, 0);
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
        _pause = false;
        _rb.velocity = _velocity;
        //Debug.Log("RESUME");
    }
}
