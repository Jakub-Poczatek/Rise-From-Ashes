using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;
using NSubstitute;

public class PlayerStateTest
{
    UIController uiController;
    GameManager gameManager;
    GameObject placementManagerGameObject;

    [SetUp]
    public void Init()
    {
        GameObject gameManagerObject = new GameObject();
        var cameraMovementComponent = gameManagerObject.AddComponent<CameraMovement>();
        //gameManagerObject.AddComponent<ResourceManagerTestStub>();

        uiController = Substitute.For<UIController>();

        gameManager = gameManagerObject.AddComponent<GameManager>();
        //gameManager.resourceManagerGameObject = gameManagerObject;
        gameManager.cameraMovement = cameraMovementComponent;
        gameManager.uiController = uiController;

        placementManagerGameObject = new();
        gameManager.placementManagerGameObject = placementManagerGameObject;
    }

    [UnityTest]
    public IEnumerator PlayerSelectionStateTestWithEnumeratorPass()
    {
        yield return new WaitForEndOfFrame(); // awake
        yield return new WaitForEndOfFrame(); // start
        Assert.IsTrue(gameManager.PlayerState is PlayerSelectionState);
    }

    [UnityTest]
    public IEnumerator PlayerBuildingSingleStructureStateTestWithEnumeratorPass()
    {
        yield return new WaitForEndOfFrame(); // awake
        yield return new WaitForEndOfFrame(); // start
        gameManager.PlayerState.OnBuildSingleStructure(null);
        yield return new WaitForEndOfFrame();
        Assert.IsTrue(gameManager.PlayerState is PlayerBuildingResGenStructureState);
    }

    [UnityTest]
    public IEnumerator PlayerRemoveStateTestWithEnumeratorPass()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        gameManager.PlayerState.OnDemolish();
        yield return new WaitForEndOfFrame();
        Assert.IsTrue(gameManager.PlayerState is PlayerDemolishingState);
    }

    [UnityTest]
    public IEnumerator PlayerBuildingRoadStateTestWithEnumeratorPass()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        gameManager.PlayerState.OnBuildRoad(null);
        yield return new WaitForEndOfFrame();
        Assert.IsTrue(gameManager.PlayerState is PlayerBuildingRoadState);
    }
}

/*      [SetUp]
        public void Init()
        {
            GameObject gameManagerObject = new GameObject();
            var cameraMovementComponent = gameManagerObject.AddComponent<CameraMovement>();
            uiController = gameManagerObject.AddComponent<UIController>();

            GameObject buildObjectbtn = new GameObject();
            GameObject cancelObjectbtn = new GameObject();
            GameObject cancelPnl = new GameObject();
            GameObject closeBuildMenuBtn = new GameObject();

            uiController.cancelActionBtn = cancelObjectbtn.AddComponent<Button>();
            var buttonBuildComponent = buildObjectbtn.AddComponent<Button>();
            uiController.buildResidentialAreaBtn = buttonBuildComponent;
            uiController.cancelActionPnl = cancelObjectbtn;
            uiController.buildingMenuPnl = cancelPnl;
            uiController.openBuildMenuBtn = uiController.cancelActionBtn;
            uiController.demolishBtn = uiController.cancelActionBtn;
            closeBuildMenuBtn.AddComponent<Button>();
            uiController.closeBuildMenuBtn = closeBuildMenuBtn.GetComponent<Button>();

            gameManager = gameManagerObject.AddComponent<GameManager>();
            gameManager.cameraMovement = cameraMovementComponent;
            gameManager.uiController = uiController;

            GameObject goldAmountTxt = new GameObject();
            goldAmountTxt.AddComponent<TextMeshPro>();
            uiController.goldAmountTxt = goldAmountTxt.GetComponent<TMP_Text>();
        }*/