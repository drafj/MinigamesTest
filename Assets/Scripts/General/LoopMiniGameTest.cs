using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneralsMiniGames;
using Economy;

public class LoopMiniGameTest : LoopMiniGame
{
    protected override IEnumerator CouLoopMiniGame()
    {
        while (true)
        {
            if (PointsManager.instance.points >= PointsManager.instance.goalPoints)
            {
                PanelCronometro.Instance.StopCronometro();
                EconomyManager.Intance.GiveCoinsQuantity(2);
                SelectChallenge.Instance.SetupNextLevelChallenge();
                MiniGameFinished();
                break;
            }
            yield return null;
        }
    }
}
