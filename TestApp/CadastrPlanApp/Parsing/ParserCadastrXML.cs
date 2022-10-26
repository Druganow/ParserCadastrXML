using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CadastrPlanApp
{
    /// <summary>
    /// Парсер кадастрового XML.
    /// </summary>
    static public class ParserCadastrXML
    {
        /// <summary>
        /// Формирует словарь типа "идентификатор - объект".
        /// </summary>
        /// <param name="document">XML документ.</param>
        /// <returns>Словарь типа "идентификатор - объект".</returns>
        static public Dictionary<string, CadastrObject> ParsingData(XDocument document)
        {
            if (document == null)
            {
                throw new ArgumentException("Пустой аргумент");
            }

            var dataSet = new Dictionary<string, CadastrObject>();
            var data = document.Element("extract_cadastral_plan_territory")
                                .Element("cadastral_blocks")
                                .Element("cadastral_block");

            foreach (var land in data.Element("record_data")
                                    .Element("base_data")
                                    .Element("land_records")
                                    .Elements("land_record"))
            {
                CadastrObject ob = new Land(land);
                dataSet.Add(ob.Id, ob);
            }

            foreach (var land in data.Element("record_data")
                                    .Element("base_data")
                                    .Element("build_records")
                                    .Elements("build_record"))
            {
                CadastrObject ob = new Build(land);
                dataSet.Add(ob.Id, ob);
            }

            foreach (var land in data.Element("record_data")
                                    .Element("base_data")
                                    .Element("construction_records")
                                    .Elements("construction_record"))
            {
                CadastrObject ob = new Construction(land);
                dataSet.Add(ob.Id, ob);
            }

            foreach (var land in data.Element("spatial_data")
                                        .Elements("entity_spatial"))
            {
                CadastrObject ob = new Spatial(land);
                dataSet.Add(ob.Id, ob);
            }

            foreach (var land in data.Element("municipal_boundaries")
                                       .Elements("municipal_boundary_record"))
            {
                CadastrObject ob = new MunicipalBoundaryRecordXML(land);
                dataSet.Add(ob.Id, ob);
            }

            foreach (var land in data.Element("zones_and_territories_boundaries")
                                        .Elements("zones_and_territories_record"))
            {
                CadastrObject ob = new ZonesAndTerritoriesRecord(land);
                dataSet.Add(ob.Id, ob);
            }

            return dataSet;
        }
    }
}
