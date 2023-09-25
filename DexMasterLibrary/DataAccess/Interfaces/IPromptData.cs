namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IPromptData
{
    Task<List<Prompt>> GetAllActivePromptsAsync();
    Task ToggleFavouriteOnPromptAsync(Prompt prompt, string userId);
    Task<DTPagedResult<Prompt>> GetActivePagedResultsAsync(
        int pageNumber,
        int promptsPerPage,
        string searchTerm = "",
        SortOption? sortOption = SortOption.Default,
        string createdById = "");
    Task<Prompt> GetPromptByIdAsync(string id);
    Task<Prompt> GetRandomPromptAsync();
    Task IncreaseViewCountForPromptAsync(Prompt prompt);
    Task SharePromptAsync(Prompt prompt);
    Task UpdatePromptAsync(Prompt prompt);
    Task CreatePromptAsync(Prompt prompt);
    Task CreateMultiplePromptsAsync(IEnumerable<Prompt> prompts);
    Task<List<Prompt>> GetPromptsByIdsAsync(IEnumerable<string> promptIds);
    Task DeletePromptAsync(Prompt prompt);
}