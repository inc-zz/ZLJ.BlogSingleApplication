using Blogs.AppServices.Commands.Admin.SysDepartment;

namespace Blogs.AppServices.ModelValidator.Admin.Department
{

    /// <summary>
    /// 修改部门
    /// </summary>
    public class UpdateDepartmentCommandValidation : DepartmentValidatorCommand<UpdateDepartmentCommand>
    {

        /// <summary>
        /// 
        /// </summary>
        public UpdateDepartmentCommandValidation()
        {
            ValidateId();
            ValidateName();
        }
    }
}
