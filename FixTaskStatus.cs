using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string basePath = "/Users/bilawalrizky/Documents/2025 - Project/SAT/TaskManagementSystem/TaskManagementSystem.Web/Views";
        
        // Fix all .cshtml files
        ProcessDirectory(basePath);
        
        Console.WriteLine("All TaskStatus references have been fixed!");
    }
    
    static void ProcessDirectory(string targetDirectory)
    {
        // Process the list of files found in the directory
        string[] fileEntries = Directory.GetFiles(targetDirectory, "*.cshtml");
        foreach (string fileName in fileEntries)
            ProcessFile(fileName);

        // Recurse into subdirectories
        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
            ProcessDirectory(subdirectory);
    }
    
    static void ProcessFile(string path)
    {
        Console.WriteLine($"Processing {path}...");
        
        string content = File.ReadAllText(path);
        string originalContent = content;
        
        // Replace TaskStatus references with fully qualified names
        // Pattern to match TaskStatus.XXX where XXX is Todo, InProgress, Completed, or Cancelled
        // But not if it's already prefixed with the full namespace
        content = Regex.Replace(content, 
            @"(?<!TaskManagementSystem\.Web\.Models\.)TaskStatus\.(Todo|InProgress|Completed|Cancelled)", 
            "TaskManagementSystem.Web.Models.TaskStatus.$1");
        
        if (content != originalContent)
        {
            File.WriteAllText(path, content);
            Console.WriteLine($"  Fixed TaskStatus references in {Path.GetFileName(path)}");
        }
    }
}
