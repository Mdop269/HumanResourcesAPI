using HumanResources.DataServices.Abstract;
using HumanResources.DataServices.Services;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Managers.HR;
using HumanResources.Manager.Managers.Review;
using HumanResourcesAPI.EntityData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Human Resource Data Services 
builder.Services.AddTransient<IDepartmentDataServices, DepartmentDataServices>();
builder.Services.AddTransient<IRoleDataServices, RoleDataServices>();
builder.Services.AddTransient<IPermissionsDataServices, PermissionsDataServices>();
builder.Services.AddTransient<ITeamRolesDataServices, TeamRolesDataServices>();
builder.Services.AddTransient<IStatusDataServices, StatusDataServices>();
builder.Services.AddTransient<IHRDataServices, HRDataServices>();
builder.Services.AddTransient<ITeamsDataServices, TeamsDataServices>();
builder.Services.AddTransient<IEmployeeDataServices, EmployeeDataServices>();
builder.Services.AddTransient<IAttendanceDataServices, AttendanceDataServices>();
builder.Services.AddTransient<ILeaveDataServices, LeaveDataServices>();
builder.Services.AddTransient<ISalaryDataServices, SalaryDataServices>();
builder.Services.AddTransient<IReviewDataServices, ReviewDataServices>();
builder.Services.AddTransient<IBonusDataServices, BonusDataServices>();
builder.Services.AddTransient<IPromotionDataServices, PromotionDataServices>();
builder.Services.AddTransient<ILoginDataServices, LoginDataServices>();


// Human Resource Manager 
builder.Services.AddTransient<DepartmentManager>();
builder.Services.AddTransient<RoleManager>();
builder.Services.AddTransient<PermissionsManager>();
builder.Services.AddTransient<TeamRolesManager>();
builder.Services.AddTransient<StatusManager>();
builder.Services.AddTransient<HrManager>();
builder.Services.AddTransient<TeamsManager>();
builder.Services.AddTransient<EmployeeManager>();
builder.Services.AddTransient<AttendanceManager>();
builder.Services.AddTransient<LeaveManager>();
builder.Services.AddTransient<SalaryManager>();
builder.Services.AddTransient<ReviewManager>();
builder.Services.AddTransient<BonusManager>();
builder.Services.AddTransient<PromotionManager>();
builder.Services.AddTransient<LoginManager>();




// for database 
builder.Services.AddDbContext<HumanResourcesContext>();

builder.Services.AddCors(options => options.AddPolicy(name: "FrontendUI",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();

        Console.WriteLine("CORS policy applied");

    }));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontendUI");

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
