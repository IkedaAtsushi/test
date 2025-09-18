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
        _pauseManager = GameObject.FindObjectOfType<PauseManager2D>();  // ���̏����� Start ���ƒx���̂� Awake �ōs���BOnEnable �̕��� Start ����ɌĂ΂�邽�߁B
    }
    void OnEnable()
    {
        // �Ă�ŗ~�������\�b�h��o�^����B
        _pauseManager.OnPauseResume += PauseResume;
    }
    void OnDisable()
    {
        // OnDisable �ł̓��\�b�h�̓o�^���������邱�ƁB�����Ȃ��ƃI�u�W�F�N�g�������ɂ��ꂽ��j�����ꂽ�肵����ɃG���[�ɂȂ��Ă��܂��B
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
        // ���x�E��]��ۑ����ARigidbody ���~����

        _velocity = _rb.velocity;
        _rb.Sleep();
        //Debug.Log("PAUSE");
    }

    public void Resume()
    {
        // Rigidbody �̊������ĊJ���A�ۑ����Ă��������x�E��]��߂�
        _rb.WakeUp();
        _rb.velocity = _velocity;
        //Debug.Log("RESUME");
    }
}
