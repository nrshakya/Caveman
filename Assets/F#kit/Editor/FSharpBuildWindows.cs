﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class FSharpBuildWindows {
    // download links found at: monodevelop.com/download/
    // .net is required for gtksharp
    // gtksharp is required for xamarin studio
    // f# bundle 3.1.2 is required to build f# libraries without errors.
    //   (f# v4.0 doesn't seem to work with xamarin)
    public static string xamarinPath = "C:/Program Files (x86)/Xamarin Studio/bin/mdtool.exe";
    public static string xamarinUrl = "http://download.xamarin.com/studio/Windows/XamarinStudio-5.10.1.6-0.msi";
    
    public static string dotnetPath = "C:/Windows/Microsoft.NET/Framework/v4.0.30319/";
    public static string dotnetUrl = "http://go.microsoft.com/fwlink/p/?LinkId=397703";
    
    public static string gtksharpPath = "C:/Program Files (x86)/GtkSharp/2.12/";
    public static string gtksharpUrl = "http://download.xamarin.com/GTKforWindows/Windows/gtk-sharp-2.12.30.msi";
    
    public static string fsharpbundlePath = "C:/Program Files (x86)/Microsoft SDKs/F#/3.1/";
    public static string fsharpbundleUrl = "https://www.microsoft.com/en-us/download/details.aspx?id=44011";

    public static string unityenginedllPath = ApplicationFolder() + "/Data/Managed/UnityEngine.dll";

    // EditorApplication.applicationPath points to the .exe, but we want the
    // folder.
    static string ApplicationFolder() {
        var path = EditorApplication.applicationPath.Split('/');        
        path = path.Take(path.Length - 1).ToArray(); // remove last .exe entry
        return String.Join("/", path);
    }

    static void CreateXamarinProject(string[] scripts) {
        // note: FSharpProject.userprefs not necessary
        // note: Script.fsx is not necessary

        // delete if still exists from previous build
        if (Directory.Exists("Assets/F#kit/FSharpProject"))
            Directory.Delete("Assets/F#kit/FSharpProject", true);

        // FSharpProject folder
        Directory.CreateDirectory("Assets/F#kit/FSharpProject");

        // AssemblyInfo.fs
        var content = @"namespace FSharpProject
open System.Reflection
open System.Runtime.CompilerServices

[<assembly: AssemblyTitle(""FSharpProject"")>]
[<assembly: AssemblyDescription("""")>]
[<assembly: AssemblyConfiguration("""")>]
[<assembly: AssemblyCompany("""")>]
[<assembly: AssemblyProduct("""")>]
[<assembly: AssemblyCopyright("""")>]
[<assembly: AssemblyTrademark("""")>]

// The assembly version has the format {Major}.{Minor}.{Build}.{Revision}

[<assembly: AssemblyVersion(""1.0.0.0"")>]

//[<assembly: AssemblyDelaySign(false)>]
//[<assembly: AssemblyKeyFile("""")>]

()

";
        File.WriteAllText("Assets/F#kit/FSharpProject/AssemblyInfo.fs", content);

        // FSharpProject.fsproj
        content = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project DefaultTargets=""Build"" ToolsVersion=""4.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProjectGuid>{CFE42277-49BD-44D7-BF80-DCFF1FCC0C4A}</ProjectGuid>
    <OutputType>Library</OutputType>
    <RootNamespace>FSharpProject</RootNamespace>
    <AssemblyName>FSharpProject</AssemblyName>
    <TargetFrameworkVersion>v3.5</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <Optimize>false</Optimize>
    <OutputPath>../F#Out</OutputPath>
    <DefineConstants>DEBUG</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <ConsolePause>false</ConsolePause>
    <PlatformTarget>
    </PlatformTarget>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugSymbols>false</DebugSymbols>
    <Optimize>true</Optimize>
    <OutputPath>../F#Out</OutputPath>
    <DefineConstants>
    </DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <ConsolePause>false</ConsolePause>
    <GenerateTailCalls>true</GenerateTailCalls>
    <PlatformTarget>
    </PlatformTarget>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""mscorlib"" />
    <Reference Include=""System"" />
    <Reference Include=""System.Core"" />
    <Reference Include=""FSharp.Core"">
      <HintPath>../F#DLL/FSharp.Core.dll</HintPath>
      <Private>False</Private>
    </Reference>
    <Reference Include=""UnityEngine"">
      <HintPath>";
        content += unityenginedllPath;
        content += @"</HintPath>
      <Private>False</Private>
    </Reference>
  </ItemGroup>
  <Import Project=""$(MSBuildExtensionsPath32)/../Microsoft SDKs/F#/3.1/Framework/v4.0/Microsoft.FSharp.Targets"" />
  <ItemGroup>";

        // add each script
        foreach (var script in scripts)
            content += @"<Compile Include=""../../../" + script + @""" />";

        content += @"</ItemGroup></Project>";
        File.WriteAllText("Assets/F#kit/FSharpProject/FSharpProject.fsproj", content);

        // FSharpProject.sln
        content = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{f2a71f9b-5d33-465a-a702-920d77279786}"") = ""FSharpProject"", ""FSharpProject.fsproj"", ""{CFE42277-49BD-44D7-BF80-DCFF1FCC0C4A}""
EndProject
Global
    GlobalSection(SolutionConfigurationPlatforms) = preSolution
        Debug|Any CPU = Debug|Any CPU
        Release|Any CPU = Release|Any CPU
    EndGlobalSection
    GlobalSection(ProjectConfigurationPlatforms) = postSolution
        {CFE42277-49BD-44D7-BF80-DCFF1FCC0C4A}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        {CFE42277-49BD-44D7-BF80-DCFF1FCC0C4A}.Debug|Any CPU.Build.0 = Debug|Any CPU
        {CFE42277-49BD-44D7-BF80-DCFF1FCC0C4A}.Release|Any CPU.ActiveCfg = Release|Any CPU
        {CFE42277-49BD-44D7-BF80-DCFF1FCC0C4A}.Release|Any CPU.Build.0 = Release|Any CPU
    EndGlobalSection
EndGlobal
";
        File.WriteAllText("Assets/F#kit/FSharpProject/FSharpProject.sln", content);
    }

    static void Cleanup() {
        // delete FSharpProject folder
        if (Directory.Exists("Assets/F#kit/FSharpProject"))
            Directory.Delete("Assets/F#kit/FSharpProject", true);

    }

    public static void Build() {
        if (FSharpKit.showDebug) Debug.Log("building for windows");

        // find all .fs scripts in the project's Assets folder
        var scripts = Directory.GetFiles("Assets", "*.fs", SearchOption.AllDirectories);

        // create xamarin project and include all scripts
        CreateXamarinProject(scripts);
        
        // the command to build with xamarin:
        // /Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool build -p:"test" -t:Build "test.sln"
        //System.Diagnostics.Process.Start("echo 3");
        var proc = new System.Diagnostics.Process {
            StartInfo = new System.Diagnostics.ProcessStartInfo {
                FileName = xamarinPath,
                Arguments = "build -p:\"FSharpProject\" -t:Build \"FSharpProject.sln\"",
                WorkingDirectory = "Assets/F#kit/FSharpProject",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };
        proc.Start();

        // show output
        var log = proc.StandardOutput.ReadToEnd();
        if (log != "") if (FSharpKit.showDebug) Debug.Log(log);
        var err = proc.StandardError.ReadToEnd();
        if (err != "") Debug.LogError(err);

        Cleanup();

        // refresh assets to reimport new scripts
        AssetDatabase.Refresh();
        
        if (FSharpKit.showDebug) Debug.Log("finished build!");        
    }
}
