using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIManager : MonoBehaviour
{
    bool _pauseFlg = false;

    void Update()
    {
        // ESC キーが押されたら一時停止・再開を切り替える
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PauseResume();
        }
    }
    void PauseResume()
    {
        _pauseFlg = !_pauseFlg;

        // 全ての GameObject を取ってきて、IPause を継承したコンポーネントが追加されていたら Pause または Resume を呼んでいる。
        // 本来は tag などで絞り込んだ方がよいでしょう。
        var objects = FindObjectsOfType<GameObject>();

        foreach (var o in objects)
        {
            IPause i = o.GetComponent<IPause>();

            if (_pauseFlg)
            {
                i?.Pause();     // ここで「多態性」が使われている（? は「null 条件演算子」）
            }
            else
            {
                i?.Resume();    // ここで「多態性」が使われている（? は「null 条件演算子」）
            }
        }
    }
}
