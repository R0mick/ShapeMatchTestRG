using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.FigureLogic;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Managers
{

    public class ActionBarManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> slots;
        private readonly List<GameObject> _figuresInSlots = new List<GameObject>();
        private bool _isFieldEmpty = false;
        

        private void Start()
        {
            SimpleEventManager.Instance.OnFigureClicked += AddFigure;
            SimpleEventManager.Instance.OnEmptyField += SetEmptyField;
        }

        private void OnDisable()
        {
            SimpleEventManager.Instance.OnFigureClicked -= AddFigure;
            SimpleEventManager.Instance.OnEmptyField -= SetEmptyField;
        }

        public void AddFigure(Transform figureTransform)
        {

            
            //disable tap
            SimpleEventManager.Instance.UpdateTouchActive(false);
            
            
            //add figure
            Transform targetSlot = slots[GetEmptySlotIndex()];
            _figuresInSlots.Add(figureTransform.gameObject);
            
            MoveToSlot(targetSlot,figureTransform, ()=>
            {
                SimpleEventManager.Instance.FiguresRemovedFromField(1);
            
                if (_isFieldEmpty)
                {
                    return;
                }
                
                
                CheckMatch();

                if (_figuresInSlots.Count == 7)
                {
                    SimpleEventManager.Instance.EndGame(false);
                }
                else
                {
                    //enable tap
                    SimpleEventManager.Instance.UpdateTouchActive(true);
                }
            });
        }

        private void CheckMatch()
        {
            if (_figuresInSlots.Count < 3)
            {
                return;
            }
            
            var figureComponents = _figuresInSlots
                .Where(go => go != null)
                .Select(go => go.GetComponent<FigureComponent>())
                .Where(script => script != null)
                .ToList();
            
            var groupsWithTriples = figureComponents
                .GroupBy(script => script.GetHash())
                .Where(group => group.Count() >= 3)
                .ToList();
            
            foreach (var group in groupsWithTriples)
            {
                var objectsToRemove = group.Take(3).ToList();
                foreach (var script in objectsToRemove)
                {
                    if (script != null)
                    {
                        _figuresInSlots.Remove(script.gameObject);
                        Destroy(script.gameObject);
                    }
                }
            }
        }

        private int GetEmptySlotIndex()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].childCount == 0)
                {
                    return i;
                }
            }
            
            return -1;
        }

        private void MoveToSlot(Transform targetSlot, Transform figure, Action onComplete)
        {
            var tween = figure.DOMove(targetSlot.position, 0.5f)
                .SetEase(Ease.OutQuart)
                .OnComplete(() =>
                {
                    figure.SetParent(targetSlot, true);
                    onComplete?.Invoke();
                });
            tween.Play();
        }

        private void SetEmptyField(bool isEmpty)
        {
            _isFieldEmpty = isEmpty;
        }
    }
}