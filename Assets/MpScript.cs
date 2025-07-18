using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpScript : ItemBase
{
    [SerializeField] int _mp;
    // Start is called before the first frame update
    private void OnDestroy()
    {
        PlayerController.GetMP(_mp);
    }
}
