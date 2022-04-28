#region Usings

using System.ComponentModel;
using CSharpFunctionalExtensions;
using OfficeOpenXml;
using UltimatePlaylist.Common.Mvc.Attributes;

#endregion

namespace UltimatePlaylist.Common.Mvc.File
{
    public static class DataExportUtil
    {
        public static byte[] GetPlayerPaymentFile<T>(IReadOnlyCollection<T> modelList)
        {
            byte[] excel;

            var (dataToExcel, columnsWithNames) = GetDataToExcel(modelList);
            using (var package = new ExcelPackage())
            {
                var worksheet = FillExcelWithData(dataToExcel, columnsWithNames, package);
                AddBlankRowsForGroupedData(worksheet, "Prize Tier");
                excel = package.GetAsByteArray();
            }

            return excel;
        }

        public static byte[] GetExcelFile<T>(IReadOnlyCollection<T> modelList)
            where T : class
        {
            byte[] excel;

            var (dataToExcel, columnsWithNames) = GetDataToExcel(modelList);

            using (var package = new ExcelPackage())
            {
                FillExcelWithData(dataToExcel, columnsWithNames, package);
                excel = package.GetAsByteArray();
            }

            return excel;
        }

        private static ExcelWorksheet FillExcelWithData(Dictionary<string, List<string>> dataToExcel, Dictionary<string, string> columnsWithNames, ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet");
            var col = 1;

            for (var i = 0; i <= columnsWithNames.Count - 1; i++)
            {
                var row = 1;

                if (columnsWithNames.ElementAtOrDefault(i).Key != null)
                {
                    var columnHeader = columnsWithNames.ElementAt(i).Value.ToString();
                    worksheet.Cells[row, col].Value = columnHeader;
                    row++;

                    var columnName = columnsWithNames.ElementAt(i).Key.ToString();

                    foreach (var values in dataToExcel.Where(x => x.Key == columnName).Select(x => x.Value))
                    {
                        values.ForEach(value =>
                        {
                            worksheet.Cells[row, col].Value = value;
                            row++;
                        });
                    }
                }

                col++;
            }

            return worksheet;
        }

        private static void AddBlankRowsForGroupedData(ExcelWorksheet worksheet, string headerValue = null)
        {
            if (string.IsNullOrEmpty(headerValue))
            {
                return;
            }

            var cells = worksheet.Cells.ToList();
            var prizeTier = cells.FirstOrDefault(x => (x.Value as string) == headerValue);
            if (prizeTier == null)
            {
                return;
            }

            var column = prizeTier.Start.Column;
            var rowWithData = 2;
            var rows = cells.Where(x => x.Start.Column == column && x.Start.Row >= rowWithData)
                    .Select(x => new { Value = x.Value as string, Row = x.Start.Row })
                    .GroupBy(x => x.Value)
                    .Select(x => x.Max(c => c.Row));

            var rowToAdd = 1;
            foreach (var rowToMove in rows)
            {
                worksheet.InsertRow(rowToMove + rowToAdd, 1);
                rowToAdd++;
            }
        }

        private static (Dictionary<string, List<string>> excelData, Dictionary<string, string> columns) GetDataToExcel<T>(IReadOnlyCollection<T> modelList)
        {
            var modelRecord = modelList.FirstOrDefault();

            var columnsWithNames = new Dictionary<string, string>();

            var propDictionary = BuildModelPropertiesDictionary(modelRecord, typeof(T), ref columnsWithNames);
            var data = GetValues(propDictionary, modelList);

            var dataToExcel = new Dictionary<string, List<string>>();

            foreach (var column in columnsWithNames)
            {
                var values = new List<string>();
                data.ForEach(dictionary =>
                {
                    values.AddRange(dictionary.Where(d => d.Key == column.Key).Select(d => d.Value).ToList());
                });
                dataToExcel.Add(column.Key, values);
            }

            return (dataToExcel, columnsWithNames);
        }

        private static List<Dictionary<string, string>> GetValues<T>(Dictionary<string, Tuple<PropertyDescriptor, Type>> propDictionary, IEnumerable<T> modelList)
        {
            var ret = new List<Dictionary<string, string>>();

            foreach (var model in modelList)
            {
                var modelDict = new Dictionary<string, string>();
                foreach (var dict in propDictionary)
                {
                    if (dict.Value == null)
                    {
                        modelDict.Add(dict.Key, string.Empty);
                    }
                    else if (dict.Value.Item2 == typeof(T))
                    {
                        modelDict.Add(dict.Key, dict.Value.Item1.GetValue(model)?.ToString());
                    }
                    else
                    {
                        var modelProperties = TypeDescriptor.GetProperties(model);
                        foreach (PropertyDescriptor property in modelProperties)
                        {
                            if (property.PropertyType == dict.Value.Item2)
                            {
                                var subModel = property.GetValue(model);
                                modelDict.Add(
                                    dict.Key,
                                    subModel != null ? dict.Value.Item1.GetValue(subModel)?.ToString() : string.Empty);
                            }
                        }
                    }
                }

                ret.Add(modelDict);
            }

            return ret;
        }

        private static Dictionary<string, string> GetAllProperties(Type type)
        {
            if (type == typeof(string) || type == typeof(DateTime))
            {
                return null;
            }

            var descriptors = TypeDescriptor.GetProperties(type);
            var allProps = new Dictionary<string, string>();

            foreach (PropertyDescriptor desc in descriptors)
            {
                var children = GetAllProperties(desc.PropertyType);
                if (children != null && children.Count > 0)
                {
                    children.ToList().ForEach(c =>
                    {
                        if (!allProps.ContainsKey(c.Key))
                        {
                            allProps.Add(c.Key, c.Value);
                        }
                    });
                }
                else
                {
                    var attr = desc.Attributes.OfType<UltimateColumnAttribute>()?
                        .FirstOrDefault();

                    if (attr != null)
                    {
                        allProps.Add(desc.Name, attr.Name);
                    }
                }
            }

            if (allProps != null && !allProps.Any())
            {
                return null;
            }

            return allProps;
        }

        private static Dictionary<string, Tuple<PropertyDescriptor, Type>> BuildModelPropertiesDictionary(object modelRecord, Type modelType, ref Dictionary<string, string> columnsWithNames)
        {
            columnsWithNames = GetAllProperties(modelType);

            var ret = new Dictionary<string, Tuple<PropertyDescriptor, Type>>();

            var modelProperties = TypeDescriptor.GetProperties(modelType);

            bool propertyFound = false;

            foreach (var column in columnsWithNames)
            {
                propertyFound = false;
                foreach (PropertyDescriptor property in modelProperties)
                {
                    var attribute = property.Attributes.OfType<UltimateColumnAttribute>()
                        .FirstOrDefault();

                    if (Matches(attribute, property, column.Key))
                    {
                        ret.Add(column.Key, new Tuple<PropertyDescriptor, Type>(property, modelType));
                        propertyFound = true;
                        break;
                    }
                }

                if (!propertyFound)
                {
                    ret.Add(column.Key, null);
                }
            }

            return ret;
        }

        private static bool Matches(UltimateColumnAttribute attribute, PropertyDescriptor property, string column)
        {
            if (attribute == null)
            {
                return false;
            }

            return property.Name.Equals(column, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
