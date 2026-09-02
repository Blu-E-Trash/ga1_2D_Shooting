using UnityEngine;
using UnityEngine.Animations;

public class PlayerMove : MonoBehaviour
{
    public float Speed = 5f;
    private float minPosX = -2.3f;
    private float maxPosX = 2.3f;
    private float minPosY = -4.68f;
    private float maxPosY = 0f;

    private void Update()
    {
        Move();
        SpeedChange();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal"); // 좌우 -1 ~ 1 사이의 값 반환
        float v = Input.GetAxisRaw("Vertical");   // 상하 -1 ~ 1 사이의 값 반환

        Vector2 direction = new Vector2(h, v);

        Vector2 normalizedSpeed = direction.normalized * Speed;

        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);


        if (transform.position.x < minPosX)
        {
            transform.position = new Vector2(maxPosX, transform.position.y);
        }
        if (transform.position.x > maxPosX)
        {
            transform.position = new Vector2(minPosX, transform.position.y);
        }
        if (transform.position.y < minPosY)
        {
            transform.position = new Vector2(transform.position.x, minPosY);
        }
        if (transform.position.y > maxPosY)
        {
            transform.position = new Vector2(transform.position.x, maxPosY);
        }
    }
    private void SpeedChange()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Speed += 1f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            if (Speed > 0)
            {
                Speed -= 1f * Time.deltaTime;
            }
        }
    }
}
