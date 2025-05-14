using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Enums;

namespace _Scripts.FigureLogic
{
    /// <summary>
    /// Generates random properties and stores it into FigureConfigTemplate
    /// </summary>
    public class FigureGenerator
    {
        private const int IdenticalCount = 3;

        public List<FigureConfigTemplate> GenerateFigures(int numberOfFigures)
        {
            List<FigureConfigTemplate> figures = new List<FigureConfigTemplate>();

            for (int i = 0; i < numberOfFigures; i++)
            {
                var figureConfig = new FigureConfigTemplate(GetRandomEnum<ShapeTypeEnum>(), GetRandomEnum<AnimalTypeEnum>(),GetRandomEnum<FillColorEnum>());
                for (int j = 0; j < IdenticalCount; j++)
                {
                    figures.Add(figureConfig);
                }
            }
            return figures.OrderBy(x => UnityEngine.Random.value).ToList();
        }
        
        
        private T GetRandomEnum<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }
        
    }
}