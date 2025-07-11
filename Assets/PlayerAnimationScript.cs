using UnityEngine;

/**
 * �L�����̌�����Ԃ�ύX����Controller
 */
public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    float _h;
    float _v;
    void Start()
    {
        this.animator = GetComponent<Animator>();
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

}