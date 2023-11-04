using UnityEngine;

public class SpiderAnimation : MonoBehaviour
{
    private const string k_SpiderIdle = "Idle";
    private const string k_HardSpiderAttack = "Attack_1";
    private const string k_MediumSpiderAttack = "Attack_2";
    private const string k_SpiderDead = "Death";
    private const string k_SpiderDamage = "TakeDamage";
    private string m_CurrentState;

    public void ChangeAnimationState(Animator i_Animator, string i_NewState)
    {
        if (i_NewState == m_CurrentState)
        {
            return;
        }

        i_Animator.Play(i_NewState);
        m_CurrentState = i_NewState;
    }

    private bool isAnimationPlaying(Animator i_Animator, string i_StateName)
    {
        if (i_Animator.GetCurrentAnimatorStateInfo(0).IsName(i_StateName) /*&&
            i_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f*/)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}