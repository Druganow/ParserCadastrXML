using System.Xml.Linq;

namespace CadastrPlanApp
{
    /// <summary>
    /// Здание.
    /// </summary>
    public class Build : RecordData
    {
        /// <summary>
        /// Площадь.
        /// </summary>
        private readonly string area;

        /// <summary>
        /// Код назначения.
        /// </summary>
        private readonly string purposeCode;

        /// <summary>
        /// Назначение.
        /// </summary>
        private readonly string purposeValue;

        /// <summary>
        /// Цена.
        /// </summary>
        private readonly string cost;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="element">XML-узел с информацией record.</param>
        public Build(XElement element) : base(element)
        {
            var paramsElement = element.Element("params");
            var costElement = element.Element("cost");

            if (paramsElement != null)
            {
                area = paramsElement.Element("area").Value;
                purposeCode = paramsElement.Element("purpose").Element("code").Value;
                purposeValue = paramsElement.Element("purpose").Element("value").Value;
            }

            if (costElement != null)
            {
                cost = costElement.Value;
            }
        }

        /// <summary>
        /// Тип записи.
        /// </summary>
        public override string Type
        {
            get
            {
                return "Build";
            }
        }

        /// <summary>
        /// Формирует строку с параметрами здания.
        /// </summary>
        /// <returns>Параметры здания.</returns>
        public string GetParamsString()
        {
            return $"\nПлощадь: {area}\n" +
                $"Код назначения: {purposeCode}\n" +
                $"Назначение: {purposeValue}\n";
        }

        /// <summary>
        /// Формирует информацию о здании.
        /// </summary>
        /// <returns>Информация о здании.</returns>
        public override string ToString()
        {
            return base.GetObjectString()
                + GetParamsString()
                + $"Цена: {cost}";
        }
    }
}
