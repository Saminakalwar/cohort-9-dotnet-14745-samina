namespace TaskManagement.Application.DTOs.Dashboard;
public class DashboardResponse
{
    public int Pending{ get; set; }
    public int InProgress{ get; set; }
    public int Completed{ get; set; }
}