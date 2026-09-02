using UnityEngine;
using UnityEngine.Animations;

public class PlayerMove : MonoBehaviour
{
    // 목적: 키보드 입력에 따른 플레이어 이동 처리

    public float Speed = 5f; // 이동 속력

    // 왼쪽 방향을 의미하는 벡터
    Vector2 direction = new Vector2(-1, 0); // = Vector2 direction = Vector2.left);

    // 매 프레임마다 실행
    // 초당 프레임 실행 횟수는 별다른 설정이 없으면 가능한 만큼 실행
    private void Update()
    {

        // 1, 키보드 입력을 받는다.
        float h = Input.GetAxis("Horizontal"); // 좌우 -1 ~ 1 사이의 값 반환
        float v = Input.GetAxis("Vertical");   // 상하 -1 ~ 1 사이의 값 반환

        Debug.Log($"{h}, {v}");

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("왼쪽 방향키를 누르고 있습니다.");
            // 속도 = 방향 * 속력          //매직넘버 : 보는 사람에 따라 의미가 달라질 수 있는 헷갈리는 숫자    
            transform.Translate(direction * Speed * Time.deltaTime);
            // deltaTime : 이전 프레임이 끝나고 다음 프레임이 시작될 때까지 걸린 시간을 Ms로 반환
        }
        // 2. 키보드 입력에 따라 방향을 구한다.
        // 게임에는 벡터라는 타입이 있음. 벡터는 크기와 방향을 의미함.



        // 3. 방향과 속도에 따라 이동한다.

    }
}
