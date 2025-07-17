using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : ItemBase
{
    // Start is called before the first frame update
    private void OnDestroy()
    {
        EventManager.GetCoin(10);
    }
}
