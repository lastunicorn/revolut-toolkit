namespace DustInTheWind.Revolut.Toolkit.Csv;

internal enum CsvDocumentReadState
{
    HeaderRow = 0,
    DataRow,
    Ended
}