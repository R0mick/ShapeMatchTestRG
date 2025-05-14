using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Managers
{
    public class GameManager:MonoBehaviour
    {
        [SerializeField] private int totalFiguresOnField;
        [SerializeField] private GameObject figuresContainer;
        
        [SerializeField] private bool ifFieldFilled = false;
        private void OnEnable()
        {
            SimpleEventManager.Instance.OnSpawnComplete += EnableInput;
            SimpleEventManager.Instance.OnFiguresRemovedFromField += SubtractFigures;
            SimpleEventManager.Instance.OnStartGame += StartGame;
        }

        
        private void OnDisable()
        {
            SimpleEventManager.Instance.OnSpawnComplete -= EnableInput;
            SimpleEventManager.Instance.OnFiguresRemovedFromField -= SubtractFigures;
            SimpleEventManager.Instance.OnStartGame -= StartGame;
        }
        
        private void StartGame()
        {
            //spawn figures
            SimpleEventManager.Instance.SpawnFigureTypes(10); //full field 20
        }
        
        public void RefreshField()
        {
            //Debug.Log("check refresh");
            //waiting before field fills
            if (!ifFieldFilled)
            {
                //Debug.Log("Waiting for field filled");
                return;
            }
            
            var figuresBeforeClean = totalFiguresOnField;
            if (totalFiguresOnField > 0)
            {
                CleanField();
                SimpleEventManager.Instance.SpawnFigureTypes(figuresBeforeClean/3);
            }

            if (ifFieldFilled && totalFiguresOnField == 0)
            {
                SimpleEventManager.Instance.EndGame(true);
            }
        }

        private void EnableInput(int totalSpawned)
        {
            totalFiguresOnField = totalSpawned;
            ifFieldFilled = true;
            SimpleEventManager.Instance.UpdateTouchActive(true);
        }

        private void SubtractFigures(int count)
        {
            totalFiguresOnField-=count;

            if (totalFiguresOnField == 0)
            {
                SimpleEventManager.Instance.EndGame(true);
                SimpleEventManager.Instance.EmptyField(true);
            }
        }

        private void CleanField()
        {
            ifFieldFilled = false;
            foreach (Transform child in figuresContainer.transform)
            {
                Destroy(child.gameObject);
            }

            totalFiguresOnField = 0;
        }
    }
}