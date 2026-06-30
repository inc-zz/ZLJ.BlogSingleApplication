using System;
using System.Collections.Generic;
using System.Text;

namespace OSS.StorageApiService;

/// <summary>
/// 对象所有者
/// </summary>
public class StorageObjectOwner
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public StorageObjectOwner(string id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    /// <summary>
    /// 所有者Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 所有者DisplayName
    /// </summary>
    public string Name { get; set; }
}
