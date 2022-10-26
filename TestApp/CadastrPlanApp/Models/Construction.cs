using System.Xml.Linq;

namespace CadastrPlanApp
{
    /// <summary>
    /// Сооружение.
    /// </summary>
    class Construction : RecordData
    {
        /// <summary>
        /// Назначение.
        /// </summary>
        private readonly string purpose;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="element">XML-узел с информацией о строении.</param>
        public Construction(XElement element) : base(element)
        {
            purpose = element.Element("params").Element("purpose").Value;
        }

        /// <summary>
        /// Формирует информацию о строении.
        /// </summary>
        /// <returns>Информация о строении.</returns>
        public override string ToString()
        {
            return base.GetObjectString()
                + $"\nНазначение: {purpose}"
                + base.GetAddressString();
        }

        /// <summary>
        /// Тип записи.
        /// </summary>
        public override string Type
        {
            get
            {
                return "Construction";
            }
        }
    }
}
