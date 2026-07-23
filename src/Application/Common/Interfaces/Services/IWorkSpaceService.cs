namespace Application.Common.Interfaces.Services
{
    public interface IWorkSpaceService
    {
        Task<bool> IsWorkSpaceExistAsync(long workSpaceId);
    }
}
