using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Text _resultText;
    public static int _highScore;
    public static string _highScoreText;
    private void OnEnable()
    {
        GameObject eventmanager = GameObject.Find("EventManager");
        EventManager em = eventmanager.GetComponent<EventManager>();
        _resultText.text = em.minute.ToString("00") + ":" + ((int)em.seconds).ToString("00");
        if(_highScore < em.minute * 60 + (int)em.seconds)
        {
            _highScore = em.minute * 60 + (int)em.seconds;
            _highScoreText = _resultText.text;
        }
    }

}
