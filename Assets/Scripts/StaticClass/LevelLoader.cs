using UnityEngine.SceneManagement;


public static class LevelLoader
{
    public static void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}
