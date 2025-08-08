using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField] Text _gameoverText = default;
    [SerializeField] Text _levelText = default;
    [SerializeField] Slider _coinSlider = default;
    public static int _currentCoin = 0;
    int _levelupCoin = 100;
    int _currentLevel = 1;
    void Start()
    {
        _gameoverText.text = "";
        _coinSlider.maxValue = _levelupCoin;
    }
    private void Update()
    {
        _coinSlider.value = _currentCoin;
        _levelText.text = _currentLevel.ToString();
        if(_currentCoin >= _levelupCoin)
        {
            Levelup();
        }
    }
    public void Gameover()
    {
        _gameoverText.text = "GAME OVER";
    }

    public void Levelup()
    {
        _currentLevel += 1;
        _currentCoin = 0;
        float lvUpCoin =(float)_levelupCoin;
        lvUpCoin *= 1.4f;
        _levelupCoin = (int)lvUpCoin;
        _coinSlider.maxValue = _levelupCoin;
    }

    public static void GetCoin(int coin)
    {
        _currentCoin += coin;
    }
}
