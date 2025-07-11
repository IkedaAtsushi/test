using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Tooltip("敵のHP")]
    [SerializeField] int _enemyHP = 5;
    [Tooltip("攻撃力")]
    [SerializeField] int _enemyAttackPower = 1;
    [SerializeField] GameObject _player;
    [Tooltip("スピード")]
    [SerializeField] float speed;
    PlayerController _playerController;
    Transform _CastleTr;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = _player.GetComponent<PlayerController>();
        _CastleTr = GameObject.FindGameObjectWithTag("Castle").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Homing();
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

    void Homing()
    {
        transform.position = Vector2.MoveTowards(
           transform.position,
           new Vector2(_CastleTr.position.x, _CastleTr.position.y),
           speed * Time.deltaTime);
    }
}
