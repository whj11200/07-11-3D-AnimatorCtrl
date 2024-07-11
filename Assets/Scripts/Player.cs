using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Animator))]
// �� ���� ������Ʈ���� Animator ���۳�Ʈ�� ������ �ȵȴ�.
// �����ڰ� �Ǽ��� Animator ���۳�Ʈ�� ���� �ϸ� ��� ����.
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
    bool is_Attack = false; // �������̳� �ƴϳ�
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
            {                  // ������ٵ��� �ӵ�
                Vector3 speed = rb.velocity;
                speed.x = 4 * h;
                speed.z = 4 * v;
                rb.velocity = speed;
                // ���� �������ٸ�
                if (h != 0f && v != 0f)
                {
                    // ���� ȸ�����ȿ� �۷ι� Vector3 �� (h.x v.x)������ ȸ���ϰ� �ȴ�.
                    // ��ġ �е� ���� ��� ���ư���.
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

        //�޺����� ������ �ð� ����
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
