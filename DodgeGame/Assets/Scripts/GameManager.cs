using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 게임이 끝났다는 것을 알려줘야함.
    public bool IsFinished;

    // public event Action OnFinished; // 해도되고
    // Player 체력이 0일때 이벤트 인보크해주면 됨.
    public UnityEvent OnFinishedEvent; // 해도된다.

    // 플레이어 비활성화를 위해.
    public GameObject Player;

    // 플레이어 스폰 위치
    [SerializeField] private Transform spawnPos;

    // UI Rect Transform.
    // Anchor : 상위 UI 기준으로 UI의 위치가 어디부터 시작될지 결정.
    // Pivot : 해당 UI의 크기, 회전등의 기준점이 어디가 될지 결정.

    [Header("UI")] 
    [SerializeField] private GameObject gameStartPanel;
    [SerializeField] private Button startBtn;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button retryBtn;

    private void Awake()
    {
        if(Instance == null) // Instance가 null이면
        {
            Instance = this; // 할당
            DontDestroyOnLoad(gameObject); // 씬전환간 삭제 X
        }
        else // null이 아니면 -> 이미 있다는 뜻.
        {
            Destroy(gameObject); // 하나만 존재해야하므로 삭제.
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
        OnFinishedEvent.AddListener(GameOver); // 구독했으면
        startBtn.onClick.AddListener(GameStart);
        retryBtn.onClick.AddListener(GameStart);
    }

    private void GameStart()
    {
        // 게임 시작할때 초기화 해줘야할 것.
        IsFinished = false;
        Player.transform.position = spawnPos.position; // 스폰위치 초기화
        Player.SetActive(true); // 활성화
        gameStartPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void GameOver()
    {
        // 게임오버됐을때 취할 동작.
        IsFinished = true;
        Player.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    private void OnDisable()
    {
        OnFinishedEvent.RemoveListener(GameOver); // 해제해줘야지
        startBtn.onClick.RemoveListener(GameStart);
        retryBtn.onClick.RemoveListener(GameStart);

    }
}
