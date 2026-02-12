using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    [SerializeField] Text _rank1Score;
    void Start()
    {
        if (ResultManager._highScoreText == null)
        {
            _rank1Score.text = "00:00";
        }
        else
        {

            _rank1Score.text = ResultManager._highScoreText;
        }
    }

    // Update is called once per frame

}
