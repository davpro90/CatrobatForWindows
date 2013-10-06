﻿using System.Reflection;

namespace Catrobat.TestsWindowsPhone.Misc
{
  public static class BasePathHelper
  {
    public static string GetSampleDataPath()
    {
      string path = "";// GetTestBasePathWithBranch();
      path += "SampleData/"; // "TestsCommon/SampleData/";

      return path;
    }

    public static string GetSampleProjectsPath()
    {
      string path = GetSampleDataPath();
      path += "SampleProjects/";

      return path;
    }
  }
}