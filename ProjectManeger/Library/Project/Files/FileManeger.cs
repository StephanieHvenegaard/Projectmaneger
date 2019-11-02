using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;
using SHUtils.Collections;
using SHUtils.Logging;

namespace ProjectManager25.Library.Project.Files
{
          
    [Serializable]
    class FileManeger : ISerializable
    {
        // Fields
        //----------------------------------------------------------------------------------------
        private ProjectFolder _RootLibary;
        public event EventHandler FileListHasChanged;
        // Constructor
        //----------------------------------------------------------------------------------------
        public FileManeger(ProjectFolder rootLibary)
        {
            rootLibary.IsRoot = true;
            _RootLibary = rootLibary;
        }
        // Events handling
        //----------------------------------------------------------------------------------------
        private void OnFileListChanged()
        {
            if(FileListHasChanged != null)
            {
                FileListHasChanged(this, new EventArgs());
            }
        }
        // General Funktions
        //----------------------------------------------------------------------------------------
        public void Add(ProjectFile item)
        {
            if( item.FileType == ProjectFile.Type.Image)
            {
                switch (Path.GetExtension(item.FileName).ToLower())
                {
                    case ".jpg":
                        item.FileType = ProjectFile.Type.JPG;
                        break;
                    case ".png":
                        item.FileType = ProjectFile.Type.PNG;
                        break;
                    case ".tga":
                        item.FileType = ProjectFile.Type.TGA;
                        break;
                    case ".gif":
                        item.FileType = ProjectFile.Type.GIF;
                        break;
                    case ".bmp":
                        item.FileType = ProjectFile.Type.BMP;
                        break;
                }
            }
            AddFile(item);

            OnFileListChanged();
        }
        public void AddShortcut(ProjectFile item)
        {
            Log.System(string.Format("Adding shortcut to Filelist for {0}",item.FileName));
            string Shortcutfolderpath = Path.Combine(new string[] { _RootLibary.FullFilePath, "ShortCuts" });
            // shortcut file name
            string linkName = Path.ChangeExtension(Shortcutfolderpath + @"\" + item.FileName, ".lnk");
            // COM object instance/props
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut sc = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(linkName);
            sc.Description = "Crated With Project maneger 2.5";
            //shortcut.IconLocation = @"C:\..."; 
            sc.TargetPath = item.FullFilePath;
            // save shortcut to target
            sc.Save();
            Log.System(string.Format("Succesfully created shortcut to Filelist for : {0} at : {1}", item.FileName, Shortcutfolderpath));

            ProjectFile shortcutFile = new ProjectFile(linkName, item.FileType);
            _RootLibary.Add(shortcutFile);
        }
        private void WriteShortcut(ProjectFile location, ProjectFile item)
        {
            var wsh = new IWshRuntimeLibrary.IWshShell_Class();
            IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(location.FullFilePath) as IWshRuntimeLibrary.IWshShortcut;
            shortcut.TargetPath = item.FullFilePath;
            shortcut.Save();
        }
        private void AddFile(ProjectFile item)
        {
            if (item.FilePath.Contains(_RootLibary.FilePath))
            {
                _RootLibary.Add(item);
            }
            else
            {
                DialogResult dr = MessageBox.Show("File is outside of the local Root directory for this project, do you whant to make a shortcut?", "Question", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    AddShortcut(item);
                }
                else if (dr == DialogResult.No)
                {
                    AddFile(item);
                }
                else
                {
                    Log.System("Abandoned Adding File");
                    return;
                }
            }
        }
        public ProjectFile GetFiles()
        {
            return _RootLibary;
        }        
        // General static Funktions
        //----------------------------------------------------------------------------------------
        public static string OpenFolder(string startPath)
        {
            FolderBrowserDialog SetFolder = new FolderBrowserDialog();
            SetFolder.ShowNewFolderButton = true;
            SetFolder.SelectedPath = startPath;
            SetFolder.ShowDialog();
            return SetFolder.SelectedPath;
        }
        public static string OpenFile(ProjectFile.Type fileType)
        {
          return OpenFile("", fileType);
        }
        public static string OpenFile(string startPath, ProjectFile.Type fileType)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = startPath;
            switch(fileType)
            {
                case ProjectFile.Type.Image:
                    openfile.Filter = FileDialogFilter.AllImageTypes;
                    break;
                case ProjectFile.Type.sln:
                    openfile.Filter = FileDialogFilter.SLN;
                    break;
                case ProjectFile.Type.WordDoc:
                    openfile.Filter = FileDialogFilter.AllWordTypes;
                    break;
                case ProjectFile.Type.Excel:
                    openfile.Filter = FileDialogFilter.AllExcelTypes;
                    break;
                case ProjectFile.Type.PowerPoint:
                    openfile.Filter = FileDialogFilter.AllPwrPointTypes;
                    break;
                case ProjectFile.Type.Zip:
                    openfile.Filter = FileDialogFilter.AllCompresedTypes;
                    break;
                case ProjectFile.Type.Xml:
                    openfile.Filter = FileDialogFilter.XML;
                    break;
                default:
                    openfile.Filter = "All Files (*.*)|*.*";
                    break;
            }
            openfile.ShowDialog();
            return openfile.FileName;
        }
        public static string SaveProjectDialog(string startPath)
        {
            return SaveProjectDialog(startPath, "");
        }
        public static string SaveProjectDialog(string startPath, string fileName)
        {
            SaveFileDialog savefiledialog = new SaveFileDialog();
            savefiledialog.InitialDirectory = startPath;
            savefiledialog.FileName = fileName;
            savefiledialog.Filter = string.Format("Project Files (*{0})|*{0}",Properties.Settings.Default.FileExtensionPm2);
            DialogResult dr = savefiledialog.ShowDialog();
            if (dr != DialogResult.Cancel) return savefiledialog.FileName;
            else return "";
        }
        // Serializing
        //----------------------------------------------------------------------------------------
        public FileManeger(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            FileListHasChanged += (EventHandler)info.GetValue("Event",typeof (EventHandler));     
            _RootLibary = (ProjectFolder)info.GetValue("Root", typeof(ProjectFolder));
        }
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("Root", _RootLibary);
            info.AddValue("Event", FileListHasChanged);
        }
     
    }
}
