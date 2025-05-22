using UnityEngine;

public class CloseGame : MonoBehaviour
{

    public void EndGame()
    {
        Application.Quit();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
