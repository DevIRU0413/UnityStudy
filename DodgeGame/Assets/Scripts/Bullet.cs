using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float bulletSpeed;

    public Stack<GameObject> returnPool; // 아직 정해지지 않음.

    void OnEnable()
    {
        // 활성화 될때마다 앞으로 나가게끔. OnEnable
        rigid.AddForce(transform.forward * bulletSpeed,ForceMode.Impulse);
        StartCoroutine(ReturnPool(3f));
    }

    private void Update()
    {
        // 게임이 끝났을때 -> 게임매니저에서 상태를 받아서.
        if (GameManager.Instance.IsFinished)
            StartCoroutine(ReturnPool());
    }

    // 언제언제 리턴해줘야할까?
    // 시간이 지났을때.
    // 플레이어와 부딪혔을때.
    // 게임이 끝났을때.
    private IEnumerator ReturnPool(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        // 초기화 해줘야함.
        // 속도 초기화.
        rigid.velocity = Vector3.zero;
        // 비활성화.
        gameObject.SetActive(false);
        // 기존 풀로 돌려보내기.
        returnPool.Push(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 3)
        {
            StartCoroutine(ReturnPool());
        }
    }

}
