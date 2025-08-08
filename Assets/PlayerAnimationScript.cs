using UnityEngine;

/**
 * �L�����̌�����Ԃ�ύX����Controller
 */
public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    float _h;
    float _v;

    PauseManager2D _pauseManager = default;
    bool _pause = false;
    void Start()
    {
        this.animator = GetComponent<Animator>();
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
    void Update()
    {
        // ���͂���Vector2�C���X�^���X���쐬
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");
        Vector2 vector = new Vector2(_h, _v).normalized;

        if (!Input.anyKeyDown)
        {
            SetStateToAnimator(vector: vector != Vector2.zero ? vector : (Vector2?)null);
        }
    }

    /**
     * Animator�ɏ�Ԃ��Z�b�g����
     *    
     */
    private void SetStateToAnimator(Vector2? vector)
    {
        if (!_pause)
        {
            if (!vector.HasValue)
            {
                this.animator.speed = 0.0f;
                return;
            }

            Debug.Log(vector.Value);
            this.animator.speed = 1.0f;
            this.animator.SetFloat("x", vector.Value.x);
            this.animator.SetFloat("y", vector.Value.y);
        }
        else
        {
            this.animator.speed = 0.0f;
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
        _pause = true;
    }
    public void Resume()
    {
        _pause = false;
    }
}