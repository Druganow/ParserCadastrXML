namespace CadastrPlanApp
{
    /// <summary>
    /// Объект в кадастре.
    /// </summary>
    abstract public class CadastrObject
    {
        /// <summary>
        /// Тип.
        /// </summary>
        abstract public string Type
        {
            get;
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        abstract public string Id
        {
            get;
        }
    }
}
