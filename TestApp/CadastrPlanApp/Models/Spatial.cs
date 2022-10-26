using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CadastrPlanApp
{
    /// <summary>
    /// Пространственный объект.
    /// </summary>
    public class Spatial : CadastrObject
    {
        /// <summary>
        /// Идентификатор системы координат.
        /// </summary>
        private readonly string skId;

        /// <summary>
        /// Список координат.
        /// </summary>
        private readonly List<Ordinate> ordinates;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="element">XML-узел с информацией о пространственном объекте.</param>
        public Spatial(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentException("Пустой аргумент.");
            }

            ordinates = new List<Ordinate>();
            skId = element.Element("sk_id").Value;

            foreach (var coord in element.Element("spatials_elements")
                                        .Element("spatial_element")
                                        .Element("ordinates")
                                        .Elements("ordinate"))
            {
                ordinates.Add(new Ordinate(coord));
            }
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public override string Id
        {
            get
            {
                return skId;
            }
        }

        /// <summary>
        /// Тип записи.
        /// </summary>
        public override string Type
        {
            get
            {
                return "Spatial";
            }
        }

        /// <summary>
        /// Формрует информацию о пространственном объекте.
        /// </summary>
        /// <returns>Информация о пространственном объекте.</returns>
        public override string ToString()
        {
            var str = "";

            foreach (var coords in ordinates)
            {
                str += coords.ToString() + "\n";
            }

            return $"Пространственные данные\n\n" +
                $"Идентификатор {skId}\n{str}";
        }
    }
}
