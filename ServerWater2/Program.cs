using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using ServerWater2.Models;
using ServerWater2.APIs;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace ServerWater2;
public class Program
{
    public class ItemHost
    {
        public List<string> host { get; set; } = new List<string>();
    }
    
    
    public class HttpNotification
    {
        public string id { get; set; } = "";
        public string mToken { get; set; } = "";
        public string state { get; set; } = "";
        public bool isOnline { get; set; } = false;
        public List<string> messagers { get; set; } = new List<string>();

    }

    public static MyRole api_role = new MyRole();
    public static MyPoint api_point = new MyPoint();
    public static MyArea api_area = new MyArea();
    public static MyLayer api_layer = new MyLayer();
    public static MySchedule api_schedule = new MySchedule();
    public static MyStatus api_status = new MyStatus();
    public static MyDevice api_device = new MyDevice();
    public static MyUser api_user = new MyUser();
    public static MyFile api_file = new MyFile();
    public static MyOrder api_order = new MyOrder();
    public static MyType api_type = new MyType();
    public static MyState api_state = new MyState();
    public static MyService api_service = new MyService();
    public static MyAction api_action = new MyAction();
    public static MyLogOrder api_log = new MyLogOrder();
    public static List<HttpNotification> httpNotifications = new List<HttpNotification>();
    //public static List<DataNotification> dataNotifications = new List<DataNotification>();


    public static MyCustomer api_customer = new MyCustomer();

    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .WriteTo.Console(theme: AnsiConsoleTheme.Code)
               .WriteTo.File("mylog.txt", rollingInterval: RollingInterval.Day)
               //.WriteTo.Seq("http://log.smartlook.com.vn:8090", apiKey: "FTidLKHRnxm8y7BaUNvX")
               .CreateLogger();

        try
        {
            string path = "./Configs";
            string link = Path.Combine(path, "configSql.json");

            if (!File.Exists(link))
            {
                bool flag = Program.api_file.createConfig("configSql");
                if (!flag)
                {
                    while (true)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("DB null !!! ");
                    }
                }
            }

            string file = Program.api_file.getFileConfig();

            ItemHost? tmp = JsonConvert.DeserializeObject<ItemHost>(file);
            if (tmp != null)
            {
                if (tmp.host.Count > 0)
                {
                    DataContext.configSql = tmp.host[0];
                }
            }

            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel((context, option) =>
            {
                option.ListenAnyIP(50000, listenOptions =>
                {

                });
                option.Limits.MaxConcurrentConnections = 1000;
                option.Limits.MaxRequestBodySize = null;
                option.Limits.MaxRequestBufferSize = null;
            });
            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("HTTPSystem", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).WithExposedHeaders("Grpc-Status", "Grpc-Encoding", "Grpc-Accept-Encoding");
                });
            });
            builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(DataContext.configSql));
            // builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseDeveloperExceptionPage();
            //app.UseMigrationsEndPoint();

            using (var scope = app.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                DataContext datacontext = services.GetRequiredService<DataContext>();
                datacontext.Database.EnsureCreated();
                await datacontext.Database.MigrateAsync();
            }

            app.UseCors("HTTPSystem");
            app.UseRouting();

            app.UseAuthorization();
            Log.Information("Connected to Server : " + DataContext.configSql);

            app.MapControllers();
            app.MapGet("/", () => string.Format("ServerWater2 of STVG - Version 1 : {0}", DateTime.Now));
            await api_role.initAsync();
            await api_user.initAsync();
            await api_service.initAsync();
            await api_device.initAsync();
            await api_state.initAsync();
            await api_type.initAsync();
            await api_action.initAsync();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }

        Log.CloseAndFlush();
    }
}