using System;
using System.IO;
using System.Data;

namespace PhoneNumberChecker
{
    public static class CsvCreator
    {
        public static DataTable CreateDataTable(bool isValid, bool isPossible,
            string PhoneType, string InternationFormat)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("IsValid", typeof(string));
            dataTable.Columns.Add("IsPossbile", typeof(string));
            dataTable.Columns.Add("PhoneType", typeof(string));
            dataTable.Columns.Add("InternationalFormat", typeof(string));

            dataTable.Rows.Add(isValid, isPossible, PhoneType, InternationFormat);

            return dataTable;
        }

        public static byte[] ToCsvByteArray(this DataTable input)
        {
            var stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream);

            for (int i = 0; i < input.Columns.Count; i++)
            {
                sw.Write(input.Columns[i]);
                if (i < input.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow row in input.Rows)
            {
                for (int i = 0; i < input.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(row[i]))
                    {
                        string value = row[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(row[i].ToString());
                        }
                    }
                    if (i < input.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();

            return stream.ToArray();
        }
    }
}
