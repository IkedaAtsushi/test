using UnityEngine;

/**
 * キャラの向き状態を変更するController
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