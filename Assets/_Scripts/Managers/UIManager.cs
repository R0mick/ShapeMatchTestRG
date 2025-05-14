using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [Header("Ui Parents")]
        [SerializeField] private GameObject startUIContentParent;
        [SerializeField] private GameObject gameOverUIContentParent;
        
        [Header("UI Elements")]
        [SerializeField] private TMP_Text gameResultText;
        
        private void OnEnable()
        {
            SimpleEventManager.Instance.OnEndGame += EndGame;
        }
        
        private void OnDisable()
        {
            SimpleEventManager.Instance.OnEndGame -= EndGame;
        }

        public void StartGame()
        {
            SimpleEventManager.Instance.StartGame();
            startUIContentParent.SetActive(false);
            
        }
        
        public void Reset()
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }

        private void EndGame(bool isWin)
        {
            Debug.Log("Game Over");
            gameOverUIContentParent.SetActive(true);
            gameResultText.text = isWin ? "You win!" : "You lose!";
        }
    }
}