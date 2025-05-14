using System;
using UnityEngine;

namespace _Scripts.Managers
{
    /// <summary>
    /// Used as simple event bus.
    /// Configured in Project Settings -> Script Execution Order -> before "Default Time".
    /// </summary>
    public class SimpleEventManager:MonoBehaviour
    {
        public static SimpleEventManager Instance;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

            }
        }
        
        
        //events
        public event Action<int> OnSpawnFigureTypes;
        public void SpawnFigureTypes(int count)
        {
            OnSpawnFigureTypes?.Invoke(count);
        }
        
        public event Action<int> OnSpawnComplete;
        public void SpawnComplete(int totalSpawned)
        {
            //Debug.Log("SpawnComplete");
            OnSpawnComplete?.Invoke(totalSpawned);
        }
        
        public event Action<Transform> OnFigureClicked;

        public void FigureClicked(Transform figureTransform)
        {
            OnFigureClicked?.Invoke(figureTransform);
        }
        
        public event Action<bool> OnUpdateTouchActive;
        public void UpdateTouchActive(bool isTouchActive)
        {
            OnUpdateTouchActive?.Invoke(isTouchActive);
        }
        
        public event Action<int> OnFiguresRemovedFromField;

        public void FiguresRemovedFromField(int figuresCount)
        {
            OnFiguresRemovedFromField?.Invoke(figuresCount);
        }
        
        public event Action<bool> OnEmptyField;
        public void EmptyField(bool fieldIsEmpty)
        {
            OnEmptyField?.Invoke(fieldIsEmpty);
        }
        
        public event Action OnStartGame;
        public void StartGame()
        {
            OnStartGame?.Invoke();
        }
        
        public event Action<bool> OnEndGame;
        public void EndGame(bool isWin)
        {
            OnEndGame?.Invoke(isWin);
        }
        
        public event Action OnReloadScene;
        public void ReloadScene()
        {
            OnReloadScene?.Invoke();
        }
    }
}