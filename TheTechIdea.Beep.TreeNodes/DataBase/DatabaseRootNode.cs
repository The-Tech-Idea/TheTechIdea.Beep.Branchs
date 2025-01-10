﻿using System;
using System.Collections.Generic;
using System.Linq;
using TheTechIdea;
using TheTechIdea.Beep;
using  Beep.Vis.Module;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Util;
using TheTechIdea.Beep.Addin;
namespace  BeepEnterprize.Vis.Module
{
    [AddinAttribute(Caption = "RDBMS",BranchType = EnumPointType.Root, Name = "DatabaseRootNode.Beep", misc = "Beep", iconimage = "database.png", menu = "DataSource", ObjectType = "Beep", Category = DatasourceCategory.RDBMS)]
    [AddinVisSchema(BranchType = EnumPointType.Root, BranchClass = "DATASOURCEROOT", RootNodeName = "DataSourcesRootNode")]
    public class DatabaseRootNode : IBranch  
    {

        public DatabaseRootNode()
        {
            IsDataSourceNode = true;
        }
        public DatabaseRootNode(ITree pTreeEditor, IDMEEditor pDMEEditor, IBranch pParentNode, string pBranchText, int pID, EnumPointType pBranchType, string pimagename)
        {
            TreeEditor = pTreeEditor;
            DMEEditor = pDMEEditor;
            ParentBranchID = pParentNode.ID;
            BranchText = pBranchText;
            BranchType = pBranchType;
            IconImageName = pimagename;
            if (pID != 0)
            {
                ID = pID;
                BranchID = ID;
            }
        }
        public IErrorsInfo SetConfig(ITree pTreeEditor, IDMEEditor pDMEEditor, IBranch pParentNode, string pBranchText, int pID, EnumPointType pBranchType, string pimagename)
        {

            try
            {
                TreeEditor = pTreeEditor;
                DMEEditor = pDMEEditor;
                //ParentBranchID = pParentNode.ID;
                //BranchText = pBranchText;
                //BranchType = pBranchType;
                //IconImageName = pimagename;
                //if (pID != 0)
                //{
                //    ID = pID;
                //    BranchID = ID;
                //}

             //   DMEEditor.AddLogMessage("Success", "Set Config OK", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                string mes = "Could not Set Config";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;

        }
        public bool Visible { get; set; } = true;
        public bool IsDataSourceNode { get; set; } = true;
        public string GuidID { get; set; } = Guid.NewGuid().ToString();
        public string ParentGuidID { get; set; }
        public string DataSourceConnectionGuidID { get; set; }
        public string EntityGuidID { get; set; }
        public string MiscStringID { get; set; }
         public IBranch ParentBranch { get  ; set  ; }
        public string Name { get; set; }
        public EntityStructure EntityStructure { get; set; }
        public string BranchText { get; set; } = "RDBMS";
        public IDMEEditor DMEEditor { get ; set ; }
        public IDataSource DataSource { get ; set ; }
        public string DataSourceName { get; set; }
        public int Level { get; set; } = 0;
        public  EnumPointType BranchType { get; set; } = EnumPointType.Root;
        public int BranchID { get ; set ; }
        public string IconImageName { get ; set ; }= "database.png";
        public string BranchStatus { get ; set ; }
        public int ParentBranchID { get ; set ; }
        public string BranchDescription { get ; set ; }
      
        public string BranchClass { get; set; } = "RDBMS";
        public List<IBranch> ChildBranchs { get ; set ; } = new List<IBranch>();
        public ITree TreeEditor { get ; set ; }
        public List<string> BranchActions { get ; set ; }
        public List<Delegate> Delegates { get ; set ; }
        public int ID { get ; set ; }
        public int Order { get; set; } = 3;
        public int MiscID { get; set; }
        public  IVisManager  Visutil { get; set; }
        PassedArgs Passedarguments { get; set; } = new PassedArgs();
        public object TreeStrucure { get ; set ; }
        public string ObjectType { get; set; } = "Beep";
        // public event EventHandler<PassedArgs> BranchSelected;
        // public event EventHandler<PassedArgs> BranchDragEnter;
        // public event EventHandler<PassedArgs> BranchDragDrop;
        // public event EventHandler<PassedArgs> BranchDragLeave;
        // public event EventHandler<PassedArgs> BranchDragClick;
        // public event EventHandler<PassedArgs> BranchDragDoubleClick;
        // public event EventHandler<PassedArgs> ActionNeeded;
        // public event EventHandler<PassedArgs> OnObjectSelected;


        public IErrorsInfo CreateChildNodes()
        {

            try
            {
               // TreeEditor.treeBranchHandler.RemoveChildBranchs(this);
                foreach (ConnectionProperties i in DMEEditor.ConfigEditor.DataConnections.Where(c => c.Category == DatasourceCategory.RDBMS && c.IsComposite==false))
                {
                    if (TreeEditor.treeBranchHandler.CheckifBranchExistinCategory(i.ConnectionName, "RDBMS")==null)
                    {
                       if(!ChildBranchs.Any(p => p.DataSourceConnectionGuidID.Equals(i.GuidID, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            CreateDBNode(i);
                            i.Drawn = true;
                        }
                       
                    }


                }
                foreach (CategoryFolder i in DMEEditor.ConfigEditor.CategoryFolders.Where(x => x.RootName.Equals("RDBMS", StringComparison.InvariantCultureIgnoreCase)))
                {
                    if (!ChildBranchs.Where(p => p.BranchText == i.FolderName && i.RootName == "RDBMS").Any())
                    {
                        CreateCategoryNode(i);
                    }

                


                }

            //    DMEEditor.AddLogMessage("Success", "Added Database Connection", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                string mes = "Could not Add Database Connection";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
            
        }

        public IErrorsInfo CreateDBNode(ConnectionProperties i)
        {
            try
            {
                DatabaseNode database = new DatabaseNode(i,TreeEditor, DMEEditor, this, i.ConnectionName, TreeEditor.SeqID, EnumPointType.DataPoint, i.ConnectionName);
                database.DataSource = DataSource;
                database.DataSourceName = i.ConnectionName;
                database.DataSourceConnectionGuidID = i.GuidID;
                TreeEditor.treeBranchHandler.AddBranch(this,database);
             
                ChildBranchs.Add(database);
               
             //   DMEEditor.AddLogMessage("Success", "Added Database Connection", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                string mes = "Could not Add Database Connection";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
           
            return DMEEditor.ErrorObject;
        }
        public  IBranch  CreateCategoryNode(CategoryFolder p)
        {
            DatabaseCategoryNode Category = null;
            try
            {
                 Category = new DatabaseCategoryNode(TreeEditor, DMEEditor, this,p.FolderName, TreeEditor.SeqID, EnumPointType.Category,TreeEditor.CategoryIcon);
                TreeEditor.treeBranchHandler.AddBranch(this, Category);
                Category.CreateChildNodes();
               
            }
            catch (Exception ex)
            {
                DMEEditor.Logger.WriteLog($"Error Creating Category Node File Node ({ex.Message}) ");
                DMEEditor.ErrorObject.Flag = Errors.Failed;
                DMEEditor.ErrorObject.Ex = ex;
            }
            return Category;

        }
    
        public IErrorsInfo ExecuteBranchAction(string ActionName)
        {
            throw new NotImplementedException();
        }

        public IErrorsInfo MenuItemClicked(string ActionNam)
        {
            throw new NotImplementedException();
        }

        public IErrorsInfo OnBranchSelected(PassedArgs ActionDef)
        {
            throw new NotImplementedException();
        }

        public IErrorsInfo RemoveChildNodes()
        {
            throw new NotImplementedException();
        }
       
        [CommandAttribute(Caption = "Create Local Database",iconimage ="localdb.png",PointType = EnumPointType.Root, ObjectType = "Beep")]
        public IErrorsInfo CreateNewLocalDatabase()
        {

            try
            {
                string[] args = { "New Query Entity", null, null };
                List<ObjectItem> ob = new List<ObjectItem>(); ;
                ObjectItem it = new ObjectItem();
                it.obj = this;
                it.Name = "Branch";
                ob.Add(it);
               
                PassedArgs Passedarguments = new PassedArgs
                {
                    Addin = null,
                    AddinName = null,
                    AddinType = "",
                    DMView = null,
                    CurrentEntity = null,

                    ObjectType = "LOCALDB",
                    DataSource = null,
                    ObjectName = null,

                    Objects = ob,

                    DatasourceName = null,
                    EventType = "NEW"

                };
                Visutil.ShowPage("uc_CreateLocalDatabase",  Passedarguments);
              //  Visutil.ShowFormFromAddin(Visutil.LLoader.AddIns.Where(x => x.ObjectName == "uc_CreateLocalDatabase").FirstOrDefault().DllPath, DMEEditor.ConfigEditor.Config.DSEntryFormName, DMEEditor, args, null);

                DMEEditor.AddLogMessage("Success", "create Local Database", DateTime.Now, 0, null, Errors.Failed);
            }
            catch (Exception ex)
            {
                string mes = "Could not create Local Database";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
        }
        [CommandAttribute(Caption = "New DB Connection", Name = "NewDBConnection", iconimage = "dbconnection.png")]
        public IErrorsInfo NewDBConnection()
        {
            DMEEditor.ErrorObject.Flag = Errors.Ok;
            //   DMEEditor.Logger.WriteLog($"Filling Database Entites ) ");
            try
            {
                string[] args = { "New Web API ", null, null };
                List<ObjectItem> ob = new List<ObjectItem>(); ;
                ObjectItem it = new ObjectItem();
                it.obj = this;
                it.Name = "Branch";
                ob.Add(it);


                PassedArgs Passedarguments = new PassedArgs
                {
                    Addin = null,
                    AddinName = null,
                    AddinType = "",
                    DMView = null,
                    CurrentEntity = BranchText,
                    Id = 0,
                    ObjectType = "DB",
                    DataSource = null,
                    ObjectName = BranchText,
                    ParameterString1 = null,
                    Objects = ob,

                    DatasourceName = BranchText,
                    EventType = "NEWDB"

                };
                // ActionNeeded?.Invoke(this, Passedarguments);
                Visutil.ShowPage("uc_Database", Passedarguments);



            }
            catch (Exception ex)
            {
                DMEEditor.Logger.WriteLog($"Error in Filling Database Entites ({ex.Message}) ");
                DMEEditor.ErrorObject.Flag = Errors.Failed;
                DMEEditor.ErrorObject.Ex = ex;
            }
            return DMEEditor.ErrorObject;

        }


    }
}