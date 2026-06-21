using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JRPG.Core
{
    // Mengelola perpindahan scene secara asinkron agar tidak terjadi frame freeze.
    public class SceneTransitionManager : MonoBehaviour
    {
        public static SceneTransitionManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public async Task LoadSceneAsync(string sceneName)
        {
            Debug.Log($"Memulai proses loading scene: {sceneName}...");

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                await Task.Yield();
            }

            await Task.Delay(500);

            operation.allowSceneActivation = true;
            Debug.Log($"Scene {sceneName} berhasil dimuat.");
        }
    }
}