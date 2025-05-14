using System.Collections.Generic;
using _Scripts.Enums;
using Random = System.Random;


namespace _Scripts.FigureLogic
{
    
    /// <summary>
    /// Stores config template for future figure.
    /// </summary>
    public class FigureConfigTemplate
    {
        private static readonly Random rng = new Random();
        
        
        public ShapeTypeEnum Shape;
        public AnimalTypeEnum Animal;
        public FillColorEnum Color;
        public List<UniqueProperties> UniqProp = new ();

        public FigureConfigTemplate(ShapeTypeEnum shape, AnimalTypeEnum animal, FillColorEnum color)
        {
            Shape = shape;
            Animal = animal;
            Color = color;

            GenerateRandomUnicProperties();
        }

        public override int GetHashCode()
        {
            return (int)Shape * 100 + (int)Animal * 10 + (int)Color;
        }
        
        private void GenerateRandomUnicProperties()
        {
            int roll = rng.Next(0, 100);
            if (roll < 5)
            {
                UniqProp.Add(UniqueProperties.Frozen);
            }
            else
            {
                UniqProp.Add(UniqueProperties.Default);
            }
        }
    }
}