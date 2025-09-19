using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CastleControler : MonoBehaviour
{
    [Header("�X�e�[�^�X")]
    [Tooltip("�̗͂̍ő�l")]
    [SerializeField] int _maxLife;
    [Tooltip("�̗͂�\������Q�[�W")]
    [SerializeField] Slider _lifeGauge;
    int _life;
    [SerializeField] GameObject _eventManager;
    EventManager _em;
    AudioSource _audioSource;
    [SerializeField] AudioClip _damage;

    [SerializeField] float _duration;
    [SerializeField] float _strength;
    [SerializeField] int _vibrato;
    [SerializeField] float _randomness;
    bool _fadeOut;
    bool _gameover = false;
    private Tweener _shakeTweener;
    private Vector3 _initPosition;
    // Start is called before the first frame update
    void Start()
    {
        _life = _maxLife;
        _lifeGauge.maxValue = _maxLife;
        _em = _eventManager.GetComponent<EventManager>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���C�t�\�������i�Q�[�W�j
        _lifeGauge.value = _life;
    }
    public void OnDamageCastle(int damage)
    {
        _life -= damage;
        _audioSource.PlayOneShot(_damage);
        StartShake(_duration, _strength, _vibrato, _randomness, _fadeOut);
        if (_life <= 0 && !_gameover)
        {
            _gameover = true;
            _em.Gameover();
        }
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
}
