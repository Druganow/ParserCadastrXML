using System;
using System.Xml.Linq;

namespace CadastrPlanApp
{
    /// <summary>
    /// Координата.
    /// </summary>
    public class Ordinate
    {
        /// <summary>
        /// X.
        /// </summary>
        private readonly string x;

        /// <summary>
        /// Y.
        /// </summary>
        private readonly string y;

        /// <summary>
        /// Номер.
        /// </summary>
        private readonly string ord_nmb;

        /// <summary>
        /// Номер точки пространства.
        /// </summary>
        private readonly string num_geopoint;

        /// <summary>
        /// Код определения координат.
        /// </summary>
        private readonly string geopoint_opredCode;

        /// <summary>
        /// Определение координаты.
        /// </summary>
        private readonly string geopoint_opredValue;

        /// <summary>
        /// Дельта.
        /// </summary>
        private readonly string delta_geopoint;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="element">XML-узел с информацией о координате.</param>
        public Ordinate(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentException("Пустой аргуент");
            }

            x = element.Element("x").Value;
            y = element.Element("y").Value;
            ord_nmb = element.Element("ord_nmb")?.Value;
            num_geopoint = element.Element("num_geopoint")?.Value;
            
            var geopoint_opred = element.Element("geopoint_opred");
            geopoint_opredCode = geopoint_opred?.Element("code").Value;
            geopoint_opredValue = geopoint_opred?.Element("value").Value;
            delta_geopoint = element.Element("delta_geopoint")?.Value;
        }

        /// <summary>
        /// Формирует информацию о координате.
        /// </summary>
        /// <returns>Информация о координате.</returns>
        public override string ToString()
        {
            string str = "";
            str += "\nКоордината: (" + x + " ; " + y + ")";

            if (num_geopoint != null) str += "\nНомер : " + num_geopoint;
            if (ord_nmb != null) str += "\nНомер точки пространства: " + ord_nmb;
            if (geopoint_opredCode != null) str += "\nКод определения координат: " + geopoint_opredCode;
            if (geopoint_opredValue != null) str += "\nОпределение координаты: " + geopoint_opredValue;
            if (delta_geopoint != null) str += "\nДельта: " + delta_geopoint;

            return str;
        }
    }
}
