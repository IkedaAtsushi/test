using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �L�����N�^�[�𑀍삷��R���|�[�l���g
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("�X�e�[�^�X")]
    [Tooltip("�̗͂̍ő�l")]
    [SerializeField] int _maxLife;
    [Tooltip("�̗͂�\������Q�[�W")]
    [SerializeField] Slider _lifeGauge;
    [Tooltip("�̗͂̉񕜗�")]
    [SerializeField] int _recoveryLife;
    [Tooltip("MP�̍ő�l")]
    [SerializeField] float _maxMagicPower;
    [Tooltip("MP�̌������x")]
    [SerializeField] float _decreaseMagicPower;
    [Tooltip("MP��\������Q�[�W")]
    [SerializeField] Slider _magicPowerGauge;
    [Tooltip("�U����")]
    [SerializeField] public int _playerAttackPower;
    [Tooltip("�v���C���[�̑���")]
    [SerializeField] float _moveSpeed = 5f;
    [Tooltip("�m�b�N�o�b�N�̋���")]
    [SerializeField] float _knockbackForce = 3f;
    [Tooltip("�m�b�N�o�b�N�̒���")]
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
        _pauseManager = GameObject.FindObjectOfType<PauseManager2D>();  // ���̏����� Start ���ƒx���̂� Awake �ōs���BOnEnable �̕��� Start ����ɌĂ΂�邽�߁B
    }
    void OnEnable()
    {
        // �Ă�ŗ~�������\�b�h��o�^����B
        _pauseManager.OnPauseResume += PauseResume;
    }
    void OnDisable()
    {
        // OnDisable �ł̓��\�b�h�̓o�^���������邱�ƁB�����Ȃ��ƃI�u�W�F�N�g�������ɂ��ꂽ��j�����ꂽ�肵����ɃG���[�ɂȂ��Ă��܂��B
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
            //�_�E����
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
        // ���C�t�\�������i�Q�[�W�j
        _lifeGauge.value = _life;
        //Debug.Log(_lifeGauge.value);
        //MP�\�������i�Q�[�W�j
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
            //�m�b�N�o�b�N����
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
        // ���x�E��]��ۑ����ARigidbody ���~����

        _velocity = _rb.velocity;
        _rb.Sleep();
        _pause = true;
        //Debug.Log("PAUSE");
    }

    public void Resume()
    {
        // Rigidbody �̊������ĊJ���A�ۑ����Ă��������x�E��]��߂�
        _rb.WakeUp();
        _pause = false;
        _rb.velocity = _velocity;
        //Debug.Log("RESUME");
    }
}
