using System.Collections.Generic;
using System.Xml.Linq;

namespace CadastrPlanApp
{
    /// <summary>
    /// Земельный участок.
    /// </summary>
    public class Land : RecordData
    {
        /// <summary>
        /// Код подтипа участка.
        /// </summary>
        private readonly string subtype_code;

        /// <summary>
        /// Подтип участка.
        /// </summary>
        private readonly string subtype_value;

        /// <summary>
        /// Код категории.
        /// </summary>
        private readonly string category_code;

        /// <summary>
        /// Категория.
        /// </summary>
        private readonly string category_value;

        /// <summary>
        /// Использование по документам.
        /// </summary>
        private readonly string useByDocument;

        /// <summary>
        /// Код фактического использования.
        /// </summary>
        private readonly string landUseCode;

        /// <summary>
        /// Фактическое использование.
        /// </summary>
        private readonly string landUseValue;

        /// <summary>
        /// Площадь.
        /// </summary>
        private readonly string area;

        /// <summary>
        /// Цена.
        /// </summary>
        private readonly string cost;

        /// <summary>
        /// Идентификатор системы координат.
        /// </summary>
        private readonly string skId;

        /// <summary>
        /// Список координат.
        /// </summary>
        private readonly List<Ordinate> ordinates = new List<Ordinate>();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="element">XML-узел с информацией о земельном участке.</param>
        public Land(XElement element) : base(element)
        {
            var subtype = element.Element("object").Element("subtype");
            var costElement = element.Element("cost");

            if (element.Element("params").Element("category") != null)
            {
                category_code = element.Element("params").Element("category").Element("type").Element("code").Value;
                category_value = element.Element("params").Element("category").Element("type").Element("value").Value;
            }

            if (element.Element("params").Element("permitted_use") != null)
            {
                var premUseSection = element.Element("params").Element("permitted_use").Element("permitted_use_established");
                useByDocument = premUseSection.Element("by_document").Value;

                if (premUseSection.Element("land_use") != null)
                {
                    landUseCode = premUseSection.Element("land_use").Element("code") != null ?
                        premUseSection.Element("land_use").Element("code").Value : "";
                    landUseValue = premUseSection.Element("land_use").Element("value") != null ?
                        premUseSection.Element("land_use").Element("value").Value : "";
                }
            }

            if (element.Element("area") != null)
            {
                area = element.Element("area").Element("value").Value;
            }

            if (subtype != null)
            {
                subtype_code = subtype.Element("code").Value;
                subtype_value = subtype.Element("value").Value;
            }

            if (costElement != null)
            {
                cost = costElement.Value;
            }

            if (element.Element("contours_location") != null)
            {
                var entity_spatial = element.Element("contours_location")
                                        .Element("contours")
                                        .Element("contour")
                                        .Element("entity_spatial");
                skId = entity_spatial.Element("sk_id").Value;

                foreach (var coord in entity_spatial.Element("spatials_elements")
                                                    .Element("spatial_element")
                                                    .Element("ordinates")
                                                    .Elements("ordinate"))
                {
                    ordinates.Add(new Ordinate(coord));
                }
            }

        }

        /// <summary>
        /// Тип записи.
        /// </summary>
        public override string Type
        {
            get
            {
                return "Land";
            }
        }

        /// <summary>
        /// Формирует строку о подтипе.
        /// </summary>
        /// <returns>Строка о подтипе.</returns>
        public string GetSubtypeString()
        {
            return $"Код подтипа: {subtype_code}\n" +
                $"Подтип: {subtype_value}\n";
        }

        /// <summary>
        /// Формирует строку о категории.
        /// </summary>
        /// <returns>Строка о категории.</returns>
        public string GetCategoryString()
        {
            return $"Код категории: {category_code}\n" +
                $"Категория: {category_value}\n";
        }

        /// <summary>
        /// Формирует строку о типе пользования.
        /// </summary>
        /// <returns>Строка о типе пользования.</returns>
        public string GetUseString()
        {
            return $"Разрешенный вид использования: {useByDocument}\n" +
                $"Код пользования: {landUseCode}\n" +
                $"Используется: {landUseValue}\n";
        }

        /// <summary>
        /// Формирует строку о земельном участке.
        /// </summary>
        /// <returns>Строка о земельном участке.</returns>
        public override string ToString()
        {
            var str = $"{base.GetObjectString()}\n{GetSubtypeString()}" +
                $"{GetCategoryString()}{GetUseString()}{base.GetAddressString()}\n";

            if (!string.IsNullOrEmpty(area))
                str += $"Площадь: {area}\n";

            if (!string.IsNullOrEmpty(area))
                str += $"Цена: {cost}\n";

            if (!string.IsNullOrEmpty(skId))
            {
                str += $"Идентификатор системы координат: {skId}\n";

                foreach (var coord in ordinates)
                {
                    str += coord.ToString() + "\n";
                }
            }

            return str;
        }
    }
}
