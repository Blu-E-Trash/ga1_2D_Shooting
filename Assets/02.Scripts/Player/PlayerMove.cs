using UnityEngine;
using UnityEngine.Animations;

public class PlayerMove : MonoBehaviour
{
    // 목적: 키보드 입력에 따른 플레이어 이동 처리

    public float Speed = 5f; // 이동 속력



    // 매 프레임마다 실행
    // 초당 프레임 실행 횟수는 별다른 설정이 없으면 가능한 만큼 실행
    private void Update()
    {

        // 1, 키보드 입력을 받는다.
        float h = Input.GetAxisRaw("Horizontal"); // 좌우 -1 ~ 1 사이의 값 반환
        float v = Input.GetAxisRaw("Vertical");   // 상하 -1 ~ 1 사이의 값 반환

        // 2. 키보드 입력에 따라 방향을 구한다.
        // 게임에는 벡터라는 타입이 있음. 벡터는 크기와 방향을 의미함.
        // 왼쪽 방향을 의미하는 벡터
        Vector2 direction = new Vector2(h, v); // = Vector2 direction = Vector2.left);

        //Debug.Log($"{h}, {v}");
        //Debug.Log("왼쪽 방향키를 누르고 있습니다.");
        // 3. 방향과 속도에 따라 이동한다.
        // 속도 = 방향 * 속력          //매직넘버 : 보는 사람에 따라 의미가 달라질 수 있는 헷갈리는 숫자    
        //transform.Translate(direction * Speed * Time.deltaTime);
        // deltaTime : 이전 프레임이 끝나고 다음 프레임이 시작될 때까지 걸린 시간을 Ms로 반환
        Vector2 normalizedSpeed = (direction * Speed).normalized;

        // 새로운 위치 = 현재 위치 + (방향 * 속력 * 시간)
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);


        if(transform.position.x < -2.3)
        {
            transform.position = new Vector2(-2.3f, transform.position.y);
        }
        if(transform.position.x > 2.3)
        {
            transform.position = new Vector2(2.3f, transform.position.y);
        }
        if(transform.position.y < -4.68)
        {
            transform.position = new Vector2(transform.position.x, -4.68f);
        }
        if(transform.position.y > 0)
        {
            transform.position = new Vector2(transform.position.x, 0);
        }
    }
}
