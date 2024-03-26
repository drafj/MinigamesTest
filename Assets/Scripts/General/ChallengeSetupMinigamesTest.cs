using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneralsMiniGames;

public class ChallengeSetupMinigamesTest : ChallengeSetup
{
    public int chickenMinGoal, chickenMaxGoal;
    public int simultaneousChicken;
    public int distractorAnimals;
    public int animalsDisplayTime;
    public int diferentAnimalsDisplayed;
    public int limitTime;
    public bool diferentAnimals;

    public override void SetupAndStartChallengeInScene()
    {
        base.SetupAndStartChallengeInScene();
        AttemptsCounter.Instance.ResetAttemps();
        AnimalSpawner.Instance.StartChallenge(chickenMinGoal, chickenMaxGoal, simultaneousChicken, distractorAnimals, animalsDisplayTime, limitTime, diferentAnimalsDisplayed, diferentAnimals);
        Debug.Log("se ejecuta, actual challenge: " + currentChallengeLevel);
    }

    public override bool GetIfCurrentChallengeLevelAvailable()
    {
        return currentChallengeLevel < quantityChallengesInLevel;
    }

    public override void PassToNextChallengeLevelAvaible()
    {
        NormalReset();
        currentChallengeLevel++;
    }

    public override void ResetChallenge()
    {
        AnimalSpawner.Instance.ResetAnimalSpawner();
        PointsManager.instance.ResetPoints();
        PanelCronometro.Instance.StopCronometro();
        PanelFinishedAttempts.Instance.OpenPanel();
    }

    public void NormalReset()
    {
        AnimalSpawner.Instance.ResetAnimalSpawner();
        PointsManager.instance.ResetPoints();
    }
}