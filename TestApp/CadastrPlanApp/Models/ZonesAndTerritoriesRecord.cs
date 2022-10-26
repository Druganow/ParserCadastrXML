using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CadastrPlanApp
{
    /// <summary>
    /// Зона и территория.
    /// </summary>
    class ZonesAndTerritoriesRecord : CadastrObject
    {
        /// <summary>
        /// Дата регистрации.
        /// </summary>
        private readonly string registration_date;

        /// <summary>
        /// Регистрационный номер.
        /// </summary>
        private readonly string reg_numb_border;

        /// <summary>
        /// Код типа границы.
        /// </summary>
        private readonly string typeBoundaryCode;

        /// <summary>
        /// Тип границы.
        /// </summary>
        private readonly string typeBoundaryValue;

        /// <summary>
        /// Код типа зоны.
        /// </summary>
        private readonly string typeZoneCode;

        /// <summary>
        /// Тип зоны.
        /// </summary>
        private readonly string typeZoneValue;

        /// <summary>
        /// Номер.
        /// </summary>
        private readonly string number;

        /// <summary>
        /// Список координат.
        /// </summary>
        private readonly List<List<Ordinate>> ordinates = new List<List<Ordinate>>();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="element">XML-узел с информацией о зоне и территории.</param>
        public ZonesAndTerritoriesRecord(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentException("Пустой аргумент.");
            }

            registration_date = element.Element("record_info").Element("registration_date").Value;
            var b_object = element.Element("b_object_zones_and_territories").Element("b_object");
            var type_zone = element.Element("b_object_zones_and_territories").Element("type_zone");
            var numberElement = element.Element("b_object_zones_and_territories").Element("number");
            var entity_spatial = element.Element("b_contours_location")
                                        .Element("contours")
                                        .Element("contour")
                                        .Element("entity_spatial");

            if (b_object != null)
            {
                reg_numb_border = b_object.Element("reg_numb_border").Value;
                typeBoundaryCode = b_object.Element("type_boundary").Element("code").Value;
                typeBoundaryValue = b_object.Element("type_boundary").Element("code").Value;
            }

            if (type_zone != null)
            {
                typeZoneCode = type_zone.Element("code").Value;
                typeZoneValue = type_zone.Element("value").Value;
            }

            if (numberElement != null)
            {
                number = numberElement.Value;
            }

            if (entity_spatial != null)
            {
                foreach (var spatials in entity_spatial.Elements("spatials_elements"))
                {
                    var list = new List<Ordinate>();

                    foreach (var coords in spatials.Element("spatial_element")
                                                    .Elements("ordinates")
                                                    .Elements("ordinate"))
                    {
                        list.Add(new Ordinate(coords));
                    }

                    ordinates.Add(list);
                }
            }
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public override string Id
        {
            get
            {
                return reg_numb_border;
            }
        }

        /// <summary>
        /// Тип записи.
        /// </summary>
        public override string Type
        {
            get
            {
                return "Zone";
            }
        }

        /// <summary>
        /// Формрует информацию о зоне и территории.
        /// </summary>
        /// <returns>Информация о зоне и территории.</returns>
        public override string ToString()
        {
            string str = "Зона/территория"
                + "\r\n\rРегистрационный номер: " + reg_numb_border
                + "\r\nДата регистации: " + registration_date
                + "\r\nТип границы: " + typeBoundaryCode + " " + typeBoundaryValue
                + "\r\nТип Зоны: " + typeZoneCode + " " + typeZoneValue;

            if (number != "") str += "\r\nНомер: " + number;

            foreach (var list in ordinates)
            {
                foreach (var coord in list)
                {
                    str += coord.ToString() + "\n";
                }
            }

            return str;
        }
    }
}
