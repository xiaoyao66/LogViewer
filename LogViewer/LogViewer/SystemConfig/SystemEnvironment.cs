using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogViewer.SystemConfig
{
    using Microsoft.Win32;
    using System.Runtime.InteropServices;
    using System.Security.Principal;
    using System.Windows.Forms;

    internal class SysEnvironment
    {
        /// <summary>
        /// 获取系统环境变量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSysEnvironmentByName(string name)
        {
            string result = string.Empty;
            try
            {
                result = OpenSysEnvironment().GetValue(name).ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return result;
        }


        /// <summary>
        /// 打开系统环境变量注册表
        /// </summary>
        /// <returns>RegistryKey</returns>
        private static RegistryKey OpenSysEnvironment()
        {
            RegistryKey regLocalMachine = Registry.LocalMachine;//HKEY_LOCAL_MACHINE下的SYSTEM 
            RegistryKey regSYSTEM = regLocalMachine.OpenSubKey("SYSTEM", true);
            RegistryKey regControlSet001 = regSYSTEM?.OpenSubKey("ControlSet001", true);
            RegistryKey regControl = regControlSet001?.OpenSubKey("Control", true);
            RegistryKey regManager = regControl?.OpenSubKey("Session Manager", true);

            RegistryKey regEnvironment = regManager?.OpenSubKey("Environment", true);
            return regEnvironment;
        }


        /// <summary>
        /// 设置系统环境变量
        /// </summary>
        /// <param name="name">变量名</param>
        /// <param name="strValue">值</param>
        public static void SetSysEnvironment(string name, string strValue) => OpenSysEnvironment().SetValue(name, strValue);


        /// <summary>
        /// 检测系统环境变量是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckSysEnvironmentExist(string name) => !string.IsNullOrEmpty(GetSysEnvironmentByName(name));



        /// <summary>
        /// 添加到PATH环境变量（会检测路径是否存在，存在就不重复）
        /// </summary>
        /// <param name="strPath"></param>
        public static void SetPathAfter(string strHome)
        {
            string pathlist;
            pathlist = GetSysEnvironmentByName("PATH");
            //检测是否以;结尾
            if (pathlist?.Substring(pathlist.Length - 1, 1) != ";")
            {
                SetSysEnvironment("PATH", pathlist + ";");
                pathlist = GetSysEnvironmentByName("PATH");
            }
            string[] list = pathlist?.Split(';');
            bool isPathExist = false;


            foreach (string item in list)
            {
                if (string.Equals(item, strHome, StringComparison.OrdinalIgnoreCase))
                    isPathExist = true;
            }
            if (!isPathExist)
            {
                SetSysEnvironment("PATH", pathlist + strHome + ";");
            }
        }


        public static void SetPathBefore(string strHome)
        {
            string pathlist;
            pathlist = GetSysEnvironmentByName("PATH");
            string[] list = pathlist?.Split(';');
            bool isPathExist = false;


            foreach (string item in list)
            {
                if (string.Equals(item, strHome, StringComparison.OrdinalIgnoreCase))
                    isPathExist = true;
            }
            if (!isPathExist)
            {
                SetSysEnvironment("PATH", strHome + ";" + pathlist);
            }
        }


        public static void SetPath(string strHome)
        {
            string pathlist;
            pathlist = GetSysEnvironmentByName("PATH");
            //检测是否以;结尾
            if (pathlist?.Substring(pathlist.Length - 1, 1) != ";")
            {
                SetSysEnvironment("PATH", pathlist + ";");
                pathlist = GetSysEnvironmentByName("PATH");
            }
            string[] list = pathlist?.Split(';');
            bool isPathExist = false;

            foreach (string item in list)
            {
                if (string.Equals(item, strHome, StringComparison.OrdinalIgnoreCase))
                    isPathExist = true;
            }
            if (!isPathExist)
            {
                SetSysEnvironment("PATH", pathlist + strHome + ";");
            }
        }

        public static void RemovePath(string strHome)
        {
            string pathlist;
            pathlist = GetSysEnvironmentByName("PATH");
            string[] list = pathlist.Split(';');
            bool isPathExist = false;

            string newEnv = "";
            foreach (string item in list)
            {
                if (string.Equals(item, strHome, StringComparison.OrdinalIgnoreCase))
                    isPathExist = true;
                else
                    newEnv += item + ";";
            }
            if (isPathExist)
            {
                SetSysEnvironment("PATH", newEnv);
            }
        }

        public static bool CheckPath(string strHome)
        {
            string pathlist;
            pathlist = GetSysEnvironmentByName("PATH");
            string[] list = pathlist?.Split(';');
            foreach (string item in list)
            {
                if (string.Equals(item, strHome, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }


    /// <summary>
    /// Kernel32.DLL内有SetEnvironmentVariable函数用于设置系统环境变量 
    /// </summary>
    internal class SetSysEnvironmentVariable
    {
        [DllImport("Kernel32.DLL ", SetLastError = true)]
        public static extern bool SetEnvironmentVariable(string lpName, string lpValue);

        public static void SetPath(string pathValue)
        {
            string pathlist;
            pathlist = SysEnvironment.GetSysEnvironmentByName("PATH");
            string[] list = pathlist.Split(';');
            bool isPathExist = false;

            foreach (string item in list)
            {
                if (item == pathValue)
                    isPathExist = true;
            }
            if (!isPathExist)
            {
                if (!pathlist.EndsWith(";"))
                    pathlist += ";";
                SetEnvironmentVariable("PATH", pathlist + pathValue + ";");
            }
        }
    }


    internal class SetDefaultFileOpen
    {
        public static bool IsAdministrator()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
            //WindowsBuiltInRole可以枚举出很多权限，例如系统用户、User、Guest等等
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// 设置文件默认打开程序 前提是程序支持参数启动打开文件
        /// 特殊说明:txt后缀比较特殊,还需要从注册表修改userchoie的键值才行
        /// </summary>
        /// <param name="fileExtension">文件拓展名 示例:'.slnc'</param>
        /// <param name="appPath">默认程序绝对路径 示例:'c:\\test.exe'</param>
        /// <param name="fileIconPath">文件默认图标绝对路径 示例:'c:\\test.ico'</param>
        static internal void SetFileOpenApp(string fileExtension, string appPath, string fileIconPath)
        {
            try
            {
                //slnc示例 注册表中tree node path
                //|-.slnc				默认		"slncfile"
                //|--slncfile
                //|---DefaultIcon		默认		"fileIconPath"			默认图标
                //|----shell
                //|-----open
                //|------command		默认		"fileExtension \"%1\""	默认打开程序路径
                makeRegFileOpenApp1(fileExtension);
                makeRegFileOpenApp(Registry.ClassesRoot, fileExtension, appPath, fileIconPath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        internal static bool CheckFileOpenApp(string fileExtension, string appPath)
        {
            return CheckRegFileOpenApp(Registry.ClassesRoot, fileExtension, appPath);
        }

        static void makeRegFileOpenApp1(string fileExtension)
        {
            var key = Registry.ClassesRoot.CreateSubKey(fileExtension);
            if (key == null) return;
            using (key)
            {
                var fileKeyName = $"{fileExtension.Substring(1)}file";
                key.SetValue(null, fileKeyName, RegistryValueKind.String);
                key.SetValue("Content Type", "text/plain", RegistryValueKind.String);
                key.SetValue("PerceivedType", "text", RegistryValueKind.String);

                var keylist = key.CreateSubKey("OpenWithProgids");
                using (keylist)
                {
                    keylist?.SetValue(fileKeyName, "", RegistryValueKind.String);
                }
            }
        }

        static void makeRegFileOpenApp(RegistryKey fileExtensionKey, string fileExtension, string appPath, string fileIconPath)
        {
            using (fileExtensionKey)
            {
                var fileKeyName = $"{fileExtension.Substring(1)}file";
                using (var fileKey = fileExtensionKey?.CreateSubKey(fileKeyName))
                {
                    fileKey.SetValue(null, fileKeyName, RegistryValueKind.String);

                    using (var defaultIcon = fileKey?.CreateSubKey("DefaultIcon"))
                    {
                        defaultIcon.SetValue(null, fileIconPath, RegistryValueKind.String);
                    }
                    using (var shell = fileKey?.CreateSubKey("shell"))
                    {
                        using (var open = shell?.CreateSubKey("open"))
                        {
                            using (var command = open?.CreateSubKey("command"))
                            {
                                command?.SetValue(null, $"{appPath} \"%1\"", RegistryValueKind.String);
                            }
                        }
                    }
                }
            }
        }
        internal static bool CheckRegFileOpenApp(RegistryKey fileExtensionKey, string fileExtension, string appPath)
        {
            bool result = false;
            try
            {
                using (fileExtensionKey)
                {
                    var fileKeyName = $"{fileExtension.Substring(1)}file";
                    using (var fileKey = fileExtensionKey?.OpenSubKey(fileKeyName))
                    {
                        using (var shell = fileKey?.OpenSubKey("shell"))
                        {
                            using (var open = shell?.OpenSubKey("open"))
                            {
                                using (var command = open?.OpenSubKey("command"))
                                {
                                    string getPath = command?.GetValue(null) as string;
                                    if (!string.IsNullOrEmpty(getPath) && getPath.Length > appPath.Length)
                                    {
                                        if (getPath[0] == '"') getPath = getPath.Substring(1);

                                        if (getPath.ToLower().StartsWith(appPath.ToLower()))
                                        {
                                            result = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
            }
            return result;
        }
    }
}
