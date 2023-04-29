using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;

public class MenuItems
{
    private static readonly string GitBashPath = @"C:\Program Files\Git\git-bash.exe";

    [MenuItem("Scripts/Open Git Bash")]
	public static void OpenGitBash()
	{
        FileInfo gitBash = new(GitBashPath);
        Process.Start(gitBash.FullName, $"--cd=\"{Environment.CurrentDirectory}\"");
	}

    [MenuItem("Scripts/Open Git Bash", isValidateFunction: true)]
    public static bool HasGitBash()
    {
        FileInfo gitBash = new(GitBashPath);
        return gitBash.Exists;
    }
}