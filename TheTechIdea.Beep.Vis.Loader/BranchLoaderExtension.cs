﻿using Beep.Vis.Module;
using System.Diagnostics;
using System.Reflection;

using TheTechIdea.Tools;
using TheTechIdea.Util;


namespace AssemblyLoaderExtension
{
    public class BranchLoaderExtension : ILoaderExtention
    {
        public AppDomain CurrentDomain { get; set; }
      
    
        public IAssemblyHandler Loader { get; set; }
        public BranchLoaderExtension(IAssemblyHandler ploader)
        {
            Loader = ploader;
          
          //  DMEEditor = 
            CurrentDomain = AppDomain.CurrentDomain;
           // DataSourcesClasses = new List<AssemblyClassDefinition>();
            CurrentDomain.AssemblyResolve += Loader.CurrentDomain_AssemblyResolve;
        }

        public IErrorsInfo LoadAllAssembly()
        {
            ErrorsInfo er = new ErrorsInfo();
            foreach (var item in Loader.Assemblies)
            {
                try
                {
                    ScanAssembly(item.DllLib);
                }
                catch (Exception ex)
                {


                }

            }

            return er;
        }
        #region "Class Extractors"
        private bool ScanAssembly(Assembly asm)
        {
            Type[] t;

            try
            {
                try
                {
                    t = asm.GetTypes();
                }
                catch (Exception ex2)
                {
                    //DMEEditor.AddLogMessage("Failed", $"Could not get types for {asm.GetName().ToString()}", DateTime.Now, -1, asm.GetName().ToString(), Errors.Failed);
                    try
                    {
                        //DMEEditor.AddLogMessage("Try", $"Trying to get exported types for {asm.GetName().ToString()}", DateTime.Now, -1, asm.GetName().ToString(), Errors.Ok);
                        t = asm.GetExportedTypes();
                    }
                    catch (Exception ex3)
                    {
                        t = null;
                        //DMEEditor.AddLogMessage("Failed", $"Could not get types for {asm.GetName().ToString()}", DateTime.Now, -1, asm.GetName().ToString(), Errors.Failed);
                    }

                }

                if (t != null)
                {
                    foreach (var mytype in t) //asm.DefinedTypes
                    {

                        TypeInfo type = mytype.GetTypeInfo();
                        string[] p = asm.FullName.Split(new char[] { ',' });
                        p[1] = p[1].Substring(p[1].IndexOf("=") + 1);
                        //-------------------------------------------------------
                       
                     
                        //-------------------------------------------------------
                   
                        //-------------------------------------------------------
                        // Get IBranch Definitions
                        if (type.ImplementedInterfaces.Contains(typeof(IBranch)))
                        {

                            //AssemblyClassDefinition xcls = new AssemblyClassDefinition();
                            //xcls.Methods = new List<MethodsClass>();
                            //xcls.className = type.Name;
                            //xcls.dllname = type.Module.Name;
                            //xcls.PackageName = type.FullName;
                            //xcls.componentType = "IBranch";
                            //xcls.type = type;
                            ////AddinVisSchema
                            //xcls.VisSchema  = (AddinVisSchema)type.GetCustomAttribute(typeof(AddinVisSchema), false);
                            //xcls.classProperties = (AddinAttribute)type.GetCustomAttribute(typeof(AddinAttribute), false);
                            //if (xcls.classProperties != null)
                            //{
                            //    xcls.RootName = xcls.classProperties.FileType;
                            //}

                            ////   xcls.RootName = "AI";
                            ////   xcls.BranchType = brcls.BranchType;
                            //foreach (MethodInfo methods in type.GetMethods()
                            //             .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                            //              .ToArray())
                            //{

                            //    CommandAttribute methodAttribute = methods.GetCustomAttribute<CommandAttribute>();
                            //    MethodsClass x = new MethodsClass();
                            //    x.Caption = methodAttribute.Caption;
                            //    x.Info = methods;
                            //    x.Hidden = methodAttribute.Hidden;
                            //    x.Click = methodAttribute.Click;
                            //    x.DoubleClick = methodAttribute.DoubleClick;
                            //    x.iconimage = methodAttribute.iconimage;
                            //    xcls.Methods.Add(x);
                            //}
                            //if (type.ImplementedInterfaces.Contains(typeof(IOrder)))
                            //{
                            //    try
                            //    {
                            //        IOrder cls = (IOrder)Activator.CreateInstance(type);
                            //        xcls.Order = cls.Order;
                            //        cls = null;
                            //    }
                            //    catch (Exception)
                            //    {


                            //    }

                            //}
                            try
                            {
                                Loader.ConfigEditor.BranchesClasses.Add(Loader.GetAssemblyClassDefinition(type, "IBranch"));
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message+$" -- {type.Name}");
                            }
                           
                        }
                        // --- Get all AI app Interfaces
                        //-----------------------------------------------------
                       

                    }
                }

            }
            catch (Exception ex)
            {
                //DMEEditor.AddLogMessage("Failed", $"Could not get Any types for {asm.GetName().ToString()}" , DateTime.Now, -1, asm.GetName().ToString(), Errors.Failed);
            };

            return true;


        }
        #endregion "Class Extractors"
   
        public IErrorsInfo Scan(assemblies_rep assembly)
        {
            ErrorsInfo er = new ErrorsInfo();
            try
            {

                ScanAssembly(assembly.DllLib);
                er.Flag = Errors.Ok;
            }
            catch (Exception ex)
            {
                er.Ex = ex;
                er.Flag = Errors.Failed;
                er.Message = ex.Message;

            }
            return er;
        }
        public IErrorsInfo Scan(Assembly assembly)
        {
            ErrorsInfo er = new ErrorsInfo();
            try
            {

                ScanAssembly(assembly);
                er.Flag = Errors.Ok;
            }
            catch (Exception ex)
            {
                er.Ex = ex;
                er.Flag = Errors.Failed;
                er.Message = ex.Message;

            }
            return er;
        }
        public IErrorsInfo Scan()
        {
            ErrorsInfo er = new ErrorsInfo();
            try
            {

                LoadAllAssembly();
                er.Flag = Errors.Ok;
            }
            catch (Exception ex)
            {
                er.Ex = ex;
                er.Flag = Errors.Failed;
                er.Message = ex.Message;

            }
            return er;
        }
    }
}