using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.FileStore
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public abstract class FileStoreCommand : Command
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        public string FolderName { get; set; }

    }
}
