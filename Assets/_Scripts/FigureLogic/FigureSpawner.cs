using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.FigureLogic
{
    /// <summary>
    /// Spawn figures on the field. Read config template and create figure from assets using it.
    /// </summary>
    public class FigureSpawner : MonoBehaviour
    {

        [Header("Figure prefabs")] [SerializeField]
        private List<GameObject> figurePrefabs;

        [SerializeField] private List<Sprite> animalSprites;

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform spawnParent;

        [SerializeField] private float spawnDelay = 0.1f;
        [SerializeField] private float dropSpeedMultiplayer = 5f;
        
        private Coroutine _spawnCoroutine;
        private FigureGenerator _figureGenerator;

        private void Awake()
        {
            _figureGenerator = new FigureGenerator();
        }

        private void OnEnable()
        {
            SimpleEventManager.Instance.OnSpawnFigureTypes += GenerateAndSpawnFigures;
        }

        private void OnDisable()
        {
            SimpleEventManager.Instance.OnSpawnFigureTypes += GenerateAndSpawnFigures;

            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
            }
        }

        public void GenerateAndSpawnFigures(int numberOfFigures)
        {
            var generatedFigureConfigs = _figureGenerator.GenerateFigures(numberOfFigures);
            _spawnCoroutine = StartCoroutine(SpawnCoroutine(generatedFigureConfigs));
        }


        public IEnumerator SpawnCoroutine(List<FigureConfigTemplate> figureConfigs)
        {
            foreach (var config in figureConfigs)
            {
                SpawnFigure(config);
                yield return new WaitForSeconds(spawnDelay);
            }
            SimpleEventManager.Instance.SpawnComplete(figureConfigs.Count);
        }

        private void SpawnFigure(FigureConfigTemplate configTemplate)
        {
            //get parts
            var targetShape = figurePrefabs.Find(f => f.name == configTemplate.Shape.ToString());
            var targetSprite = animalSprites.Find(sprite => sprite.name == configTemplate.Animal.ToString());
            var targetColor = GetColor(configTemplate.Color);
            
            //instantiate
            GameObject targetFigure = targetShape;
            float randomOffset = Random.Range(-0.2f, 0.2f);
            GameObject figureGo = Instantiate(targetFigure, spawnPoint.position+ new Vector3(randomOffset,0,0), Quaternion.identity, spawnParent);
            

            //set figure properties
            var figureComp = figureGo.GetComponent<FigureComponent>();
            figureComp.SetVisuals(targetSprite, targetColor);
            figureComp.SetHash(configTemplate.GetHashCode());
            figureComp.SetUniqueProperties(configTemplate.UniqProp);
            
            //set speed
            figureGo.GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * dropSpeedMultiplayer;

        }


        public static Color GetColor(FillColorEnum colorEnum)
        {
            switch (colorEnum)
            {
                case FillColorEnum.Red:
                    return new Color(0.98f, 0.6f, 0.5f);
                case FillColorEnum.Green:
                    return new Color(0.6f, 0.7f, 0.5f);
                case FillColorEnum.Blue:
                    return new Color(0.5f, 0.6f, 0.8f);
                default:
                    return Color.white;
            }
        }

    }
}
