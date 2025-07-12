using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Tooltip("敵のHP")]
    [SerializeField] int _enemyHP = 5;
    [Tooltip("攻撃力")]
    [SerializeField] int _enemyAttackPower = 1;
    [SerializeField] GameObject _player;
    [Tooltip("スピード")]
    [SerializeField] float _speed;
    [Tooltip("ノックバックの強さ")]
    [SerializeField] float _knockbackForce;
    [Tooltip("ノックバックの長さ")]
    [SerializeField] float _knockbackTime;
    PlayerController _playerController;
    GameObject _castle;
    Rigidbody2D _rb;
    bool _knockback = false;
    float _timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = _player.GetComponent<PlayerController>();
        _castle = GameObject.FindGameObjectWithTag("Castle");
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _knockbackTime)
        {
            _knockback = false;
        }
        if (!_knockback)
        {
            Homing();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Vector2 thisPos = transform.position;
            float xPos = thisPos.x;
            float yPos = thisPos.y;
            _playerController.OnDamage(_enemyAttackPower, xPos, yPos);
            if (_enemyHP > 0)
            {
                _enemyHP -= _playerController._playerAttackPower;
            }
            if (_enemyHP <= 0)
            {
                Destroy(this.gameObject);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Castle")
        {
            Debug.Log("ノックバック");
            _knockback = true;
            _timer = 0;
            Vector2 thisPos = transform.position;
            Vector2 castlePos = _castle.transform.position;
            float xDistination = thisPos.x - castlePos.x;
            float yDistination = thisPos.y - castlePos.y;
            Vector2 knockback = new Vector2(xDistination, yDistination);
            _rb.velocity = knockback * _knockbackForce;
        }
    }
    void Homing()
    {
        Vector2 castlePos = _castle.transform.position;
        float x = castlePos.x;
        float y = castlePos.y;
        Vector2 direction = new Vector2(x - transform.position.x, y - transform.position.y).normalized;
        _rb.velocity = direction * _speed;
    }
}
