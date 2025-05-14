using System;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Interfaces;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.FigureLogic
{
    /// <summary>
    /// Main figure component with properties and behaviour.
    /// </summary>
    public class FigureComponent : MonoBehaviour, ITouchable
    {
        
        [Header("Links")]
        [SerializeField] private SpriteRenderer animalRenderer; 
        [SerializeField] private SpriteRenderer fillRenderer;

        [SerializeField] private int figureHash;
        
        [Header("Optional Behaviors")]
        [SerializeField] private bool isFrozen = false;
        [SerializeField] private int figuresRemoveLeftBeforeUnfreeze = 6;


        private void OnEnable()
        {
            SimpleEventManager.Instance.OnFiguresRemovedFromField += CheckFrozenStatus;
        }
        
        private void OnDisable()
        {
            SimpleEventManager.Instance.OnFiguresRemovedFromField -= CheckFrozenStatus;
        }

        public void SetVisuals(Sprite animalSprite, Color fillColor)
        {
            if (animalRenderer != null) animalRenderer.sprite = animalSprite;
            if (fillRenderer != null) fillRenderer.color = fillColor;
        }
        
        public void SetUniqueProperties(List<UniqueProperties> props)
        {
            if (props.Contains(UniqueProperties.Frozen))
            {
                isFrozen = true;
                animalRenderer.color = new Color(0.5f, 0.8f, 0.9f);
            }

            //add more properties
        }

        public void SetHash(int hash)
        {
            figureHash = hash;
        }
        public int GetHash()
        {
            return figureHash;
        }
        
        


        private void CheckFrozenStatus(int count)
        {
            figuresRemoveLeftBeforeUnfreeze-=count;
            if (figuresRemoveLeftBeforeUnfreeze <= 0)
            {
                isFrozen = false;
                animalRenderer.color = Color.white;
            }
        }
        
        
        public void OnTouch()
        {
            if (isFrozen)
            {
                return;
            }
            //Debug.Log("Click on object");
            PrepareFigureToMove();
            SimpleEventManager.Instance.FigureClicked(transform);
        }
        
        private void PrepareFigureToMove()
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            gameObject.transform.rotation = Quaternion.identity;
        }
    }
}
