/*
 * Portable store by Tom OLIVIER
 * https://github.com/Tom60chat/Portable-store
 * 
 * Copyright (c) 2022, Tom OLIVIER
 * All rights reserved.
 */

using Portable_store.Console;
using Portable_store.Models;
using System.Reflection;

var options = new List<string>();
string command = "";
var names = new List<string>();

foreach (var arg in args)
{
    if (arg.StartsWith('-'))
        options.Add(arg);
    else if (string.IsNullOrEmpty(command))
        command = arg;
    else
        names.Add(arg);
}

// Set options
Application_options.Parse_options(options);


// Apply options
if (Application_options.Version)
{
    Options.ShowVersion();
    return;
}

// Apply commandes
var progress = new Progress<Progress_info_Model>(p =>
{
    if (Application_options.Verbose)
        ConsoleHelper.WriteLine(p.Details);
});

await (command switch
{
    "download" => Commands.Download(names, progress),
    "delete" => Commands.Delete(names, progress),
    "search" => Commands.Search(names, progress),
    "list" => Commands.List(names, progress),
    //"update" => Commands.Update(names, progress),
    "refresh" => Commands.Refresh(names, progress),
    "run" => Commands.Run(names, progress),
    "create" => Commands.Create(names, progress),
    "read" => Commands.Read(names, progress),
    _ => Task.Run(() => // I like cheating
    {
        if (Application_options.Help)
        {
            Options.ShowHelp();
            return;
        }

        if (!string.IsNullOrEmpty(command))
            ConsoleHelper.WriteLine("Unknown command" + Environment.NewLine);
        else
            ConsoleHelper.WriteLine($"No command given, do {Assembly.GetExecutingAssembly().GetName().Name} --help for more information");
    })
});

