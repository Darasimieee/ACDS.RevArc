using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Helpers
{
    public class ExcelHelper
    {

        public List<T> ImportExcel<T>(string filePath) where T : new()
        {
            List<T> data = new List<T>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var startRow = worksheet.Dimension.Start.Row + 1;

                for (int row = startRow; row <= worksheet.Dimension.End.Row; row++)
                {
                    T item = new T();
                    foreach (var property in typeof(T).GetProperties())
                    {
                        var columnIndex = GetColumnIndex(worksheet, property.Name);
                        if (columnIndex != -1)
                        {
                            object cellValue = worksheet.Cells[row, columnIndex].Value;
                            if (cellValue != null)
                            {
                                property.SetValue(item, Convert.ChangeType(cellValue, property.PropertyType));
                            }
                        }
                    }
                    data.Add(item);
                }
            }

            return data;
        }

        private int GetColumnIndex(ExcelWorksheet worksheet, string columnName)
        {
            for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
            {
                if (worksheet.Cells[1, i].Text.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

      
    }
}

