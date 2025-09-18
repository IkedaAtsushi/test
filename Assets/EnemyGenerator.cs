using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] float _cooltime;
    [SerializeField] GameObject[] _level1;
    [SerializeField] Transform _rightRangeA;
    [SerializeField] Transform _rightRangeB;
    [SerializeField] Transform _leftRangeA;
    [SerializeField] Transform _leftRangeB;
    [SerializeField] Transform _upRangeA;
    [SerializeField] Transform _upRangeB;
    [SerializeField] Transform _downRangeA;
    [SerializeField] Transform _downRangeB;

    float _time;
    float _gameTimer;
    float x;
    float y;

    PauseManager2D _pauseManager = default;
    bool _pause = false;

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

    private void Start()
    {
        _time = _cooltime;
    }
    private void Update()
    {
        if (!_pause)
        {
            _time += Time.deltaTime;
            _gameTimer += Time.deltaTime;
        }
        if (_time > _cooltime)
        {
            int r = Random.Range(0, 4);
            if (r == 0)
            {
                x = Random.Range(_rightRangeA.position.x, _rightRangeB.position.x);
                y = Random.Range(_rightRangeA.position.y, _rightRangeB.position.y);
            }
            else if (r == 1)
            {
                x = Random.Range(_leftRangeA.position.x, _leftRangeB.position.x);
                y = Random.Range(_leftRangeA.position.y, _leftRangeB.position.y);
            }
            else if (r == 2)
            {
                x = Random.Range(_upRangeA.position.x, _upRangeB.position.x);
                y = Random.Range(_upRangeA.position.y, _upRangeB.position.y);
            }
            else if (r == 3)
            {
                x = Random.Range(_downRangeA.position.x, _downRangeB.position.x);
                y = Random.Range(_downRangeA.position.y, _downRangeB.position.y);
            }

            Instantiate(_level1[Random.Range(0,_level1.Length)], new Vector3(x, y, 0), Quaternion.identity);

            _time = 0;
        }
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
