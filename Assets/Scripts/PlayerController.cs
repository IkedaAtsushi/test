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
    [SerializeField] int _maxMagicPower;
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
    public int _life;
    public static int _magicPower;
    float _timer;
    public bool down = false;
    Rigidbody2D _rb = default;
    Animator _animator;
    float _h;
    float _v;
    Image _fillimage;
    Color _yellow = Color.yellow;
    Color originalColor;
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

    // Update is called once per frame
    void Update()
    {

        _timer += Time.deltaTime;
        if (!down)
        {
            if (_timer >= _knockbackTime)
            {
                _h = Input.GetAxisRaw("Horizontal");
                _v = Input.GetAxisRaw("Vertical");
                Vector2 dir = new Vector2(_h, _v).normalized;
                _rb.velocity = dir * _moveSpeed;
            }
        }

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
                    down = false;
                }
            }
            if (_timer >= _knockbackTime)
            {
                _rb.velocity = new Vector2(0, 0);
            }
        }
        // ���C�t�\�������i�Q�[�W�j
        _lifeGauge.value = _life;
        //Debug.Log(_lifeGauge.value);
        _magicPowerGauge.value = _magicPower;
    }

    public void OnDamagePlayer(int damage, float xDirection, float yDirection)
    {
        if (!down)
        {
            _life = _life - damage;
            _timer = 0;
            if (_life <= 0)
            {
                _life = 0;
                down = true;
                Invoke(nameof(Down), _knockbackTime);
            }
            Vector2 thisPos = transform.position;
            float xDistination = thisPos.x - xDirection;
            float yDistination = thisPos.y - yDirection;
            Vector2 knockback = new Vector2(xDistination * _knockbackForce, yDistination * _knockbackForce);
            _rb.velocity = knockback;
        }
    }

    public static void GetMP(int mp)
    {
        _magicPower += mp;
    }
    void Down()
    {
        this.gameObject.layer = 7;
        _rb.velocity = new Vector2(0, 0);
    }
}
