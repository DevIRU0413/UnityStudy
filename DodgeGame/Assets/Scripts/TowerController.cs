using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private float rotSpeed;
    [SerializeField] private float detectRadius;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform targetPos;
    [SerializeField] private Transform muzzlePos;
    [SerializeField] private float fireRate;

    private Coroutine fireCoroutine;
    private YieldInstruction fireDelay;


    // 간단하게 오브젝트 풀 만들어보기
    private Stack<GameObject> bulletPool; // 스택을 이용한 불렛 풀.
    [SerializeField] private int poolSize; // 불렛 풀의 크기
    [SerializeField] private GameObject bulletPrefab; //불렛 프리팹
    
    
    void Start()
    {
        fireDelay = new WaitForSeconds(fireRate);
        bulletPool = new Stack<GameObject>();

        for(int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab); // 생성
            obj.GetComponent<Bullet>().returnPool = bulletPool; // 돌아갈 Pool 설정
            obj.SetActive(false); // 비활성화해서
            bulletPool.Push(obj); // 넣어준다.
        }
    }

    void Update()
    {
        if (!GameManager.Instance.IsFinished) // 게임이 끝나지 않았을때만 타워 활성화.
            DetectPlayer();
    }

    private IEnumerator Fire()
    {
        while(true)
        {
            // 총알 생성.
            GameObject bullet = bulletPool.Pop();
            // 날아가는 건 여기서해도 되고, Bullet에서 해도 되고... Bullet에서 하자.
            bullet.transform.position = muzzlePos.position;
            // 총알 방향은 그대로 둬도 되고.
            bullet.transform.forward = transform.forward;
            // 총알 활성화.
            bullet.SetActive(true);
            // 삭제하고.
            yield return fireDelay;
        }
    }

    private void DetectPlayer()
    {
        if(Physics.OverlapSphere(transform.position, detectRadius, targetLayer).Length > 0) // 오버랩스피어에 플레이어 레이어를 가진 콜라이더가 하나라도 있으면
        {
            // 총구가 플레이어를 바라봐야함.
            Vector3 lookPos = new Vector3(targetPos.position.x, transform.position.y, targetPos.position.z);
            transform.LookAt(lookPos);

            // 발사 -> 발사속도에 맞춰서 한번씩 발사되어야 함. 코루틴 사용.
            // 코루틴이 한번 시작되면, 그때부터는 호출되지 않아야함.
            if (fireCoroutine == null)
             fireCoroutine = StartCoroutine(Fire());
        }
        else // 아니면
        {
            TowerRotate(); // 타워가 시계방향으로 돌기.
            // 발사 코루틴을 멈춰줘야함.
            if(fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
        }

    }

    private void TowerRotate()
    {
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
