namespace EntityFrameworkCore.BootKit;

public class DbConnectionSetting
{
    public DbConnectionSetting()
    {
        Slavers = [];
    }

    public string Master { get; set; }
    public string[] Slavers { get; set; }

    public int ConnectionTimeout { get; set; } = 30;

    public int ExecutionTimeout { get; set; } = 30;
}
