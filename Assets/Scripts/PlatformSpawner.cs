using UnityEngine;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public int count = 3; // 생성할 발판의 개수

    public float timeBetSpawnMin = 1.25f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 2.25f; // 다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; // 다음 배치까지의 시간 간격

    public float yMin = -3.5f; // 배치할 위치의 최소 y값
    public float yMax = 1.5f; // 배치할 위치의 최대 y값
    private float xPos = 20f; // 배치할 위치의 x 값

    private GameObject[] platforms; // 미리 생성한 발판들
    private int currentIndex = 0; // 사용할 현재 순번의 발판

    private Vector2 poolPosition = new Vector2(0, -20); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; // 마지막 배치 시점


    // 변수들을 초기화하고 사용할 발판들을 미리 생성
    void Start() 
    {
        platforms = new GameObject[count];

        for(int i = 0; i < count; i++)
		{
            /*
              Instantiate(프리펩, 생성할 위치, 회전) -> 프리펩의 복제본을 생성
              poolPosition (0, -20) 이므로 화면의 아래쪽에 멀리 밀려나 생성되어 보이지 않음
              Quaternion.identity -> (0, 0, 0)
             */
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
		}

        lastSpawnTime = 0f;
        // 게임 실행 즉시 발판을 하나 생성
        timeBetSpawn = 0f;
    }

    // 순서를 돌아가며 주기적으로 발판을 배치
    void Update() 
    {
        if(GameManager.instance.isGameover)
		{
            return;
		}

        // Time.time -> 유니티 라이브러리. 게임 시작 후 시간이 몇 초 지났는지 출력
        // lastSpawnTime(마지막 발판이 생성된 시점) + timeBetSpawn(다음번 배치까지의 시간 간격) => 다음번 배치가 일어날 시점
        if(Time.time >= lastSpawnTime + timeBetSpawn)
		{
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            float yPos = Random.Range(yMin, yMax);

            // Platform.cs의 OnEnable() 메서드를 실행 시키기 위해 게임 오브젝트를 껏다가 킴.
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            platforms[currentIndex].transform.position = new Vector2(xPos, yPos);

            currentIndex++;

            if(currentIndex >= count)
			{
                currentIndex = 0;
			}
		}
    }
}