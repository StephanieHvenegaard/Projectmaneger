using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager25.Library.Project.Files
{
    [Serializable]
    class ProjectFolder : ProjectFile 
    {
        // Fields
        //----------------------------------------------------------------------------------------
        private List<ProjectFile> _FilesInFolder = new List<ProjectFile>();
        private bool _isRoot=false;
        // Construktor
        //----------------------------------------------------------------------------------------
        public ProjectFolder(string fullFilePath)
        {
            this.FullFilePath = fullFilePath;
            if(!Directory.Exists(this.FullFilePath))
            {
                Directory.CreateDirectory(this.FullFilePath);
            }
        }
        // Properties
        //----------------------------------------------------------------------------------------
        public virtual string FilePath { get { return FullFilePath; } }
        public virtual string FileName { get { return null; } }
        public virtual string FileNameWithoutExtension { get { return null; } }
        public override ProjectFile.Type FileType { get { if (IsRoot)return ProjectFile.Type.RootFolder; else  return ProjectFile.Type.Folder; } } 
        public bool IsRoot { get { return _isRoot; } set { _isRoot = value; } }
        // General Funktions
        //----------------------------------------------------------------------------------------
        public ProjectFile[] GetFiles()
        {
            return _FilesInFolder.ToArray();
        }
        public void Add(ProjectFile item)
        {
            string Path = item.FilePath.Replace(this.FullFilePath,"");
            if (!string.IsNullOrEmpty(Path))
            {
                string[] folders = Path.Split('\\');
                if(folders.Length == 0)
                {
                    _FilesInFolder.Add(item);
                }
                else
                {
                    string newfolder = this.FullFilePath + "\\" + folders[1];
                    ProjectFolder folder = (ProjectFolder)this.Find(newfolder);
                    if(folder==null)
                    {
                        ProjectFolder pj = new ProjectFolder(newfolder);
                        pj.Add(item);
                        _FilesInFolder.Add(pj);
                    } 
                    else
                    {
                        ((ProjectFolder)_FilesInFolder[_FilesInFolder.IndexOf(folder)]).Add(item);
                    }
                }               
            }
            else
            {
                _FilesInFolder.Add(item);
            }
        }
        public bool Contains(ProjectFile item)
        {
            return _FilesInFolder.Contains(item);            
        }
        public ProjectFile Find(ProjectFile item)
        {
            return _FilesInFolder.Find(x => x.FullFilePath == item.FullFilePath);
        }
        public ProjectFile Find(string itemPath)
        {
            return _FilesInFolder.Find(x => x.FullFilePath==itemPath);
        }
        public void Remove(ProjectFile item)
        {
            _FilesInFolder.Remove(item);
        }
        // Serializing
        //----------------------------------------------------------------------------------------
        public ProjectFolder(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            FullFilePath = (string)info.GetValue("FilePath", typeof(string));
            _FilesInFolder = (List<ProjectFile>)info.GetValue("FilesInFolder", typeof(List<ProjectFile>));
            _isRoot = (bool)info.GetValue("Root", typeof(bool));
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("FilePath", FullFilePath);
            info.AddValue("FilesInFolder", _FilesInFolder);
            info.AddValue("Root", _isRoot);
        }
    }
}
