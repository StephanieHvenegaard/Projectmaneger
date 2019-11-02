using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProjectManager25.Library.Project.Files
{
    [Serializable]
    class ProjectFile : ISerializable, IEquatable<ProjectFile>
    {
        public enum Type
        {
            WordDoc,
            Excel,
            PowerPoint,
            ProjectPlan,
            sln,
            Checklist,
            Image,
            JPG,
            PNG,
            GIF,
            TGA,
            BMP,
            Folder,
            RootFolder,
            Zip,
            Xml,
            Link,
        }
        // Properties
        //----------------------------------------------------------------------------------------
        public virtual string FilePath { get { return Path.GetDirectoryName(FullFilePath); } }
        public virtual string FullFilePath { get; set; }
        public virtual string FileName { get { return Path.GetFileName(FullFilePath); } }
        public virtual string FileNameWithoutExtension { get { return Path.GetFileNameWithoutExtension(FullFilePath); } }
        public virtual ProjectFile.Type FileType { get; set; }
        // Constructor
        //----------------------------------------------------------------------------------------
        public ProjectFile()
        {
        }
        public ProjectFile(string fullFilePath, ProjectFile.Type type)
        {
            this.FileType = type;
            this.FullFilePath = fullFilePath;
        }
        // Serializing
        //----------------------------------------------------------------------------------------
        public ProjectFile(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            FullFilePath = (string)info.GetValue("FilePath", typeof(string));
            FileType = (ProjectFile.Type)info.GetValue("FileType", typeof(ProjectFile.Type));
        }
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("FilePath", FullFilePath);
            info.AddValue("FileType", FileType);
        }

        public bool Equals(ProjectFile other)
        {
            if (this.FullFilePath != other.FullFilePath) return false;
            return true;
        }
    }
}
