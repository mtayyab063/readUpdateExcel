using System;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;


namespace ConsoleApp7
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                //Create COM Objects.
                Application excelApp = new Application();
                var totalRows = 0;

                if (excelApp == null)
                {
                    Console.WriteLine("Excel is not installed!!");
                    return;
                }

                Workbook excelBook = excelApp.Workbooks.Open(@"D:\Nabil\NEW\NAFIS_500.xlsx");
                _Worksheet excelSheet = excelBook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;


                int rowCount = excelRange.Rows.Count;
                int colCount = excelRange.Columns.Count;

                int colNo = excelSheet.UsedRange.Columns.Count;
                int rowNo = excelSheet.UsedRange.Rows.Count;

                object[,] array = excelSheet.UsedRange.Value;
                for (int j = 1; j <= colNo; j++)
                {
                    for (int i = 1; i <= rowNo; i++)
                    {
                        if (array[i, j] != null)
                            if (array[i, j].ToString() == "EID")
                            {
                                //if (rowNo <= 28256)
                                //{
                                for (int m = i + 1; m < rowNo; m++)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(array[m, j].ToString())))
                                    {
                                        Console.WriteLine(Convert.ToString(array[m, j].ToString()));
                                        Console.WriteLine(m);
                                        PensionDetailService.RequiredData res = new PensionDetailService.RequiredData(); ;
                                        try
                                        {
                                            res = PensionDetailService.GetDataWithAuthentication(Convert.ToString(array[m, j].ToString())).Result;

                                        }
                                        catch (TaskCanceledException ex)
                                        {
                                            Console.WriteLine(ex);
                                        }

                                        array[m, 31] = res.IsInsured;
                                        array[m, 32] = res.IsPensionior;
                                        array[m, 33] = res.IsBenificiary;
                                        //array[m, 34] = DateTime.Now.ToString();
                                    }
                                    //if (m == 200)
                                    if (m == 28256)
                                    {
                                        totalRows = m;
                                        break;
                                    }


                                }
                            }
                        if (totalRows == 28256)
                        {
                            break;
                        }
                    }
                    if (totalRows == 28256)
                    {
                        break;
                    }
                }
                excelSheet.UsedRange.Value = array;
                excelBook.SaveAs2(@"D:\Nabil\Sheets\NAFIS_500.xlsx");
                
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                Console.ReadLine();
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
