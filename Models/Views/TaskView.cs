using System.Text.Json.Serialization;

namespace TaskManagementAPI;

public class TaskView
{
    public string Name {get; set;}
    public string Description {get; set;}
    public string Icon {get; set;}
    public Status Status {get; set;}   
    public DateTime Deadline {get; set;}
}
