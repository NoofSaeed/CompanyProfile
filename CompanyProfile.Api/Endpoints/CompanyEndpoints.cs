using CompanyProfile.Shared.Common;
using CompanyProfile.Api.Filters;
using CompanyProfile.Api.Services;
using CompanyProfile.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

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

        publicGroup.MapGet("/team", GetTeamMembers)
            .WithSummary("جلب أعضاء الفريق باللغة المطلوبة");

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

        adminGroup.MapPost("/{language}/team", AddTeamMember)
            .WithSummary("إضافة عضو إلى الفريق باللغة المحددة");

        adminGroup.MapPost("/{language}/team/upload", AddTeamMemberWithImage)
            .DisableAntiforgery()
            .WithSummary("إضافة عضو إلى الفريق مع صورة");

        adminGroup.MapPost("/{language}/team/{id}/translation", AddTeamMemberTranslation)
            .AddEndpointFilter<ValidationFilter<CreateTeamMemberTranslationDto>>()
            .WithSummary("إضافة ترجمة لعضو فريق موجود");

        adminGroup.MapPut("/{language}/team/{id}", UpdateTeamMember)
            .WithSummary("تعديل عضو في الفريق");

        adminGroup.MapPut("/{language}/team/{id}/upload", UpdateTeamMemberWithImage)
            .DisableAntiforgery()
            .WithSummary("تعديل عضو في الفريق مع صورة");

        adminGroup.MapDelete("/team/{id}", DeleteTeamMember)
            .WithSummary("حذف عضو من الفريق");

        adminGroup.MapGet("/contact", GetContactMessages)
            .WithSummary("عرض رسائل الزوار");

        adminGroup.MapDelete("/contact/{id}", DeleteContactMessage)
            .WithSummary("حذف رسالة زائر");

        adminGroup.MapPut("/{language}/services/{id}", UpdateService)
            .AddEndpointFilter<ValidationFilter<UpdateServiceDto>>()
            .WithSummary("تعديل خدمة باللغة المحددة");

        adminGroup.MapPost("/{language}/services/{id}/translation", AddServiceTranslation)
            .AddEndpointFilter<ValidationFilter<CreateServiceTranslationDto>>()
            .WithSummary("إضافة ترجمة لخدمة موجودة");

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

    private static async Task<IResult> GetTeamMembers(
        CompanyService companyService,
        ILanguageContext languageContext)
    {
        var team = await companyService.GetTeamMembersAsync(languageContext.Language);
        return Results.Ok(ApiResponse<List<TeamMemberResponseDto>>.SuccessResponse(team, ApiMessages.Success));
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

    private static async Task<IResult> AddTeamMember(
        string language,
        CreateTeamMemberAdminDto dto,
        CompanyService companyService)
    {
        var responseDto = await companyService.AddTeamMemberAsync(language, dto);

        return Results.Created(
            $"/api/team/{responseDto.Id}",
            ApiResponse<TeamMemberResponseDto>.SuccessResponse(responseDto, ApiMessages.Success));
    }

    private static async Task<IResult> AddTeamMemberWithImage(
        string language,
        [FromForm] string name,
        [FromForm] string role,
        [FromForm] string bio,
        IFormFile image,
        CompanyService companyService,
        IWebHostEnvironment environment,
        HttpRequest request)
    {
        var imageUrl = await SaveTeamImageAsync(image, environment, request);
        var responseDto = await companyService.AddTeamMemberAsync(
            language,
            new CreateTeamMemberAdminDto(imageUrl, name, role, bio));

        return Results.Created(
            $"/api/team/{responseDto.Id}",
            ApiResponse<TeamMemberResponseDto>.SuccessResponse(responseDto, ApiMessages.Success));
    }

    private static async Task<IResult> UpdateService(int id, string language, UpdateServiceDto dto, CompanyService companyService)
    {
        var responseDto = await companyService.UpdateServiceAsync(id, language, dto);

        if (responseDto is null)
            return Results.NotFound(ApiResponse<ServiceResponseDto>.FailureResponse("الخدمة غير موجودة."));

        return Results.Ok(ApiResponse<ServiceResponseDto>.SuccessResponse(responseDto, ApiMessages.Success));
    }

    private static async Task<IResult> AddServiceTranslation(
        int id,
        string language,
        CreateServiceTranslationDto dto,
        CompanyService companyService)
    {
        var responseDto = await companyService.AddServiceTranslationAsync(id, language, dto);

        if (responseDto is null)
            return Results.Conflict(ApiResponse<ServiceResponseDto>.FailureResponse("الخدمة غير موجودة أو الترجمة موجودة مسبقاً."));

        return Results.Created(
            $"/api/services/{id}",
            ApiResponse<ServiceResponseDto>.SuccessResponse(responseDto, ApiMessages.Success));
    }

    private static async Task<IResult> UpdateTeamMember(
        int id,
        string language,
        UpdateTeamMemberAdminDto dto,
        CompanyService companyService)
    {
        var responseDto = await companyService.UpdateTeamMemberAsync(id, language, dto);

        if (responseDto is null)
            return Results.NotFound(ApiResponse<TeamMemberResponseDto>.FailureResponse("عضو الفريق غير موجود."));

        return Results.Ok(ApiResponse<TeamMemberResponseDto>.SuccessResponse(responseDto, ApiMessages.Success));
    }

    private static async Task<IResult> AddTeamMemberTranslation(
        int id,
        string language,
        CreateTeamMemberTranslationDto dto,
        CompanyService companyService)
    {
        var responseDto = await companyService.AddTeamMemberTranslationAsync(id, language, dto);
        if (responseDto is null)
            return Results.Conflict(ApiResponse<TeamMemberResponseDto>.FailureResponse("عضو الفريق غير موجود أو الترجمة موجودة مسبقاً."));

        return Results.Created($"/api/team/{id}", ApiResponse<TeamMemberResponseDto>.SuccessResponse(responseDto, ApiMessages.Success));
    }

    private static async Task<IResult> UpdateTeamMemberWithImage(
        int id,
        string language,
        [FromForm] string name,
        [FromForm] string role,
        [FromForm] string bio,
        IFormFile image,
        CompanyService companyService,
        IWebHostEnvironment environment,
        HttpRequest request)
    {
        var imageUrl = await SaveTeamImageAsync(image, environment, request);
        var responseDto = await companyService.UpdateTeamMemberAsync(
            id,
            language,
            new UpdateTeamMemberAdminDto(imageUrl, name, role, bio));

        if (responseDto is null)
            return Results.NotFound(ApiResponse<TeamMemberResponseDto>.FailureResponse("عضو الفريق غير موجود."));

        return Results.Ok(ApiResponse<TeamMemberResponseDto>.SuccessResponse(responseDto, ApiMessages.Success));
    }

    private static async Task<IResult> DeleteTeamMember(int id, CompanyService companyService)
    {
        var deleted = await companyService.DeleteTeamMemberAsync(id);
        return deleted
            ? Results.Ok(ApiResponse<string>.SuccessResponse("تم حذف عضو الفريق.", ApiMessages.Success))
            : Results.NotFound(ApiResponse<string>.FailureResponse("عضو الفريق غير موجود."));
    }

    private static async Task<IResult> GetContactMessages(CompanyService companyService)
    {
        var messages = await companyService.GetContactMessagesAsync();
        return Results.Ok(ApiResponse<List<ContactMessageAdminDto>>.SuccessResponse(messages, ApiMessages.Success));
    }

    private static async Task<IResult> DeleteContactMessage(int id, CompanyService companyService)
    {
        var deleted = await companyService.DeleteContactMessageAsync(id);
        return deleted
            ? Results.Ok(ApiResponse<string>.SuccessResponse("تم حذف الرسالة.", ApiMessages.Success))
            : Results.NotFound(ApiResponse<string>.FailureResponse("الرسالة غير موجودة."));
    }

    private static async Task<string> SaveTeamImageAsync(IFormFile image, IWebHostEnvironment environment, HttpRequest request)
    {
        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        if (!allowedExtensions.Contains(extension) || image.Length > 5 * 1024 * 1024)
            throw new InvalidOperationException("Only JPG, PNG and WEBP images up to 5 MB are allowed.");

        var folder = Path.Combine(environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot"), "uploads", "team");
        Directory.CreateDirectory(folder);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(folder, fileName);

        await using var stream = File.Create(filePath);
        await image.CopyToAsync(stream);
        return $"{request.Scheme}://{request.Host}{request.PathBase}/uploads/team/{fileName}";
    }

    private static async Task<IResult> DeleteService(int id, CompanyService companyService)
    {
        var isDeleted = await companyService.DeleteServiceAsync(id);

        if (!isDeleted)
            return Results.NotFound(ApiResponse<string>.FailureResponse("الخدمة المطلوبة غير موجودة."));

        return Results.Ok(ApiResponse<string>.SuccessResponse("تم حذف الخدمة وترجماتها بنجاح.", ApiMessages.Success));
    }
}