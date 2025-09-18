using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PauseManager2D;

public class EventManager : MonoBehaviour
{
    [SerializeField] Text _gameoverText = default;
    [SerializeField] Text _levelText = default;
    [SerializeField] Text _StartText = default;
    [SerializeField] Text _Timer = default;
    [SerializeField] Slider _coinSlider = default;
    [SerializeField] GameObject _pauseUIPrefab;
    GameObject _pauseUIInstance;
    public static int _currentCoin = 0;
    int _levelupCoin = 50;
    public int _currentLevel = 1;
    PauseManager2D _pauseManager;
    PauseUIManager _pauseUIManager;
    AudioSource _audioSource;
    [SerializeField]AudioClip _levelUp;
    bool _pause = false;
    int minute = 0;
    float seconds = 0f;
    float oldSeconds = 0f;
    void Start()
    {
        _gameoverText.text = "";
        _coinSlider.maxValue = _levelupCoin;

        _pauseManager = GetComponent<PauseManager2D>();
        _pauseUIManager = GetComponent<PauseUIManager>();
        _audioSource = GetComponent<AudioSource>();

        _pauseManager.PauseResume();
        StartCoroutine(GameStartCoroutine());

        minute = 0;
        seconds = 0f;
        oldSeconds = 0f;
    }
    private void Update()
    {
        if (!_pause)
        {
            seconds += Time.deltaTime;
        }
        if (seconds >= 60f)
        {
            minute++;
            seconds = seconds - 60;
        }
        //�@�l���ς�����������e�L�X�gUI���X�V
        if ((int)seconds != (int)oldSeconds)
        {
            _Timer.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
        }
        oldSeconds = seconds;

        _coinSlider.value = _currentCoin;
        _levelText.text = _currentLevel.ToString();
        if (_currentCoin >= _levelupCoin)
        {
            Levelup();
        }
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

    private IEnumerator GameStartCoroutine()
    {
        _StartText.text = "READY?";

        yield return new WaitForSeconds(1);

        _StartText.text = "START!";
        _pauseManager.PauseResume();
        yield return new WaitForSeconds(1);

        _StartText.text = "";
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1);
        _pauseManager.PauseResume();
    }
    public void Gameover()
    {
        StartCoroutine(GameOverCoroutine());
    }

    public void Levelup()
    {
        _currentLevel += 1;
        _currentCoin = 0;
        float lvUpCoin = (float)_levelupCoin;
        lvUpCoin *= 1.4f;
        _levelupCoin = (int)lvUpCoin;
        _coinSlider.maxValue = _levelupCoin;
        _audioSource.PlayOneShot(_levelUp);
        _pauseManager.PauseResume();
        _pauseUIPrefab.SetActive(true);
        //_pauseUIManager.PauseResume();
    }

    public static void GetCoin(int coin)
    {
        _currentCoin += coin;
    }
    void PauseResume(bool isPause)
    {
        if (isPause)
        {
            _pause = true;
        }
        else
        {
            _pause = false;
        }
    }
}
