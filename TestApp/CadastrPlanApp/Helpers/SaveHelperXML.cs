using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CadastrPlanApp
{
    /// <summary>
    /// Работа с сохранением XML-файла.
    /// </summary>
    static public class SaveHelperXML
    {
        /// <summary>
        /// Сохраняет документ с нужными объектами.
        /// </summary>
        /// <param name="listId">Список с идентификаторами объектов.</param>
        /// <param name="document">Исходный документ.</param>
        /// <param name="path">Путь нового файла.</param>
        public static void SaveToFile(List<string> listId, XDocument document, string path)
        {
            if (listId == null)
            {
                throw new ArgumentException("Пустой аргумент.", nameof(listId));
            }

            if (document == null)
            {
                throw new ArgumentException("Пустой аргумент.", nameof(document));
            }

            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Пустой аргумент.", nameof(path));
            }

            var land = document.Element("extract_cadastral_plan_territory")
                                   .Element("cadastral_blocks")
                                   .Element("cadastral_block")
                                   .Element("record_data")
                                   .Element("base_data")
                                   .Element("land_records").
                                   Elements("land_record");

            land.Where(x => !listId.Contains(x.Element("object")
                                              .Element("common_data")
                                              .Element("cad_number").Value)).Remove();


            var build = document.Element("extract_cadastral_plan_territory")
                                   .Element("cadastral_blocks")
                                   .Element("cadastral_block")
                                   .Element("record_data")
                                   .Element("base_data")
                                   .Element("build_records")
                                   .Elements("build_record");

            build.Where(x => !listId.Contains(x.Element("object")
                                              .Element("common_data")
                                              .Element("cad_number").Value)).Remove();

            var construction = document.Element("extract_cadastral_plan_territory")
                                   .Element("cadastral_blocks")
                                   .Element("cadastral_block")
                                   .Element("record_data")
                                   .Element("base_data")
                                   .Element("construction_records")
                                   .Elements("construction_record");

            construction.Where(x => !listId.Contains(x.Element("object")
                                  .Element("common_data")
                                  .Element("cad_number").Value)).Remove();

            var municipal = document.Element("extract_cadastral_plan_territory")
                                    .Element("cadastral_blocks")
                                    .Element("cadastral_block")
                                    .Element("municipal_boundaries")
                                    .Elements("municipal_boundary_record");

            municipal.Where(x => !listId.Contains(x.Element("b_object_municipal_boundary")
                                                    .Element("b_object")
                                                    .Element("reg_numb_border").Value)).Remove();

            var spatial = document.Element("extract_cadastral_plan_territory")
                                    .Element("cadastral_blocks")
                                    .Element("cadastral_block")
                                    .Element("spatial_data")
                                    .Elements("entity_spatial");

            spatial.Where(x => !listId.Contains(x.Element("sk_id").Value)).Remove();

            var zone = document.Element("extract_cadastral_plan_territory")
                                    .Element("cadastral_blocks")
                                    .Element("cadastral_block")
                                    .Element("zones_and_territories_boundaries")
                                    .Elements("zones_and_territories_record");

            zone.Where(x => !listId.Contains(x.Element("b_object_zones_and_territories")
                                                    .Element("b_object")
                                                    .Element("reg_numb_border").Value)).Remove();
            document.Save(path);
        }
    }
}
