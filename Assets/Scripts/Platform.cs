using UnityEngine;

// 발판으로서 필요한 동작을 담은 스크립트
public class Platform : MonoBehaviour {
    public GameObject[] obstacles; // 장애물 오브젝트들
    private bool stepped = false; // 플레이어 캐릭터가 밟았었는가

    // 컴포넌트가 활성화될때 마다 매번 실행되는 메서드
    // 발판을 리셋하는 처리
    private void OnEnable() 
    {
        stepped = false;

        for(int i = 0; i < obstacles.Length; i++)
		{
            // 3개의 발판 중 랜덤값이 0 일 떄 i 값에 따라 활성화 그 외에는 비활성화
            if(Random.Range(0,3) == 0)
			{
                obstacles[i].SetActive(true);
			}
            else          
			{
                obstacles[i].SetActive(false);
			}
		}
    }

    // 플레이어 캐릭터가 자신을 밟았을때 점수를 추가하는 처리
    void OnCollisionEnter2D(Collision2D collision) 
    {
        // 충돌한 상대방의 태그가 Player이고, 이전에 플레이어가 밟지 않은 경우
        if(collision.collider.tag == "Player" && !stepped)
		{
            stepped = true;
            GameManager.instance.AddScore(1);
		}
    }
}