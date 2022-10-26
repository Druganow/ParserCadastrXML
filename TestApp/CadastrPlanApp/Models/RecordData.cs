using System;
using System.Xml.Linq;

namespace CadastrPlanApp
{
    /// <summary>
    /// Объект со свойствами "object" и "address".
    /// </summary>
    public class RecordData : CadastrObject
    {
        /// <summary>
        /// Код ОКАТО.
        /// </summary>
        private readonly string okato = "";

        /// <summary>
        /// Код КЛАДР.
        /// </summary>
        private readonly string kladr = "";

        /// <summary>
        /// Код региона.
        /// </summary>
        private readonly string code_region = "";

        /// <summary>
        /// Название региона.
        /// </summary>
        private readonly string value_region = "";

        /// <summary>
        /// Тип района.
        /// </summary>
        private readonly string type_district = "";

        /// <summary>
        /// Название района.
        /// </summary>
        private readonly string name_district = "";

        /// <summary>
        /// Тип населенного пункта.
        /// </summary>
        private readonly string type_locality = "";

        /// <summary>
        /// Название населенного пункта.
        /// </summary>
        private readonly string name_locality = "";

        /// <summary>
        /// Тип улицы.
        /// </summary>
        private readonly string type_street = "";

        /// <summary>
        /// Название улицы.
        /// </summary>
        private readonly string name_street = "";

        /// <summary>
        /// Тип дома.
        /// </summary>
        private readonly string type_level1 = "д.";

        /// <summary>
        /// Номер дома.
        /// </summary>
        private readonly string name_level1 = "";

        /// <summary>
        /// Полный адрес.
        /// </summary>
        private readonly string readable_address = "";

        /// <summary>
        /// Дополнонительная информация.
        /// </summary>
        private readonly string other = "";

        /// <summary>
        /// Код типа земельного участка.
        /// </summary>
        private readonly string object_type_code = "";

        /// <summary>
        /// Тип земельного участка.
        /// </summary>
        private readonly string object_type_value = "";

        /// <summary>
        /// Кадастровый номер.
        /// </summary>
        private readonly string cad_number = "";

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public override string Id
        {
            get
            {
                return cad_number;
            }
        }

        /// <summary>
        /// Тип записи.-
        /// </summary>
        public override string Type
        {
            get
            {
                return "record";
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="element">XML-узел с информацией record.</param>
        public RecordData(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentException("Пустой аргумент");
            }

            var objectElement = element.Element("object").Element("common_data");
            var address = element.Element("address_location").Element("address");

            if (objectElement != null)
            {
                object_type_code = objectElement.Element("type").Element("code").Value;
                object_type_value = objectElement.Element("type").Element("value").Value;
                cad_number = objectElement.Element("cad_number").Value;
            }

            if (address != null)
            {
                var fias = address.Element("address_fias");

                if (fias != null)
                {
                    var levelSection = fias.Element("level_settlement");
                    var detailed_level = fias.Element("detailed_level");

                    if (levelSection != null)
                    {
                        okato = levelSection.Element("okato").Value;
                        kladr = levelSection.Element("kladr").Value;

                        var region = levelSection.Element("region");
                        var district = levelSection.Element("district");
                        var locality = element.Element("locality");

                        if (region != null)
                        {
                            code_region = region.Element("code").Value;
                            value_region = region.Element("value").Value;
                        }

                        if (district != null)
                        {
                            type_district = district.Element("type_district").Value;
                            name_district = district.Element("name_district").Value;
                        }

                        if (locality != null)
                        {
                            type_locality = locality.Element("type_locality").Value;
                            name_locality = locality.Element("name_locality").Value;
                        }
                    }

                    if (detailed_level != null)
                    {
                        var street = detailed_level.Element("street");
                        var level = detailed_level.Element("level1");
                        var other_elem = detailed_level.Element("other");

                        if (street != null)
                        {
                            type_street = street.Element("type_street").Value;
                            name_street = street.Element("name_street").Value;
                        }

                        if (level != null)
                        {
                            if (level.Element("type_level1") != null)
                                type_level1 = level.Element("type_level1").Value;

                            name_level1 = level.Element("name_level1").Value;
                        }

                        if (other_elem != null)
                            other = other_elem.Value;
                    }
                }

                if (element.Element("readable_address") != null)
                    readable_address = element.Element("readable_address").Value;
            }

        }

        /// <summary>
        /// Формирует строку с информацией об объекте.
        /// </summary>
        /// <returns>Строка с информацией об объекте.</returns>
        public string GetObjectString()
        {
            string str = "Тип участка: " + object_type_value
                + "\r\nКадастровый номер : " + cad_number;

            if (object_type_code != null)
                str += "\r\nВид объекта: " + object_type_code;

            return str;
        }

        /// <summary>
        /// Формирует строку с информацией об адресе.
        /// </summary>
        /// <returns>Строка с адресом.</returns>
        public string GetAddressString()
        {
            string str = "\nOkato : " + okato + "\nKladr: " + kladr;

            if (value_region != "-") str += "\nРегион: " + code_region + " " + value_region;
            if (name_district != "-") str += "\nРайон: " + type_district + ". " + name_district;
            if (name_locality != "-") str += "\nЛокация: " + type_locality + ". " + name_locality;
            if (name_street != "-") str += "\nУлица: " + type_street + ". " + name_street;
            if (type_level1 != "-") str += "\nДом: " + type_level1 + name_level1;
            if (other != "-") str += "\r\nДополнительно: " + other;
            if (readable_address != "-") str += "\nПолный адрес: " + readable_address;

            return str;
        }

        /// <summary>
        /// Формирует информацию об объекте.
        /// </summary>
        /// <returns>Информация об объекте.</returns>
        public override string ToString()
        {
            return $"{GetObjectString()}\n{GetAddressString()}";
        }
    }
}
