using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace OfficeSpace.Models
{
    public class ExcelExportExtension
    {
        readonly static Dictionary<string, string> numberFormat = new Dictionary<string, string>();

        static ExcelExportExtension()
        {
            numberFormat.Add("Decimal", "#,##0.000000");
            numberFormat.Add("DateTime", "dd/MM/yyyy");
        }

        public static string ExcelContentType
        {
            get { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
        }

        static DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(string.IsNullOrEmpty(property.DisplayName) ? property.Name : property.DisplayName, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static byte[] ExportToExcel(DataTable dataTable, string tableName, string heading, bool showSrNo, string userDateFormat, Dictionary<string, string> columnDisplayName = null, string[] columnsToRemove = null)
        {
            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(tableName);
                workSheet.View.ShowGridLines = false;

                int startRowFrom = string.IsNullOrEmpty(heading) ? 1 : 3;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }

                if (columnDisplayName != null)
                {
                    foreach (DataColumn col in dataTable.Columns)
                    {

                        if (columnDisplayName.ContainsKey(col.ColumnName))
                        {
                            string KeyName = col.ColumnName;
                            col.ColumnName = columnDisplayName[col.ColumnName];
                            if (columnsToRemove != null)
                            {
                                for (int k = 0; k < columnsToRemove.Length; k++)
                                {
                                    if (columnsToRemove[k] == KeyName)
                                    {
                                        columnsToRemove[k] = columnDisplayName[KeyName];
                                    }
                                }
                            }

                        }
                    }
                }


                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content
                int columnIndex = 1;
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                        workSheet.Column(columnIndex).AutoFit();
                        columnIndex++;
                    }

                    using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    }
                }


            

                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    ExcelTableCollection tblcollection = workSheet.Tables;
                    ExcelTable table = tblcollection.Add(r, tableName);
                    table.TableStyle = TableStyles.None;
                }

              

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (numberFormat.ContainsKey((dataTable.Columns[i].DataType).Name))
                    {
                        switch ((dataTable.Columns[i].DataType).Name)
                        {
                            case "Decimal":
                                switch (dataTable.Columns[i].ColumnName)
                                {
                                    case "Difference":
                                    case "Dealt Rate":
                                    case "NCFX Reference Rate":
                                    case "BP Cost":
                                    case "Fwd Pips":
                                        workSheet.Column(i + 1).Style.Numberformat.Format = numberFormat[(dataTable.Columns[i].DataType).Name];
                                        break;

                                    default:
                                        workSheet.Column(i + 1).Style.Numberformat.Format = "#,##0.00";
                                        break;
                                }                               
                                break;

                            default:
                                workSheet.Column(i + 1).Style.Numberformat.Format = userDateFormat;
                                break;

                        }


                    }
                }

                if (columnsToRemove != null)
                {
                    if (columnsToRemove.Length > 0)
                    {
                        for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                        {
                            if (i == 0 && showSrNo)
                            {
                                continue;
                            }

                            if (columnsToRemove.Contains(dataTable.Columns[i].ColumnName))
                            {
                                workSheet.DeleteColumn(i + 1);
                            }
                        }
                    }
                }


                if (!string.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportToExcel<T>(List<T> data, string tableName, string heading, bool showSlno,string userDateFormat, Dictionary<string, string> columnDisplayName = null, string[] columnsToRemove = null)
        {
            return ExportToExcel(ListToDataTable<T>(data), tableName, heading, showSlno, userDateFormat, columnDisplayName, columnsToRemove);
        }
    }
}
