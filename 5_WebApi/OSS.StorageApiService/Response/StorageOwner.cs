namespace OSS.StorageApiService
{
    public class StorageOwner
    {

        public StorageOwner(string id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        /// <summary>
        /// The unique identifier of the owner.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }

    }
}
