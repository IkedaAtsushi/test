using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager2D : MonoBehaviour
{
    /// <summary>true の時は一時停止とする</summary>
    bool _pauseFlg = false;
    /// <summary>一時停止・再開を制御する関数の型（デリゲート）を定義する</summary>
    public delegate void Pause(bool isPause);
    /// <summary>デリゲートを入れておく変数</summary>
    Pause _onPauseResume = default;

    EventManager _eventManager;
    /// <summary>
    /// 一時停止・再開を入れるデリゲートプロパティ
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
    /// 一時停止・再開を切り替える
    /// </summary>
    public void PauseResume()
    {
        _pauseFlg = !_pauseFlg;
        _onPauseResume(_pauseFlg);  // これで変数に代入した関数を（全て）呼び出せる
    }
}
