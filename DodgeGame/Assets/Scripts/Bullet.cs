using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float bulletSpeed;

    public Stack<GameObject> returnPool; // ���� �������� ����.

    void OnEnable()
    {
        // Ȱ��ȭ �ɶ����� ������ �����Բ�. OnEnable
        rigid.AddForce(transform.forward * bulletSpeed,ForceMode.Impulse);
        StartCoroutine(ReturnPool(3f));
    }

    private void Update()
    {
        // ������ �������� -> ���ӸŴ������� ���¸� �޾Ƽ�.
        if (GameManager.Instance.IsFinished)
            StartCoroutine(ReturnPool());
    }

    // �������� ����������ұ�?
    // �ð��� ��������.
    // �÷��̾�� �ε�������.
    // ������ ��������.
    private IEnumerator ReturnPool(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        // �ʱ�ȭ �������.
        // �ӵ� �ʱ�ȭ.
        rigid.velocity = Vector3.zero;
        // ��Ȱ��ȭ.
        gameObject.SetActive(false);
        // ���� Ǯ�� ����������.
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
