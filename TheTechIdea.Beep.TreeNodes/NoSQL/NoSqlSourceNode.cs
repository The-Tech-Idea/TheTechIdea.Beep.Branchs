﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.Utilities;

namespace TheTechIdea.Beep.TreeNodes.NoSQL
{
    [AddinAttribute(Caption = "NoSQL", BranchType = EnumPointType.DataPoint, Name = "NoSQL.Beep", misc = "Beep", iconimage = "nosql.png", menu = "Beep", ObjectType = "Beep")]
    
    public class NoSqlSourceNode  : IBranch 
    {
        public NoSqlSourceNode()
        {

        }
        public NoSqlSourceNode(ITree pTreeEditor, IDMEEditor pDMEEditor, IBranch pParentNode, string pBranchText, int pID, EnumPointType pBranchType, string pimagename)
        {



            TreeEditor = pTreeEditor;
            DMEEditor = pDMEEditor;
            ParentBranchID = pParentNode!=null? pParentNode.ID : -1;
            BranchText = pBranchText;
            BranchType = pBranchType;
            DataSourceName = pBranchText;
            IconImageName = pimagename;

            if (pID != 0)

            {
                ID = pID;
                BranchID = pID;
            }
        }
        public bool Visible { get; set; } = true;
        public string MenuID { get; set; }
        public bool IsDataSourceNode { get; set; } = true;
        public string GuidID { get; set; } = Guid.NewGuid().ToString();
        public string ParentGuidID { get; set; }
        public string DataSourceConnectionGuidID { get; set; }
        public string EntityGuidID { get; set; }
        public string MiscStringID { get; set; }
        #region "Properties"
         public IBranch ParentBranch { get  ; set  ; }
        public string ObjectType { get; set; } = "Beep";
        public int ID { get; set; }
        public EntityStructure EntityStructure { get; set; }
        public string Name { get; set; }
        public string BranchText { get; set; } = "NoSqlSource";
        public IDMEEditor DMEEditor { get; set; }
        public IDataSource DataSource { get; set; }
        public string DataSourceName { get; set; }
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.DataPoint;
        public int BranchID { get; set; }
        public string IconImageName { get; set; } = "nosql.png";
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; }
        public string BranchClass { get; set; } = "NOSQL";
        public List<IBranch> ChildBranchs { get; set; } = new List<IBranch>();
        public ITree TreeEditor { get; set; }
        public List<string> BranchActions { get; set; }
        public object TreeStrucure { get; set; }
        public  IAppManager  Visutil { get; set; }
        public int MiscID { get; set; }


       // public event EventHandler<PassedArgs> BranchSelected;
       // public event EventHandler<PassedArgs> BranchDragEnter;
       // public event EventHandler<PassedArgs> BranchDragDrop;
       // public event EventHandler<PassedArgs> BranchDragLeave;
       // public event EventHandler<PassedArgs> BranchDragClick;
       // public event EventHandler<PassedArgs> BranchDragDoubleClick;
       // public event EventHandler<PassedArgs> ActionNeeded;
        #endregion "Properties"
        #region "Interface Methods"
        public IErrorsInfo CreateChildNodes()
        {

            try
            {
                CreateNodes();

                DMEEditor.AddLogMessage("Success", "Added Child Nodes", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                string mes = "Could not Child Nodes";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;

        }

        public IErrorsInfo ExecuteBranchAction(string ActionName)
        {
            throw new NotImplementedException();
        }

        public IErrorsInfo MenuItemClicked(string ActionNam)
        {
            throw new NotImplementedException();
        }

        public IErrorsInfo RemoveChildNodes()
        {
            throw new NotImplementedException();
        }

        public IErrorsInfo SetConfig(ITree pTreeEditor, IDMEEditor pDMEEditor, IBranch pParentNode, string pBranchText, int pID, EnumPointType pBranchType, string pimagename)
        {
            try
            {
                TreeEditor = pTreeEditor;
                DMEEditor = pDMEEditor;
                ParentBranchID = pParentNode != null ? pParentNode.ID : -1;
                BranchText = pBranchText;
                BranchType = pBranchType;
                IconImageName = pimagename;
                if (pID != 0)
                {
                    ID = pID;
                }

                //   DMEEditor.AddLogMessage("Success", "Set Config OK", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                string mes = "Could not Set Config";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
        }
        #endregion "Interface Methods"
        #region "Exposed Interface"
        [CommandAttribute(Caption = "Get Entities", iconimage = "getchilds.png", ObjectType = "Beep")]
        public IErrorsInfo CreateDatabaseEntites()
        {
            DMEEditor.ErrorObject.Flag = Errors.Ok;
            DMEEditor.Logger.WriteLog($"Filling Database Entites ) ");
            try
            {
                DataSource = (IDataSource)DMEEditor.GetDataSource(BranchText);
                if (DataSource != null)
                {
                    DataSource.Openconnection();
                   
                    if (DataSource.ConnectionStatus == System.Data.ConnectionState.Open)
                    {
                        DataSource.GetEntitesList();
                        TreeEditor.Treebranchhandler.RemoveChildBranchs(this);
                        int i = 0;
                        foreach (string tb in DataSource.EntitiesNames)
                        {

                            NoSqlEntityNode dbent = new NoSqlEntityNode(TreeEditor, DMEEditor, this, tb, TreeEditor.SeqID);
                            dbent.DataSource = DataSource;
                            dbent.GuidID=DataSource.GuidID;
                            dbent.DataSourceName = DataSource.DatasourceName;
                            TreeEditor.Treebranchhandler.AddBranch(this, dbent);
                            //  dbent.CreateChildNodes();
                          
                            i += 1;


                        }
                    }

                }



            }
            catch (Exception ex)
            {
                DMEEditor.Logger.WriteLog($"Error in Filling Database Entites ({ex.Message}) ");
                DMEEditor.ErrorObject.Flag = Errors.Failed;
                DMEEditor.ErrorObject.Ex = ex;
            }
            return DMEEditor.ErrorObject;

        }
        [CommandAttribute(Caption = "Edit NOSQL Connection", Name = "EditNOSQLConnection", iconimage = "dbconnection.png")]
        public IErrorsInfo EditNOSQLConnection()
        {
            DMEEditor.ErrorObject.Flag = Errors.Ok;
            //   DMEEditor.Logger.WriteLog($"Filling Database Entites ) ");
            try
            {
                string[] args = { "EDITNOSQL ", null, null };
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
                    ObjectType = "EDITSQL",
                    DataSource = null,
                    ObjectName = BranchText,
                    ParameterString1 = DataSourceConnectionGuidID,
                    Objects = ob,
                    DatasourceName = BranchText,
                    EventType = "EDITNOSQL"

                };
                // ActionNeeded?.Invoke(this, Passedarguments);
                Visutil.ShowPage("uc_NoSql", Passedarguments);



            }
            catch (Exception ex)
            {
                DMEEditor.Logger.WriteLog($"Error in Filling Database Entites ({ex.Message}) ");
                DMEEditor.ErrorObject.Flag = Errors.Failed;
                DMEEditor.ErrorObject.Ex = ex;
            }
            return DMEEditor.ErrorObject;

        }
        #endregion Exposed Interface"
        #region "Other Methods"
        public IErrorsInfo CreateNodes()
        {

            try
            {


                DMEEditor.AddLogMessage("Success", "Created child Nodes", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                string mes = "Could not Create child Nodes";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;

        }

        public  IBranch  CreateCategoryNode(CategoryFolder p)
        {
            throw new NotImplementedException();
        }
        #endregion"Other Methods"

    }
}
