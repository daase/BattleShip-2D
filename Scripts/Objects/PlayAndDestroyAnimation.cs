using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Animator animator; // Animator ������Ʈ�� ������ ����
    public string animationName; // ����� �ִϸ��̼� Ŭ���� �̸�
    public SpriteRenderer renderer;
    public AudioSource audioSource;
    void Start()
    {
        // Animator ������Ʈ�� ������ ����� ����ϰ� ��ũ��Ʈ�� ��Ȱ��ȭ
        if (animator == null)
        {
            Debug.LogWarning("Animator component is not set.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // �ִϸ��̼��� ����Ǹ� �ش� ���� ������Ʈ ��Ȱ��ȭ
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1f)
        {
            if(gameObject.tag == "Splash")
            {
                ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.splash).SetActive(false);
   
            }

            else if(gameObject.tag == "Explose")
            {
                renderer.enabled = false;

                if (!audioSource.isPlaying) 
                {
                    renderer.enabled = true;
                    ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.explose).SetActive(false);                   
                } 
            }          
        }
    }

    // �ִϸ��̼� ����� �����ϴ� �޼���
    public void PlayAnimation()
    {
        // Animator ������Ʈ���� �ش� �ִϸ��̼� Ŭ���� ���
        animator.Play(animationName);
    }
}