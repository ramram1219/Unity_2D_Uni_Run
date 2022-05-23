using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
   public AudioClip deathClip; // 사망시 재생할 오디오 클립
   public float jumpForce = 700f; // 점프 힘

   private int jumpCount = 0; // 누적 점프 횟수
   private bool isGrounded = false; // 바닥에 닿았는지 나타냄
   private bool isDead = false; // 사망 상태

   private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
   private Animator animator; // 사용할 애니메이터 컴포넌트
   private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

   private void Start() 
   {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
   }

   private void Update() 
   {
       if(isDead)
	   {
            return;
   	   }

        /*
          Input.GetMouseButtonDown( ) -> 마우스 버튼 입력을 감지. 입력을 감지하면 true, 아니면 false를 반환
          0 : 마우스 왼쪽 버튼
          1 : 마우스 오른쪽 버튼
          2: 마우스 휠 스크롤 버튼

         Input.GetMouseButtonUp( ) -> 마우스 버튼에서 손을 땐 경우 실행
         */
        if (Input.GetMouseButtonDown(0) && jumpCount < 4)
	    {
            jumpCount++;

            // 점프 직전 속도를 (0, 0)으로 변경 Vector2.zero == new Vector2(0, 0)
            playerRigidbody.velocity = Vector2.zero;
            // 위쪽으로 힘 주기
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            // 오디오 재생
            playerAudio.Play();


	    }
        else if(Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0)  
		{
            // y 방향 속도 값이 -일 경우(낙하하는 순간)는 속도를 줄이지 않는다  ** 속도와 위치를 헷갈리지 말것
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;
		}

        // Grounded 파라미터를 isGrounded 값으로 갱신
        animator.SetBool("Grounded", isGrounded);
   }

   private void Die() 
   {
        // 애니메이터의 Die 트리거 파라미터를 set
        animator.SetTrigger("Die");

        // 오디오 소스에 할당된 오디오 클립을 deathClip으로 변경
        playerAudio.clip = deathClip;
        // 사망 효과음 재생
        playerAudio.Play();

        playerRigidbody.velocity = Vector2.zero;
        isDead = true;

        GameManager.instance.OnPlayerDead();
   }

   // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
   private void OnTriggerEnter2D(Collider2D other) 
   {
       if(other.tag == "Dead" && !isDead)
	   {
           Die();
	   }
   }

   // 바닥에 닿았음을 감지하는 처리
   private void OnCollisionEnter2D(Collision2D collision)
   {
        /*
          Collision 충돌 이벤트는 충돌 정보를 담는 Collision 타입의 데이터를 받는다
          충돌 지점의 정보는 contacts라는 배열 변수로 저장됨 -> contacts 배열의 길이는 충돌 지점의 개수와 일치
          
          첫 번쨰 충돌 지점의 정보를 가져와서 충돌 표면의 방향(노말벡터)을 알려주는 normal을 사용
          노말 벡터 y의 값을 이용하여 절벽이나 천장을 바닥으로 인식하는 문제를 방지
         */
        if (collision.contacts[0].normal.y > 0.7f)
	    {
             isGrounded = true;
             jumpCount = 0;
	    }
   }

   // 바닥에서 벗어났음을 감지하는 처리
   private void OnCollisionExit2D(Collision2D collision) 
   {
        isGrounded = false;
   }
}