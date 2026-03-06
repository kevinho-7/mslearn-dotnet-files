using System.IO;
using System.Collections.Generic;

//var salesFiles = FindFiles("D:/Kevin/Documentos/BYUI/CSE325/mslearn-dotnet-files/stores");
// var salesFiles = FindFiles("stores");
var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");
var salesFiles = FindFiles(storesDirectory);

foreach(var file in salesFiles)
{
    Console.WriteLine(file);
}

IEnumerable<string> FindFiles(string folderName){
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach(var file in foundFiles)
    {
        if (file.EndsWith("sales.json"))
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}



