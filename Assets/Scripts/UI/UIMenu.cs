using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private Text txtKeys;

    [SerializeField] private Button btnReplay;

    [SerializeField] private Animator AnimStartGame;

    [SerializeField] private Animator AnimGameOver;

    private Player m_player;

    private GameManager m_gameManager;

    public void SetupOnStart(GameManager mngr)
    {
        m_gameManager = mngr;
        m_player = mngr.Player;

        btnReplay.onClick.AddListener(OnClickReplay);
    }

    private void OnClickReplay()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        AnimStartGame.Play("start");
    }

    public void GameOver()
    {
        txtKeys.gameObject.SetActive(false);

        AnimGameOver.Play("start");
    }

    private void Update()
    {
        if (m_player != null)
        {
            txtKeys.text = string.Format("Keys: {0}", m_player.KeyCount);
        }
    }

    private void OnDestroy()
    {
        if (btnReplay) btnReplay.onClick.RemoveAllListeners();
    }
}
