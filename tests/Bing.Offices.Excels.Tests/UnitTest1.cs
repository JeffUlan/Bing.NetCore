using System;
using Bing.Offices.Excels.Abstractions;
using Bing.Offices.Excels.Core.Styles;
using Bing.Offices.Excels.Npoi.Core;
using Xunit;

namespace Bing.Offices.Excels.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            IWorkbook workbook=new Workbook(ExcelVersion.Xlsx);
            var sheet = workbook.CreateSheet("���Թ�����");
            var row = sheet.CreateRow();
            var cell = row.CreateCell();
            cell.SetValue("������Ϣ11111111111111111111111111");
            cell.SetStyle(new CellStyle()
            {
                FontColor = Color.Blue,
                BackgroundColor = Color.Red,
                FillPattern = FillPattern.SolidForeground
            });
            workbook.SaveToFile("D:\\����Npoi_007.xlsx");
        }
    }
}
