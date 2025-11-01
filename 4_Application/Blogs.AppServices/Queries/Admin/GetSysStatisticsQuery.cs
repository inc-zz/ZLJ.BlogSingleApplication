using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.Admin
{
    public class GetSysStatisticsQuery: IRequest<ResultObject<HomeStatisticsDto>>
    {

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

    }
}
