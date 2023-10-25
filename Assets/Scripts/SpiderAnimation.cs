using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimation : MonoBehaviour
{
    string m_CurrentState;
    const string k_SpiderIdle = "Idle";
    const string k_HardSpiderAttack = "Attack_1";
    const string k_MediumSpiderAttack = "Attack_2";
    const string k_SpiderDead = "Death";
    const string k_SpiderDamage = "TakeDamage";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void ChangeAnimationState(Animator i_Animator, string newState)
    {
        if (newState == m_CurrentState)
        {
            return;
        }

        i_Animator.Play(newState);

        m_CurrentState = newState;
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
