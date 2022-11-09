using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AWSDynamoDB.Models;


namespace AWSDynamoDB.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IAmazonDynamoDB _dynamoDB;
    private readonly DynamoDBContext _dbContext;

    [BindProperty]
    public Note NewNote { get; set; } = new();

    public List<Note> notes = new();

    public IndexModel(ILogger<IndexModel> logger, IAmazonDynamoDB amazonDynamoDB)
    {
        _logger = logger;
        _dynamoDB = amazonDynamoDB;
        _dbContext = new DynamoDBContext(amazonDynamoDB);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        NewNote.Id = Guid.NewGuid().ToString();
        await _dbContext.SaveAsync<Note>(NewNote);
        return RedirectToAction("Get");
    }

    public async Task OnGetAsync()
    {
        var scanOperation = _dbContext.ScanAsync<Note>(new List<ScanCondition>());
        if (scanOperation == null)
        {
            return;
        }
        var data = await scanOperation.GetRemainingAsync();
        if (data != null)
        {
            notes = data;
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        await _dbContext.DeleteAsync<Note>(id);
        return RedirectToAction("Get");
    }
}
