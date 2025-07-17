using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //リザルト画面にゲームオーバー時のコイン数を保存
        int resultCoin = EventManager._currentCoin;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
