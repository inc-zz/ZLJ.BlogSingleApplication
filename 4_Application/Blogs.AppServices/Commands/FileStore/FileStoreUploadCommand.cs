using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.FileStore
{
    public class FileStoreUploadCommand:FileStoreCommand
    {
        public string BusinessType { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public FileStoreUploadCommand(string fileName, string folderName, string businessType, string? description, string createdBy)
        {
            FileName = fileName;
            FolderName = folderName;
            BusinessType = businessType;
            Description = description;
            CreatedBy = createdBy;
        }
        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(BusinessType) && !string.IsNullOrEmpty(CreatedBy);
        }
    }
}
