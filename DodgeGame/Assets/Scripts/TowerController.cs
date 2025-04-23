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


    // �����ϰ� ������Ʈ Ǯ ������
    private Stack<GameObject> bulletPool; // ������ �̿��� �ҷ� Ǯ.
    [SerializeField] private int poolSize; // �ҷ� Ǯ�� ũ��
    [SerializeField] private GameObject bulletPrefab; //�ҷ� ������
    
    
    void Start()
    {
        fireDelay = new WaitForSeconds(fireRate);
        bulletPool = new Stack<GameObject>();

        for(int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab); // ����
            obj.GetComponent<Bullet>().returnPool = bulletPool; // ���ư� Pool ����
            obj.SetActive(false); // ��Ȱ��ȭ�ؼ�
            bulletPool.Push(obj); // �־��ش�.
        }
    }

    void Update()
    {
        if (!GameManager.Instance.IsFinished) // ������ ������ �ʾ������� Ÿ�� Ȱ��ȭ.
            DetectPlayer();
    }

    private IEnumerator Fire()
    {
        while(true)
        {
            // �Ѿ� ����.
            GameObject bullet = bulletPool.Pop();
            // ���ư��� �� ���⼭�ص� �ǰ�, Bullet���� �ص� �ǰ�... Bullet���� ����.
            bullet.transform.position = muzzlePos.position;
            // �Ѿ� ������ �״�� �ֵ� �ǰ�.
            bullet.transform.forward = transform.forward;
            // �Ѿ� Ȱ��ȭ.
            bullet.SetActive(true);
            // �����ϰ�.
            yield return fireDelay;
        }
    }

    private void DetectPlayer()
    {
        if(Physics.OverlapSphere(transform.position, detectRadius, targetLayer).Length > 0) // ���������Ǿ �÷��̾� ���̾ ���� �ݶ��̴��� �ϳ��� ������
        {
            // �ѱ��� �÷��̾ �ٶ������.
            Vector3 lookPos = new Vector3(targetPos.position.x, transform.position.y, targetPos.position.z);
            transform.LookAt(lookPos);

            // �߻� -> �߻�ӵ��� ���缭 �ѹ��� �߻�Ǿ�� ��. �ڷ�ƾ ���.
            // �ڷ�ƾ�� �ѹ� ���۵Ǹ�, �׶����ʹ� ȣ����� �ʾƾ���.
            if (fireCoroutine == null)
             fireCoroutine = StartCoroutine(Fire());
        }
        else // �ƴϸ�
        {
            TowerRotate(); // Ÿ���� �ð�������� ����.
            // �߻� �ڷ�ƾ�� ���������.
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
