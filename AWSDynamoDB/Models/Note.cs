using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace AWSDynamoDB.Models;

[DynamoDBTable("notes")]
public class Note
{
    public string? Id { get; set; }

    [Required]
    public string? Title { get; set; }
    [Required]

    public string? Message { get; set; }
    // Property to store version number for optimistic locking.
    [DynamoDBVersion]
    public int? Version { get; set; }
}