using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]GameObject _eventManager;
    EventManager _em;
    // Start is called before the first frame update
    void Start()
    {
        _life = _maxLife;
        _lifeGauge.maxValue = _maxLife;
        _em = _eventManager.GetComponent<EventManager>();
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
        if (_life <= 0)
        {
            _em.Gameover();
        }
    }
}
