using UnityEngine;

// 왼쪽 끝으로 이동한 배경을 오른쪽 끝으로 재배치하는 스크립트
public class BackgroundLoop : MonoBehaviour {
    private float width; // 배경의 가로 길이

    // Start처럼 초기 1회 자동 실행되는 유니티 이벤트 메서드
    // Start보다 한 프레임 더 빠르게 실행 됨.
    // 가로 길이를 측정하는 처리
    private void Awake() 
    {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x;
    }

    // 현재 위치가 원점에서 왼쪽으로 width 이상 이동했을때 위치를 리셋
    private void Update() 
    {
        // Background가 왼쪽으로 이동하므로 -width 길이와 비교
        if(transform.position.x <= -width)
		{
            Reposition();
		}
    }

    // 위치를 리셋하는 메서드
    private void Reposition() 
    {
        // 현재 위치에서 오른쪽으로 width * 2 길이 만큼 이동
        Vector2 offset = new Vector2(width * 2f, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}