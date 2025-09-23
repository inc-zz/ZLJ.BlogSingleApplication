using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Config;

public class GlobalFilter
{
    public string Message { get; set; }
    public bool Enable { get; set; }
    public string[] Actions { get; set; }
}
