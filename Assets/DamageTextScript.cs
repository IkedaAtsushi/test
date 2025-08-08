using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float _displaytime = 0.4f;
    private float _timer;
    private Vector3 pos;
    private void Awake()
    {
        pos = transform.position;
    }
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _displaytime)
            Destroy(gameObject);

        pos.y += 0.01f;
        transform.position = pos;
    }
}
