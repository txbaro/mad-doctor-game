using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Vector3 tempScale;
    private int currentAnimation;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public bool IsAnimationPlaying(string animationName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    public void PlayAnimation(string animationName)
    {
        int animHash = Animator.StringToHash(animationName);

        if (currentAnimation == animHash)
        {
            return;
        }
        
        anim.Play(animationName);
        currentAnimation = animHash;
    }

    public void SetFacingDirection(bool faceRight)
    {
        tempScale = transform.localScale;
        tempScale.x = faceRight ? 1f : -1f;
        transform.localScale = tempScale;
    }
}