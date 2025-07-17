using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField] Text _gameoverText = default;
    [SerializeField] Text _coinText = default;
    public static int _currentCoin = 0;
    void Start()
    {
        _gameoverText.text = "";
    }
    private void Update()
    {
        _coinText.text = _currentCoin.ToString();
    }
    public  void Gameover()
    {
        _gameoverText.text = "GAME OVER";
    }

    public static void GetCoin(int coin)
    {
        _currentCoin += coin;
    }
}
