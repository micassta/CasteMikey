using UnityEngine;
using UnityEngine.SceneManagement;

public class Reiniciarnivel : MonoBehaviour
{
    public void ReiniciarNivel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
