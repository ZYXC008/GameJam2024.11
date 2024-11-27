using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISaveable
{
    public Transform playerTrans;
    public Vector3 menuPositon;
    public Vector3 firstPosition;
    [Header("�¼�����")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;
    [Header("�㲥")]
    public VoidEventSO afterSceneLoaderEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO unLoadedEvent;
    [SerializeField] public GameSceneSO currentLoadedScene;

    [Header("����")]
    public GameSceneSO menuScene;
    public GameSceneSO firstLoadScene;

    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private bool isLoading;
    public float fadeDuration;

    private void Start()
    {
        //��ɱ���������ע�ͣ���ע��NewGame()
        //loadEventSO.RaiseLoadRequestEvent(menuScene, menuPositon, true);
        NewGame();
    }
    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRiased += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRiased -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
            return;
        isLoading = true;
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;
        if (currentLoadedScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }
    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
            fadeEvent.FadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);

        unLoadedEvent.RaiseLoadRequestEvent(sceneToLoad,positionToGo, true);

        yield return currentLoadedScene.sceneRenference.UnLoadScene();

        playerTrans.gameObject.SetActive(false);
        LoadNewScene();
    }
    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneRenference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadedScene = sceneToLoad;
        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;
        if(currentLoadedScene.sceneType != SceneType.Menu)
        {
            afterSceneLoaderEvent.RaiseEvent();
        }
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadedScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefination>().ID;
        if (data.characterPosDict.ContainsKey(playerID)){
            positionToGo = data.characterPosDict[playerID];
            sceneToLoad = data.GetSavedScene();

            OnLoadRequestEvent(sceneToLoad, positionToGo, true);
        }
    }
}
