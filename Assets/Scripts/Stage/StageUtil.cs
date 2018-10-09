using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageUtil : Singleton<StageUtil>
{

    const int LOGIN_STAGEID = 1;
    const int CREATEROLE_STAGEID = 2;
    const int SELECTROLE_STAGEID = 3;

    public void LoadLogin()
    {

    }

    public void LoadCreateRole()
    {

    }


    public void LoadSelectRole()
    {

    }

    public void Load<T>(int id) where T : Stage
    {

        var name = string.Empty;
        CoroutineUtil.Instance.Begin(Co_Load<T>(name));
    }


    IEnumerator Co_Load<T>(string sceneName)
    {
        SceneManager.LoadScene("Empty");

        var async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            yield return null;
        }
    }




}
