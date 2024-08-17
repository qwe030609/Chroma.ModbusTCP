using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    internal class CSVHelper
    {
        static CSVHelper csvHelper;
        static StreamReader reader;
        static StreamWriter writer;

        static CSVHelper()
        {
            csvHelper = new CSVHelper();
        }

        internal static CSVHelper GetInstance()
        {
            return csvHelper;
        }

        internal CSVHelper()
        {
            
        }

        ~CSVHelper()
        {

        }

        internal void ImportCSV()
        {
            // Load MDB file to old copmmand list
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select file";
            ofd.InitialDirectory = ".\\";
            ofd.Filter = "CSV file (*.csv)|*.csv";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            if (ofd.FileName.Split('.').LastOrDefault() == "csv")
            {
                // Parse csv document
                int readlineCount = 0;
                int addr_idx = 0, DataType_idx = 0, isLittleEndian_idx = 0, isReverse_idx = 0;
                string filepath = ofd.FileName;
                reader = new StreamReader(File.OpenRead(filepath), Encoding.UTF8);
                while (!reader.EndOfStream)
                {
                    string line;
                    string[] values;
                    line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                        continue;

                    readlineCount++;
                    values = csvHelper.ParseCSVRead(line);

                    if (readlineCount == 1) // read header to record specified data index
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i] == "address")
                                addr_idx = i;
                            else if (values[i] == "modbusDataType")
                                DataType_idx = i;
                            else if (values[i] == "isLittleEndian")
                                isLittleEndian_idx = i;
                            else if (values[i] == "isReverse")
                                isReverse_idx = i;
                        }
                    }
                    else
                    {
                        // Store data according to the data index above
                        if (values[DataType_idx] == "")
                            values[DataType_idx] = "bool";
                        else if (values[DataType_idx].StartsWith("s"))
                            values[DataType_idx] = values[DataType_idx].Substring(1);

                        TagStaticDatas.tagDataList.Add(new TagData()
                        {
                            TagType = int.Parse(values[addr_idx][0].ToString()),
                            Address = int.Parse(values[addr_idx].Substring(1)),
                            TagDataType = (int)Enum.Parse(typeof(ModbusTCPProtocol.TagDataType), (values[DataType_idx]).ToUpper()),
                            IsLittleEndian = bool.Parse(values[isLittleEndian_idx]),
                            IsReverse = bool.Parse(values[isReverse_idx]),
                            //Value = 0,
                        });
                    }
                }

                reader.Close();
                return;
            }
        }

        private string[] ParseCSVRead(string line)
        {
            //string[] values = line.Trim().Split(",(?=([^\"]*\"[^\"]*\")*[^\"]*$)", -1); // split content with no double quote with regex (Java)
            string[] values = Regex.Split(line.Trim(), ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");  // split content with no double quote with regex (C#)

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].StartsWith("\"")) values[i] = values[i].Substring(1, values[i].Length - 1);   // trim the double quotes (beginning)
                if (values[i].EndsWith("\"")) values[i] = values[i].Substring(0, values[i].Length - 1); // trim the double quotes (ending)
                values[i] = values[i].Replace("\"\"", "\"");    // convert content with 2 double quotes to 1 double quote
                values[i] = values[i].Trim();           // trim the whitespace tails
            }
            return values;
        }

        private void ParseCSVWrite(params string[] line)
        {
            for (int i = 0; i < line.Count(); i++)
            {
                bool quoteFlag = false;     // tag to mark if double quotes are added
                if (line[i].Contains("\""))    // if 1 double quote found in string, replace to 2 double quotes, and add double quotes both in beginning and ending
                {
                    line[i] = line[i].Replace("\"", "\"\"");
                    line[i] = "\"" + line[i] + "\"";
                    quoteFlag = true;
                }
                if (line[i].Contains(",") && !quoteFlag)   // if comma found in string, add double quotes both in beginning and ending
                {
                    line[i] = "\"" + line[i] + "\"";
                }
            }
        }

        //private void SaveRecipeListAsCSV(ObservableCollection<RecipeModel> recipeList)
        //{
        //    string export_filepath = @"D:\My docu\Projects\WPFMVVM\Output\SampleRecipe.csv";

        //    writer = new StreamWriter(File.OpenWrite(export_filepath), Encoding.UTF8);

        //    if (writer == null)
        //    {
        //        FileBase.MessageBoxTimeoutA((IntPtr)0, "匯出Recipe List失敗!!", "", 0, 0, 1500); // 直接调用 1秒后自动关闭
        //        return;
        //    }
                

        //    for (int idx = 0; idx < recipeList.Count; idx++)
        //    {
        //        RecipeModel recipe = recipeList[idx];
        //        //string name = recipe.Name;
        //        //string ingrediant = recipe.Ingrediant;
        //        //string direction = recipe.Direction;
        //        string[] strRecipe = new string[] { recipe.Name, recipe.Ingrediant, recipe.Direction };

        //        //ParseCSVWrite(name, ingrediant, direction);
        //        ParseCSVWrite(strRecipe);

        //        var line = string.Format("{0},{1},{2}", strRecipe[0], strRecipe[1], strRecipe[2]);
        //        writer.WriteLine(line);
        //        writer.Flush();
        //    }

        //    writer.Close();
        //    FileBase.MessageBoxTimeoutA((IntPtr)0, $@"匯出Recipe List成功，存檔路徑: {Directory.GetParent(export_filepath).ToString()}", "", 0, 0, 1500); // 直接调用 1秒后自动关闭
        //}


        //private void SaveAsCSV(ObservableCollection<RecipeModel> recipeList)
        //{
        //    //// Configure open file dialog box
        //    //Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        //    //dlg.FileName = "Document"; // Default file name
        //    //dlg.DefaultExt = ".txt"; // Default file extension
        //    //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

        //    //// Show open file dialog box
        //    //Nullable<bool> result = dlg.ShowDialog();

        //    //// Process open file dialog box results
        //    //if (result == true)
        //    //{
        //    //    // Open document
        //    //    string filename = dlg.FileName;
        //    //}

        //    DateTime tm = DateTime.Now;
        //    string timestamp = string.Format("{0:####}{1:00}{2:00}_{3:00}{4:00}{5:00}", tm.Year, tm.Month, tm.Day, tm.Hour, tm.Minute, tm.Second);
        //    string initialDirectory = @"D:\My docu\Projects\WPFMVVM\Output";

        //    // Configure save file dialog box
        //    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
        //    dlg.FileName = "Recipe_" + timestamp;                       // Default file name
        //    dlg.DefaultExt = ".csv";                                    // Default file extension
        //    dlg.Filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*"; // Filter files by extension

        //    if (!Directory.Exists(initialDirectory))
        //    {
        //        dlg.InitialDirectory = @"C:\";
        //    }
        //    else
        //        dlg.InitialDirectory = initialDirectory;

        //    // Remember Last Dir
        //    dlg.RestoreDirectory = true;

        //    // Show save file dialog box
        //    Nullable<bool> result = dlg.ShowDialog();

        //    // Process save file dialog box results
        //    if (result == true)
        //    {
        //        // Save document
        //        string filename = dlg.FileName;
        //        writer = new StreamWriter(File.OpenWrite(filename), Encoding.UTF8);
        //        if (writer == null || recipeList.Count == 0)
        //        {
        //            writer.Close();
        //            return;
        //        }
                    

        //        for (int idx = 0; idx < recipeList.Count; idx++)
        //        {
        //            RecipeModel recipe = recipeList[idx];
        //            string[] strRecipe = new string[] { recipe.Name, recipe.Ingrediant, recipe.Direction };

        //            ParseCSVWrite(strRecipe);

        //            var line = string.Format("{0},{1},{2}", strRecipe[0], strRecipe[1], strRecipe[2]);
        //            writer.WriteLine(line);
        //            writer.Flush();
        //        }

        //        writer.Close();

        //        FileBase.MessageBoxTimeoutA((IntPtr)0, $@"匯出Recipe List成功，存檔路徑: {Directory.GetParent(filename).ToString()}", "", 0, 0, 1500); // 直接调用 1秒后自动关闭
        //    }
        //    else
        //    {
        //        FileBase.MessageBoxTimeoutA((IntPtr)0, "匯出Recipe List失敗!!", "", 0, 0, 1500); // 直接调用 1秒后自动关闭
        //    }
        //}
    }
}
