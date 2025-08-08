using UnityEngine;

/**
 * キャラの向き状態を変更するController
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
    void Update()
    {
        // 入力からVector2インスタンスを作成
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");
        Vector2 vector = new Vector2(_h, _v).normalized;

        if (!Input.anyKeyDown)
        {
            SetStateToAnimator(vector: vector != Vector2.zero ? vector : (Vector2?)null);
        }
    }

    /**
     * Animatorに状態をセットする
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