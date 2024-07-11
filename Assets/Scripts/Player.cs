using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Animator))]
// 이 게임 오브젝트에서 Animator 컴퍼넌트가 없으면 안된다.
// 개발자가 실수로 Animator 컴퍼넌트를 삭제 하면 경고를 띄운다.
public class Player : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected Rigidbody rb;
    float h = 0f,v = 0f;
    [SerializeField]
    protected AudioSource audioSource;
    [SerializeField]
    protected AudioClip clip;
    float lastAttackTime = 0f;
    bool is_Attack = false; // 공격중이냐 아니냐
    bool is_Deshing = false;
    bool IS_Skill = false;
    
    IEnumerator Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        yield return null;
        audioSource = GetComponent<AudioSource>();
    }
    public void OnStickPos(Vector3 Stickpos)
    {
        h = Stickpos.x;
        v = Stickpos.y;
        
    }
  
    void Update()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", (h * h + v * v));
            if (rb != null)
            {                  // 리지드바디의 속도
                Vector3 speed = rb.velocity;
                speed.x = 4 * h;
                speed.z = 4 * v;
                rb.velocity = speed;
                // 만약 움직였다면
                if (h != 0f && v != 0f)
                {
                    // 현재 회전값안에 글로벌 Vector3 값 (h.x v.x)값으로 회전하게 된다.
                    // 터치 패드 방향 대로 돌아간다.
                    transform.rotation = Quaternion.LookRotation(new Vector3(h, 0f, v));
                }
            }

        }
    }
    public void SkillAttack()
    {
        IS_Skill = true;
        if (Time.time - lastAttackTime > 1f)
        {
            animator.SetTrigger("SkillTrigger");
        }
            
        audioSource.PlayOneShot(clip,1f);
        
    }
    public void SkillAttackUp()
    {
        IS_Skill = false;

    }
    public void OnAttackDown()
    {
        is_Attack = true;
        animator.SetBool("Combo", is_Attack);
        StartCoroutine(ComoboAttackTiming());
    }
    public void OnAttackUp()
    {
        is_Attack = false;
        animator.SetBool("Combo", is_Attack);

    }


    IEnumerator ComoboAttackTiming()
    {

        //콤보어택 까지의 시간 간격
        if (Time.time - lastAttackTime > 1f)
        {
            lastAttackTime = Time.time;
            while (is_Attack)
            {
                animator.SetBool("Combo",true);
                yield return new WaitForSeconds(1f);
            }
           
        }
    }
    public void DushAttackDown()
    {
        if (Time.time - lastAttackTime > 1f)
        {
            is_Deshing = true;
            animator.SetTrigger("DushAttack");
            lastAttackTime = Time.time;
            animator.applyRootMotion = true;
        }
           
        
    }
    public void DushAttackUP()
    {
        is_Deshing = true;
        animator.applyRootMotion = false;
    }
}
