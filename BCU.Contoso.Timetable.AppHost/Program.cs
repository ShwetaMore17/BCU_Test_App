namespace BCU.Contoso.Timetable.AppHost;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var sql = builder.AddSqlServer("sql")
            .WithContainerRuntimeArgs("-p", "1433:1433")
            .WithDataVolume()
            .WithLifetime(ContainerLifetime.Persistent);

        var db = sql.AddDatabase("timetable");

        var apiService = builder.AddProject<Projects.BCU_Contoso_Timetable_ApiService>("apiservice")
            .WithReference(db)
            .WaitFor(db);

        builder.AddProject<Projects.BCU_Contoso_Timetable_Web>("webfrontend")
            .WithExternalHttpEndpoints()
            .WithReference(apiService)
            .WaitFor(apiService);

        builder.Build().Run();
    }
}