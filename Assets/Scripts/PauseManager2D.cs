using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager2D : MonoBehaviour
{
    /// <summary>true �̎��͈ꎞ��~�Ƃ���</summary>
    bool _pauseFlg = false;
    /// <summary>�ꎞ��~�E�ĊJ�𐧌䂷��֐��̌^�i�f���Q�[�g�j���`����</summary>
    public delegate void Pause(bool isPause);
    /// <summary>�f���Q�[�g�����Ă����ϐ�</summary>
    Pause _onPauseResume = default;

    EventManager _eventManager;
    /// <summary>
    /// �ꎞ��~�E�ĊJ������f���Q�[�g�v���p�e�B
    /// </summary>
    public Pause OnPauseResume
    {
        get { return _onPauseResume; }
        set { _onPauseResume = value; }
    }

    private void Start()
    {
        _eventManager = GetComponent<EventManager>();
    }
   

    /// <summary>
    /// �ꎞ��~�E�ĊJ��؂�ւ���
    /// </summary>
    public void PauseResume()
    {
        _pauseFlg = !_pauseFlg;
        _onPauseResume(_pauseFlg);  // ����ŕϐ��ɑ�������֐����i�S�āj�Ăяo����
    }
}
