using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 플레이어는 WASD로 이동.
    // HP가 있고, HP가 0이되면 게임 종료(게임매니저에서)
    // 게임 종료 되면 비활성화.

    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float playerSpeed;
    [SerializeField] private int playerHealth; // 플레이어 헬스가 0일때 캐릭터 비활성화. -> 게임도 끝나겠지

    private Vector3 inputVec;

    private void OnEnable()
    {
        playerHealth = 5;
    }
    void Update()
    {
        // 입력을 받아
        PlayerInput();
    }
    private void FixedUpdate()
    {
        // FixedUpdate -> 신뢰할 수 있는 타이머를 기반으로 동작하므로 물리 처리에 적절. deltaTime을 곱해줄 필요가 없다.
        Move();
    }
    private void PlayerInput()
    {
        // InputManager 사용하여 입력 받기.
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        inputVec = new Vector3(x, 0, z).normalized; // 입력받은 벡터는 이동시 일정한 값으로 받기위해 정규화 nomalized;
    }
    private void Move()
    {
        // 트랜스폼 이동. -> 트랜스폼을 변경하므로, 충돌시에 문제가 생길 가능성이 있다.

        // 물리기반 이동 -> 물리기반으로 이동하는 것이 나을듯.
        // AddForce 같은 경우 직접 힘을 가해주니까.
        // velocity는 그냥 가해지는 힘을 직접 변경. -> 이걸로 가자.
        rigid.velocity = inputVec * playerSpeed;
    }

    private void TakeHit()
    {
        playerHealth -= 1;

        if(playerHealth == 0)
        {
            GameManager.Instance.OnFinishedEvent.Invoke();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeHit();
        }
    }
  
}
