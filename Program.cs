using Newtonsoft.Json; 

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);   

var salesFiles = FindFiles(storesDirectory, "*");

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

//##############
string welcome = "Sales Sumary \n-----------------";
var salesSumaryPath = Path.Combine(salesTotalDir, "SalesSumary.txt");
if (File.Exists(salesSumaryPath))
{
    File.Delete(salesSumaryPath);
}

var onlySalesJsonFiles = FindFiles(storesDirectory, "sales.json");
ReadSalesFile(onlySalesJsonFiles);
var salesFilesAndTotals = ReadSalesFile(onlySalesJsonFiles);
var details = string.Join(Environment.NewLine + " ", salesFilesAndTotals);

File.AppendAllText(Path.Combine(salesTotalDir, "SalesSumary.txt"), $"{welcome}{Environment.NewLine} Total Sales: {salesTotal:C2}{Environment.NewLine}\n Details: {Environment.NewLine}  {details}{Environment.NewLine}");

//##############

IEnumerable<string> FindFiles(string folderName, string fileName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, fileName, SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

IEnumerable<string> ReadSalesFile(IEnumerable<string> salesJsonFile)
{
    List<string> totalInSalesFiles = new List<string>();
    
    foreach(var file in salesJsonFile)
    {
        var salesJson = File.ReadAllText(file);
        dynamic? data = JsonConvert.DeserializeObject(salesJson);
        decimal total = data?.Total;
        string jsonFileName = Path.GetFileName(file);
        totalInSalesFiles.Add($" {jsonFileName}: ${total}");
        
    }

    return totalInSalesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    
    foreach (var file in salesFiles)
    {      
        string salesJson = File.ReadAllText(file);
    
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
    
        salesTotal += data?.Total ?? 0;
    }
    
    return salesTotal;
}

record SalesData (double Total);
