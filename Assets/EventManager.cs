using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField] Text _gameoverText = default;
    void Start()
    {
        _gameoverText.text = "";
    }
    public  void Gameover()
    {
        _gameoverText.text = "GAME OVER";
    }

}
