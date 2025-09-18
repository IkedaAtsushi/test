using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PauseManager2D;

public class ItemBase : MonoBehaviour
{
    Rigidbody2D _rb;
    PauseManager2D _pauseManager = default;
    AudioSource _audioSource;
    [SerializeField] AudioClip _getSE;
    Vector3 _velocity;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        float x = Random.Range(-10, 10);
        float y = Random.Range(-10, 10);
        _rb.velocity = new Vector2(x, y);
    }
    void Awake()
    {
        _pauseManager = GameObject.FindObjectOfType<PauseManager2D>();  // この処理は Start やると遅いので Awake で行う。OnEnable の方が Start より先に呼ばれるため。
    }
    void OnEnable()
    {
        // 呼んで欲しいメソッドを登録する。
        _pauseManager.OnPauseResume += PauseResume;
    }
    void OnDisable()
    {
        // OnDisable ではメソッドの登録を解除すること。さもないとオブジェクトが無効にされたり破棄されたりした後にエラーになってしまう。
        _pauseManager.OnPauseResume -= PauseResume;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_getSE, this.transform.position);
            Destroy(this.gameObject);
        }
    }
    void PauseResume(bool isPause)
    {
        if (isPause)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }
    public void Pause()
    {
        // 速度・回転を保存し、Rigidbody を停止する

        _velocity = _rb.velocity;
        _rb.Sleep();
        //Debug.Log("PAUSE");
    }

    public void Resume()
    {
        // Rigidbody の活動を再開し、保存しておいた速度・回転を戻す
        _rb.WakeUp();
        _rb.velocity = _velocity;
        //Debug.Log("RESUME");
    }
}
