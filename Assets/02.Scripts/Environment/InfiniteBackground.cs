using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private float _bottomY = -10f;
    [SerializeField]
    private float _topY = 10f;

    private void Update()
    {
        MoveDown();
    }

    private void MoveDown()
    {
        transform.position += new Vector3(0, -_speed * Time.deltaTime, 0);

        if (transform.position.y <= _bottomY)
        {
            transform.position = new Vector3(transform.position.x, _topY, transform.position.z);
        }
    }
}