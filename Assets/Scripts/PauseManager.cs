using UnityEngine;
    public class PauseManager : MonoBehaviour
    {
        public GameObject pauseMenuUI;
        private bool isPaused = false;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                    Reanudar();
                else
                    Pausar();
            }
        }

        public void Pausar()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }

        public void Reanudar()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

