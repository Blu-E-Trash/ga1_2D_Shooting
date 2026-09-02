using UnityEngine;
using UnityEngine.Animations;

public class PlayerMove : MonoBehaviour
{
    // 목적: 키보드 입력에 따른 플레이어 이동 처리
    


    private void Update()
    {
        // 1, 키보드 입력을 받는다.
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("왼쪽 방향키를 누르고 있습니다.");
        }
        // 2. 키보드 입력에 따라 방향을 구한다.
        // 3. 방향과 속도에 따라 이동한다.
    }
}
