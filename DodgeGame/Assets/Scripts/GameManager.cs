using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ������ �����ٴ� ���� �˷������.
    public bool IsFinished;

    // public event Action OnFinished; // �ص��ǰ�
    // Player ü���� 0�϶� �̺�Ʈ �κ�ũ���ָ� ��.
    public UnityEvent OnFinishedEvent; // �ص��ȴ�.

    // �÷��̾� ��Ȱ��ȭ�� ����.
    public GameObject Player;

    // �÷��̾� ���� ��ġ
    [SerializeField] private Transform spawnPos;

    // UI Rect Transform.
    // Anchor : ���� UI �������� UI�� ��ġ�� ������ ���۵��� ����.
    // Pivot : �ش� UI�� ũ��, ȸ������ �������� ��� ���� ����.

    [Header("UI")] 
    [SerializeField] private GameObject gameStartPanel;
    [SerializeField] private Button startBtn;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button retryBtn;

    private void Awake()
    {
        if(Instance == null) // Instance�� null�̸�
        {
            Instance = this; // �Ҵ�
            DontDestroyOnLoad(gameObject); // ����ȯ�� ���� X
        }
        else // null�� �ƴϸ� -> �̹� �ִٴ� ��.
        {
            Destroy(gameObject); // �ϳ��� �����ؾ��ϹǷ� ����.
        }
    }
    private void Init()
    {
        IsFinished = true;
        Player.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        Init();
        OnFinishedEvent.AddListener(GameOver); // ����������
        startBtn.onClick.AddListener(GameStart);
        retryBtn.onClick.AddListener(GameStart);
    }

    private void GameStart()
    {
        // ���� �����Ҷ� �ʱ�ȭ ������� ��.
        IsFinished = false;
        Player.transform.position = spawnPos.position; // ������ġ �ʱ�ȭ
        Player.SetActive(true); // Ȱ��ȭ
        gameStartPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void GameOver()
    {
        // ���ӿ��������� ���� ����.
        IsFinished = true;
        Player.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    private void OnDisable()
    {
        OnFinishedEvent.RemoveListener(GameOver); // �����������
        startBtn.onClick.RemoveListener(GameStart);
        retryBtn.onClick.RemoveListener(GameStart);

    }
}
