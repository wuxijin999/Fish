using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : Singleton<SceneLoad>
{

    const int LOGIN_SCENEID = 1;
    const int CREATEROLE_SCENEID = 2;
    const int SELECTROLE_SCENEID = 3;

    Scene m_CurrentScene = null;
    public int currentSceneId { get; private set; }
    public bool isLoading { get; private set; }

    float m_Progress = 0f;
    public float progress {
        get { return m_Progress; }
        private set {
            m_Progress = value;
            progressChangeEvent.Invoke(m_Progress);
        }
    }

    public BizEvent<float> progressChangeEvent = new BizEvent<float>();

    public void LoadLogin()
    {
        CoroutineUtil.Instance.Begin(Co_Load<LoginScene>(LOGIN_SCENEID));
    }

    public void LoadCreateRole()
    {
        CoroutineUtil.Instance.Begin(Co_Load<CreateRoleScene>(CREATEROLE_SCENEID));
    }

    public void LoadSelectRole()
    {
        CoroutineUtil.Instance.Begin(Co_Load<SelectRoleScene>(SELECTROLE_SCENEID));
    }

    public void Load<T>(int id) where T : Scene
    {
        CoroutineUtil.Instance.Begin(Co_Load<T>(id));
    }

    IEnumerator Co_Load<T>(int sceneId) where T : Scene
    {
        isLoading = true;
        progress = 0f;
        PreLoad(currentSceneId, sceneId);
        currentSceneId = sceneId;

        progress = 0.1f;
        SceneManager.LoadScene("Empty");
        progress = 0.2f;

        var assetName = GetSceneName(currentSceneId);
        var async = SceneManager.LoadSceneAsync(assetName);
        while (!async.isDone)
        {
            progress = 0.2f + async.progress * 0.5f;
            yield return null;
        }

        m_CurrentScene = new GameObject("Scene").AddComponent<T>();
        progress = 0.8f;
        AfterLoad();

        yield return null;
        progress = 1f;
        isLoading = false;
    }

    private void PreLoad(int lastSceneId, int currentSceneId)
    {
        try
        {
            if (m_CurrentScene != null)
            {
                m_CurrentScene.OnUnInitialize();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }

        Windows.Instance.CloseAll();
        switch (currentSceneId)
        {
            case LOGIN_SCENEID:
                LoadingPresenter.Instance.OpenWindow();
                break;
            case CREATEROLE_SCENEID:
                LoadingPresenter.Instance.OpenWindow();
                break;
            case SELECTROLE_SCENEID:
                LoadingPresenter.Instance.OpenWindow();
                break;
            default:
                LoadingPresenter.Instance.OpenWindow();
                break;
        }
    }

    private void AfterLoad()
    {
        try
        {
            if (m_CurrentScene != null)
            {
                m_CurrentScene.OnInitialize();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }

        LoadingPresenter.Instance.CloseWindow();
    }


    private string GetSceneName(int id)
    {
        switch (id)
        {
            case LOGIN_SCENEID:
                return "Login";
            case CREATEROLE_SCENEID:
                return "CreateRole";
            case SELECTROLE_SCENEID:
                return "SelectRole";
            default:
                return string.Empty;
        }
    }

}
