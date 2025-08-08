using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIManager : MonoBehaviour
{
    bool _pauseFlg = false;

    void Update()
    {
        // ESC �L�[�������ꂽ��ꎞ��~�E�ĊJ��؂�ւ���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PauseResume();
        }
    }
    void PauseResume()
    {
        _pauseFlg = !_pauseFlg;

        // �S�Ă� GameObject ������Ă��āAIPause ���p�������R���|�[�l���g���ǉ�����Ă����� Pause �܂��� Resume ���Ă�ł���B
        // �{���� tag �Ȃǂōi�荞�񂾕����悢�ł��傤�B
        var objects = FindObjectsOfType<GameObject>();

        foreach (var o in objects)
        {
            IPause i = o.GetComponent<IPause>();

            if (_pauseFlg)
            {
                i?.Pause();     // �����Łu���Ԑ��v���g���Ă���i? �́unull �������Z�q�v�j
            }
            else
            {
                i?.Resume();    // �����Łu���Ԑ��v���g���Ă���i? �́unull �������Z�q�v�j
            }
        }
    }
}
