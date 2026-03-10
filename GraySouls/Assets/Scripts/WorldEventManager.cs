using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    public UIBossHealthBar bossHealthBar;
    EnemyBossManager boss;

    public List<FogWall> fogWalls;
    public bool bossFightIsActive;
    public bool bossHasBeenAwakened;
    public bool bossHasBeenDefeated;

    private void Awake()
    {
        bossHealthBar = FindAnyObjectByType<UIBossHealthBar>();
    }

    public void ActivateBossFight()
    {
        bossFightIsActive = true;
        bossHasBeenAwakened = true;
        bossHealthBar.SetUIHealthBarToActive();

        foreach(var fogWall in fogWalls)
        {
            fogWall.ActivateFogWall();
        }
    }

    public void BossHasBeenDefeated()
    {
        bossHasBeenDefeated = true;
        bossFightIsActive = false;
        //bossHasBeenAwakened = false;

        foreach (var fogWall in fogWalls)
        {
            fogWall.DeactivateFogWall();
        }
    }
}
