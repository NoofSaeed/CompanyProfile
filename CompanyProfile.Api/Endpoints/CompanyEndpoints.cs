using CompanyProfile.Shared.Common;
using CompanyProfile.Api.Filters;
using CompanyProfile.Api.Services;
using CompanyProfile.Shared.DTOs;

namespace CompanyProfile.Api.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        var publicGroup = app.MapGroup("/api")
            .WithTags("Public Website API");

        publicGroup.MapGet("/info", GetCompanyInfo)
            .WithSummary("جلب معلومات الشركة باللغة المطلوبة");

        publicGroup.MapGet("/services", GetServices)
            .WithSummary("جلب خدمات الشركة باللغة المطلوبة");

        publicGroup.MapGet("/services/{id}", GetServiceById)
            .WithSummary("جلب خدمة الشركة باللغة المطلوبة");

        publicGroup.MapPost("/contact", CreateContactMessage)
            .AddEndpointFilter<ValidationFilter<CreateContactMessageDto>>()
            .WithSummary("إرسال رسالة تواصل جديدة");

        var adminGroup = app.MapGroup("/api/admin")
            .RequireAuthorization(policy => policy.RequireRole("Admin"))
            .WithTags("Admin Dashboard API");

        adminGroup.MapGet("/servicesWithAllTrans/{id}", GetServiceByIdWithAllTrans)
            .WithSummary("جلب خدمة معينة مع جميع الترجمات");

        adminGroup.MapGet("/infoWithTran", GetCompanyInfoWithAllTrans)
            .WithSummary("جلب معلومات الشركة بجميع اللغات");

        adminGroup.MapPut("/{language}/info", UpdateCompanyInfo)
            .AddEndpointFilter<ValidationFilter<UpdateCompanyInfoDto>>()
            .WithSummary("تعديل معلومات الشركة باللغة المحددة");

        adminGroup.MapPost("/{language}/services", AddService)
            .AddEndpointFilter<ValidationFilter<CreateServiceDto>>()
            .WithSummary("إضافة خدمة باللغة المحددة");

        adminGroup.MapPut("/{language}/services/{id}", UpdateService)
            .AddEndpointFilter<ValidationFilter<UpdateServiceDto>>()
            .WithSummary("تعديل خدمة باللغة المحددة");

        adminGroup.MapDelete("/services/{id}", DeleteService)
            .WithSummary("حذف خدمة نهائياً من قاعدة البيانات");
    }

    private static async Task<IResult> GetCompanyInfo(
        CompanyService companyService,
        ILanguageContext languageContext)
    {
        var data = await companyService.GetCompanyInfoAsync(languageContext.Language);

        if (data is null)
            return Results.NotFound(ApiResponse<CompanyInfoResponseDto>.FailureResponse("بيانات الشركة غير موجودة."));

        return Results.Ok(ApiResponse<CompanyInfoResponseDto>.SuccessResponse(data, ApiMessages.Success));
    }

    private static async Task<IResult> GetCompanyInfoWithAllTrans(CompanyService companyService)
    {
        var data = await companyService.GetCompanyInfoWithAllTransAsync();

        if (data is null)
            return Results.NotFound(ApiResponse<CompanyInfoWithTranslationsResponseDto>.FailureResponse("بيانات الشركة غير موجودة."));

        return Results.Ok(ApiResponse<CompanyInfoWithTranslationsResponseDto>.SuccessResponse(data, ApiMessages.Success));
    }

    private static async Task<IResult> GetServices(
        CompanyService companyService,
        ILanguageContext languageContext)
    {
        var services = await companyService.GetServicesAsync(languageContext.Language);
        return Results.Ok(ApiResponse<List<ServiceResponseDto>>.SuccessResponse(services, ApiMessages.Success));
    }

    private static async Task<IResult> GetServiceById(
        int id,
        CompanyService companyService,
        ILanguageContext languageContext)
    {
        var service = await companyService.GetServiceByIdAsync(id, languageContext.Language);

        if (service is null)
            return Results.NotFound(ApiResponse<ServiceResponseDto>.FailureResponse("الخدمة غير موجودة."));

        return Results.Ok(ApiResponse<ServiceResponseDto>.SuccessResponse(service, ApiMessages.Success));
    }

    private static async Task<IResult> GetServiceByIdWithAllTrans(int id, CompanyService companyService)
    {
        var service = await companyService.GetServiceByIdWithAllTransAsync(id);

        if (service is null)
            return Results.NotFound(ApiResponse<ServiceWithTranslationsResponseDto>.FailureResponse("الخدمة غير موجودة."));

        return Results.Ok(ApiResponse<ServiceWithTranslationsResponseDto>.SuccessResponse(service, ApiMessages.Success));
    }

    private static async Task<IResult> CreateContactMessage(CreateContactMessageDto inputDto, CompanyService companyService)
    {
        var responseDto = await companyService.CreateContactMessageAsync(inputDto);

        return Results.Created(
            $"/api/contact/{responseDto.Id}",
            ApiResponse<ContactMessageResponseDto>.SuccessResponse(responseDto, ApiMessages.Success));
    }

    private static async Task<IResult> UpdateCompanyInfo(string language, UpdateCompanyInfoDto dto, CompanyService companyService)
    {
        var responseDto = await companyService.UpdateCompanyInfoAsync(language, dto);

        if (responseDto is null)
            return Results.NotFound(ApiResponse<CompanyInfoResponseDto>.FailureResponse("بيانات الشركة غير موجودة."));

        return Results.Ok(ApiResponse<CompanyInfoResponseDto>.SuccessResponse(responseDto, ApiMessages.Success));
    }

    private static async Task<IResult> AddService(string language, CreateServiceDto dto, CompanyService companyService)
    {
        var responseDto = await companyService.AddServiceAsync(language, dto);

        return Results.Created(
            $"/api/services/{responseDto.Id}",
            ApiResponse<ServiceResponseDto>.SuccessResponse(responseDto, ApiMessages.Success));
    }

    private static async Task<IResult> UpdateService(int id, string language, UpdateServiceDto dto, CompanyService companyService)
    {
        var responseDto = await companyService.UpdateServiceAsync(id, language, dto);

        if (responseDto is null)
            return Results.NotFound(ApiResponse<ServiceResponseDto>.FailureResponse("الخدمة غير موجودة."));

        return Results.Ok(ApiResponse<ServiceResponseDto>.SuccessResponse(responseDto, ApiMessages.Success));
    }

    private static async Task<IResult> DeleteService(int id, CompanyService companyService)
    {
        var isDeleted = await companyService.DeleteServiceAsync(id);

        if (!isDeleted)
            return Results.NotFound(ApiResponse<string>.FailureResponse("الخدمة المطلوبة غير موجودة."));

        return Results.Ok(ApiResponse<string>.SuccessResponse("تم حذف الخدمة وترجماتها بنجاح.", ApiMessages.Success));
    }
}