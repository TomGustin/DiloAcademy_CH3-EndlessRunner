using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimation(string anim_id, bool value)
    {
        animator.SetBool(anim_id, value);
    }

    public void UpdateAnimation(string anim_id, int value)
    {
        animator.SetInteger(anim_id, value);
    }

    public void UpdateAnimation(string anim_id, float value)
    {
        animator.SetFloat(anim_id, value);
    }
}
