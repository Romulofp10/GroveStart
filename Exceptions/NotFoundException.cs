namespace GroveStart.Exceptions
{
    public class NotFoundException : Exception
    {
        public string ResourceType { get; set; }
        public object ResourceId { get; set; }

        public NotFoundException(string resourceType, object resourceId) 
            : base($"{resourceType} with id {resourceId} not found")
        {
            ResourceType = resourceType;
            ResourceId = resourceId;
        }
    }
}
