using UnityEngine.SceneManagement;
public enum SceneName
{
    MainMenu,
    LoginScene,
    LobbyScene,
    GameScene,
}
public class SceneController
{
 

    // 로딩 씬 → 대상 씬 전환 (비동기 로딩)
    public void LoadScene(SceneName scene)
    {
        SceneManager.LoadScene(GetSceneName(scene));
    }

    string GetSceneName(SceneName scene)
    {
        string name = System.Enum.GetName(typeof(SceneName), scene);
        return name;
    }
}
