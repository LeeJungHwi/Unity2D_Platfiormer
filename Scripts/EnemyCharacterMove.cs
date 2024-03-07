using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterMove : MonoBehaviour
{
    // 컴포넌트
    public int nextMove;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;

    void Awake() // 초기화
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        Invoke("Think", 5);
    }

    void FixedUpdate() // AI
    {
        // 이동
        rigid.velocity = new Vector2(nextMove * -1, rigid.velocity.y);

        // 플랫폼 체크
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * -0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null) // 빛을 맞은 객체의 정보가 없다면
        {
            Turn(); // 턴 함수 호출
        }
    }

    void Think() // 다음이동을 생각하는 함수
    {
        // 다음 이동
        nextMove = Random.Range(-1, 2);

        // 에니메이션
        anim.SetInteger("Run Speed", nextMove);

        // 방향전환
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        // 랜덤 생각시간
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn() // 방향을 강제전환하는 함수
    {
        // 반대방향으로
        nextMove *= -1;

        // 방향전환
        spriteRenderer.flipX = nextMove == 1;

        // 현재 초를 세고있는 Invoke함수를 멈춘다
        CancelInvoke();

        // 다시 생각
        Invoke("Think", 2);
    }

    public void OnDamaged() // 죽는 함수
    {
        // 투명
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 방향전환 Y축
        spriteRenderer.flipY = true;

        // 캡슐 콜라이더 비활성화
        capsuleCollider.enabled = false;

        // 죽을때 점프 효과
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // 적캐릭터 소멸
        Invoke("DeActive", 5);
    }

    void DeActive() // 오브젝트를 없애는 함수
    {
        gameObject.SetActive(false);
    }
}
