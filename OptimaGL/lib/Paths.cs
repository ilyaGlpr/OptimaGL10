﻿using System;
using System.IO;

namespace OptimaGL.lib
{
    class Paths
    {
        public static string SysDir = Path.GetPathRoot(Environment.SystemDirectory);
        public static string ShellCmdLocation = 
            (File.Exists(SysDir + @"Windows\Sysnative\cmd.exe")) ? 
                SysDir + @"Windows\Sysnative\cmd.exe" : SysDir + @"Windows\System32\cmd.exe";
        public static string System32Location = 
            (File.Exists(SysDir + @"Windows\Sysnative\cmd.exe")) ? 
                SysDir + @"Windows\Sysnative\" : SysDir + @"Windows\System32\";


    }
}
