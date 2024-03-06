using System.Text.Json.Serialization;

namespace TaskManagementAPI;

public class Task
{

    public Guid IdTask {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public string Icon {get; set;}
    public Status Status {get; set;}   
    public DateTime CreationDate {get; set;}
    public DateTime Deadline {get; set;}
}

public enum Status
{
    ToDo, InProgress, Completed, WontDo
}
