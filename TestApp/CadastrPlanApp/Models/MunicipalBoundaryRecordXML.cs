using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CadastrPlanApp
{
    /// <summary>
    /// Муниципальный участок.
    /// </summary>
    class MunicipalBoundaryRecordXML : CadastrObject
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
        /// <param name="element">XML-узел с информацией о муниципальном участке.</param>
        public MunicipalBoundaryRecordXML(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentException("Пустой аргумент.");
            }

            registration_date = element.Element("record_info").Element("registration_date").Value;
            var b_object = element.Element("b_object_municipal_boundary").Element("b_object");
            var entity_spatial = element.Element("b_contours_location")
                                        .Element("contours")
                                        .Element("contour")
                                        .Element("entity_spatial");

            if (b_object != null)
            {
                reg_numb_border = b_object.Element("reg_numb_border").Value;
                typeBoundaryCode = b_object.Element("type_boundary").Element("code").Value;
                typeBoundaryValue = b_object.Element("type_boundary").Element("value").Value;
            }

            if (entity_spatial != null)
            {
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
                return "Municipal";
            }
        }

        /// <summary>
        /// Форимрует информацию о муниципальном участке.
        /// </summary>
        /// <returns>Информация о муниципальном участке.</returns>
        public override string ToString()
        {
            string str = "Муниципальные границы"
                + $"\nИдентификатор системы координат: {skId}"
                + "\nРегистрационный номер: " + reg_numb_border
                + "\nДата регистации: " + registration_date
                + "\nТип границы: " + typeBoundaryCode + " " + typeBoundaryValue
                + "\n";

            foreach (var coord in ordinates)
            {
                str += coord.ToString() + "\n";
            }

            return str;
        }
    }
}