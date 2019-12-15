using UnityEngine;

/*
 * Author   : Vikram
 * Class    : DataReader
 * Function : Reading CSV data from the 'Resources' folder 
 */

public class DataReader: MonoBehaviour
{
    public class DataPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public string Cluster { get; set; }
        public string Tooltip { get; set; }
    }

    public DataPoint[] DataFrame { get; set; }

    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
    
    char[] lineSeperator = { '\n' };
    char[] textSeperator = { ',' };

    public void LoadData()
    {
        string fileName = "Iris"; // to be changed later, remove harcoded value
        bool is3DPlot = true; // to be passed to this function as a parameter


        TextAsset fileContents = Resources.Load<TextAsset>(fileName);

        string[] data = fileContents.text.Split(lineSeperator);
        string[] header = data[0].Split(textSeperator); // Get the header properties of the file

        RowCount = GetRowCount(data);
        ColumnCount = GetColumnCount(header);

        LoadDataFrame(data, is3DPlot); // loads the contents from 'data' to 'DataFrame'

        // print(RowCount + " " + ColumnCount);
    }

    private int GetRowCount(string[] data)
    {
        // Count ignoring the header and the last blank line
        return data.Length - 2;
    }

    private int GetColumnCount(string[] header)
    {
        return header.Length;
    }

    private void LoadDataFrame(string[] data, bool is3DPlot)
    {
        int indexOffset = 1; // Used for ignoring the header

        DataFrame = new DataPoint[RowCount]; // Create a dataframe of RowCount size


        for (int i = 0; i < RowCount; i++)
        {
            string[] row = data[i + indexOffset].Split(textSeperator);
            int tokenIndex = 0; // Eg: {'2', '3', 'A', 'Info'} -> if tokenIndex = 2, then row[tokenIndex] is 'A', keeping track of tokens in row[]

            DataFrame[i] = new DataPoint();
            

            DataFrame[i].X = float.Parse(row[tokenIndex++]);
            DataFrame[i].Y = float.Parse(row[tokenIndex++]);

            if (is3DPlot) // Check if the DataFrame loads 2D or 3D
            {
                DataFrame[i].Z = float.Parse(row[tokenIndex++]);
            }

            DataFrame[i].Cluster= row[tokenIndex++];
            DataFrame[i].Tooltip = row[tokenIndex++];

            // print(DataFrame[i].X + " " + DataFrame[i].Y + " " + DataFrame[i].Z + " " + DataFrame[i].Cluster + " " + DataFrame[i].Tooltip);
        }
    }

}