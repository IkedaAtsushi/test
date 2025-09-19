using UnityEngine;
using DG.Tweening;


public class EnemyBase : MonoBehaviour
{
    [Tooltip("�G��HP")]
    [SerializeField] int _enemyHP = 5;
    [Tooltip("�U����")]
    [SerializeField] int _enemyAttackPower = 1;
    [Tooltip("�X�s�[�h")]
    [SerializeField] float _speed;
    [Tooltip("�m�b�N�o�b�N�̋���")]
    [SerializeField] float _knockbackForce;
    [Tooltip("�m�b�N�o�b�N�̒���")]
    [SerializeField] float _knockbackTime;
    [Tooltip("�R�C���h���b�v��")]
    [SerializeField] int _minimumCoin;
    [SerializeField] int _maxCoin;
    [SerializeField] GameObject _coin;
    [SerializeField] GameObject _magic;
    [SerializeField] AudioClip _damageSE;
    int _dropCoin;
    GameObject _player;
    Sprite idolSprite;
    PlayerController _playerController;
    CastleControler _castleControler;
    GameObject _castle;
    Rigidbody2D _rb;
    SpriteRenderer _sr;
    Animator _anim;
    AudioSource _audioSource;
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
    public void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _castle = GameObject.FindGameObjectWithTag("Castle");
        _castleControler = _castle.GetComponent<CastleControler>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        idolSprite = _sr.sprite;
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
            if (_timer >= _knockbackTime)
            {
                _knockback = false;
            }
            if (!_knockback)
            {
                Homing();
            }
        }
        //�X�v���C�g���]
        if (this.transform.position.x < 0 && !_sr.flipX)
        {
            _sr.flipX = true;
        }

        if (this.transform.position.x > 0 && _sr.flipX)
        {
            _sr.flipX = false;
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
                    if (_playerController._powerup)
                    {
                        _audioSource.PlayOneShot(_damageSE);
                    }
                    if (_enemyHP > 0)
                    {

                        if (_playerController._powerup)
                        {
                            _playerController.OnDamagePlayer(0, xPos, yPos, true);
                            _knockback = true;
                            _timer = 0;
                            float xDis = thisPos.x - _player.transform.position.x;
                            float yDis = thisPos.y - _player.transform.position.y;
                            Vector2 kb = new Vector2(xDis, yDis);
                            _rb.velocity = kb * _knockbackForce * 1.4f;

                            float p = (float)_playerController._playerAttackPower;
                            p *= 1.1f;
                            int _p = (int)p;
                            _enemyHP -= _p;
                        }
                        else
                        {
                            _playerController.OnDamagePlayer(_enemyAttackPower, xPos, yPos,false);
                            _enemyHP -= _playerController._playerAttackPower;
                            _initPosition = transform.position;
                            StartShake(_duration, _strength, _vibrato, _randomness, _fadeOut);
                        }
                    }
                    if (_enemyHP <= 0)
                    {
                        _dropCoin = Random.Range(_minimumCoin, _maxCoin + 1);
                        for (int i = 0; i < _dropCoin; i++)
                        {
                            Instantiate(_coin, transform.position, Quaternion.identity);
                        }
                        int m = Random.Range(0, 2);
                        if (m == 0)
                        {
                            Instantiate(_magic, transform.position, Quaternion.identity);
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
                //Debug.Log("�m�b�N�o�b�N");
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
        // �O��̏������c���Ă���Β�~���ď����ʒu�ɖ߂�
        if (_shakeTweener != null)
        {
            _shakeTweener.Kill();
            gameObject.transform.position = _initPosition;
        }
        // �h��J�n
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
        // ���x�E��]��ۑ����ARigidbody ���~����

        _velocity = _rb.velocity;
        _rb.Sleep();
        _pause = true;
        _anim.enabled = false;
        //Debug.Log("PAUSE");
    }

    public void Resume()
    {
        // Rigidbody �̊������ĊJ���A�ۑ����Ă��������x�E��]��߂�
        _rb.WakeUp();
        _pause = false;
        _rb.velocity = _velocity;
        _anim.enabled = true;
        //Debug.Log("RESUME");
    }
}
