using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsGameStarted { get; private set; }

    public Player Player { get; private set; }

    private Creature[] m_creatureList;

    private UIMenu m_menu;
    private void Awake()
    {
        m_creatureList = FindObjectsOfType<Creature>();

        this.Player = m_creatureList.Where(x => x is Player).Cast<Player>().FirstOrDefault();

        m_menu = FindObjectOfType<UIMenu>();
    }

    void Start()
    {
        foreach (var creature in m_creatureList)
        {
            creature.SetupOnStart(this);
        }

        m_menu.SetupOnStart(this);

        m_menu.StartGame();

        IsGameStarted = true;
    }

    internal void GameOver()
    {
        IsGameStarted = false;

        m_menu.GameOver();
    }
}
