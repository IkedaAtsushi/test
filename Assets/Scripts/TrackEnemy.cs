using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackEnemy : EnemyBase
{
    Rigidbody2D _rb;
    GameObject _player;
    float _trackSpeed;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    public override void Homing()
    {
        Vector2 playerPos = _player.transform.position;
        float x = playerPos.x;
        float y = playerPos.y;
        Vector2 direction = new Vector2(x - transform.position.x, y - transform.position.y).normalized;
        _rb.velocity = direction * _trackSpeed;
    }
}
