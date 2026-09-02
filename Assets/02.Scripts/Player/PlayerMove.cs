using UnityEngine;
using UnityEngine.Animations;

public class PlayerMove : MonoBehaviour
{
    // 목적: 키보드 입력에 따른 플레이어 이동 처리

    public float speed = 5f; // 이동 속력

    // 왼쪽 방향을 의미하는 벡터
    Vector2 direction = new Vector2(-1, 0); // = Vector2 direction = Vector2.left);

    private void Update()
    {
        // 1, 키보드 입력을 받는다.
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("왼쪽 방향키를 누르고 있습니다.");
            // 속도 = 방향 * 속력          //매직넘버 : 보는 사람에 따라 의미가 달라질 수 있는 헷갈리는 숫자    
            transform.Translate(direction * speed);
        }
        // 2. 키보드 입력에 따라 방향을 구한다.
        // 게임에는 벡터라는 타입이 있음. 벡터는 크기와 방향을 의미함.

                                               

        // 3. 방향과 속도에 따라 이동한다.
        
    }
}
