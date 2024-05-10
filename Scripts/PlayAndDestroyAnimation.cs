using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Animator animator; // Animator 컴포넌트를 참조할 변수
    public string animationName; // 재생할 애니메이션 클립의 이름

    void Start()
    {
        // Animator 컴포넌트가 없으면 경고를 출력하고 스크립트를 비활성화
        if (animator == null)
        {
            Debug.LogWarning("Animator component is not set.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // 애니메이션이 종료되면 해당 게임 오브젝트 삭제
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1f)
        {
            if(gameObject.tag == "Splash")
            {
                ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.splash).SetActive(false);
            }

            else if(gameObject.tag == "Explose")
            {
                ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.explose).SetActive(false);
            }
            
        }
    }

    // 애니메이션 재생을 시작하는 메서드
    public void PlayAnimation()
    {
        // Animator 컴포넌트에서 해당 애니메이션 클립을 재생
        animator.Play(animationName);
    }
}
