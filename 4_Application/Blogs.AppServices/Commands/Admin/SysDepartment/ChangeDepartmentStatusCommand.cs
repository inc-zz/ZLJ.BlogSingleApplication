using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysDepartment
{
    /// <summary>
    /// 
    /// </summary>
    public class ChangeDepartmentStatusCommand : DepartmentCommand
    {

        /// <summary>
        /// 
        /// </summary>
        public ChangeDepartmentStatusCommand(long id,int status,string remark)
        {
            Id = id;
            Status = status;
            Description = remark;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return Id > 0;
        }
    }
}
