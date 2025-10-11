using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.AppServices.Responses;
using Blogs.Core.DtoModel.Admin;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Admin;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.QueryHandlers.Admin
{
    public class DepartmentQueryHandler : SqlSugarDbContext,
        IRequestHandler<GetDepartmentTreeQuery, List<DepartmentTreeDto>>,
        IRequestHandler<GetDepartmentListQuery, PagedResult<SysDepartmentDto>>
    {

        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentQueryHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<List<DepartmentTreeDto>> Handle(GetDepartmentTreeQuery query, CancellationToken cancellationToken)
        {
            var list = await _departmentRepository.GetDepartmentTreeAsync();

            var result = list.Adapt<List<DepartmentTreeDto>>();

            return result;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResult<SysDepartmentDto>> Handle(GetDepartmentListQuery request, CancellationToken cancellationToken)
        {
            RefAsync<int> total = 0;
            var list = await DbContext.Queryable<SysDepartment>()
                .WhereIF(request.DepId > 0, it => it.ParentId == request.DepId || it.Id == request.DepId)
                .ToTreeAsync(it => it.Children, it => it.ParentId, 0);

            var jsonData = JsonConvert.SerializeObject(list);
            var newList = ConvertToFlatList(list, 0);
            var result = new PagedResult<SysDepartmentDto>
            {
                code = 200,
                message = "获取成功",
                success = true,
                Items = newList.ToList(),
                Total = total.Value,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<SysDepartmentDto> ConvertToFlatList(List<SysDepartment> list, long parentId)
        {
            var newList = new List<SysDepartmentDto>();

            foreach (var item in list)
            {
                var dto = new SysDepartmentDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    ParentName = parentId == 0 ? "顶级部门" : list.FirstOrDefault(x => x.Id == parentId)?.Name,
                    Description = item.Description,
                    CreatedAt = item.CreatedAt,
                    CreatedBy = item.CreatedBy,
                    ModifiedAt = item.ModifiedAt,
                    ModifiedBy = item.ModifiedBy,
                    IsDeleted = item.IsDeleted == 1
                };

                newList.Add(dto);
                if (item.Children != null && item.Children.Count > 0)
                {
                    var childList = ConvertToFlatList(item.Children, item.Id);
                    dto.Children = childList;
                }
            }
            return newList;

        }

    }
}
